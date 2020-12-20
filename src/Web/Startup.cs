using AutoMapper;
using Data;
using Data.Models.Users;
using Data.Seeding;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repo;
using Services.BacklogPriorities;
using Services.BoardColumns;
using Services.BurndownDatas;
using Services.Comments;
using Services.Notifications;
using Services.Projects;
using Services.Sprints;
using Services.TeamRoles;
using Services.WorkItems;
using Services.WorkItems.Bugs;
using Services.WorkItems.Tasks;
using Services.WorkItems.Tests;
using Services.WorkItems.UserStories;
using System;
using Utilities.Mailing;
using Web.Hangfire.Filters;
using Web.Middlewares;

namespace Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, Role>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;

                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;

                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            services.AddRazorPages().AddRazorRuntimeCompilation();

            services.AddAutoMapper(typeof(Startup));

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = Configuration["GoogleAuth:ClientId"];
                options.ClientSecret = Configuration["GoogleAuth:ClientSecret"];
            });

            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

            services.AddHangfireServer();
            services.AddMvc();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddTransient<Utilities.Mailing.IEmailSender, EmailSender>();
            services.AddTransient<IProjectsService, ProjectsService>();
            services.AddTransient<IWorkItemService, WorkItemService>();
            services.AddTransient<IBacklogPrioritiesService, BacklogPrioritiesService>();
            services.AddTransient<ICommentsService, CommentsService>();
            services.AddTransient<IUserStoryService, UserStoryService>();
            services.AddTransient<ITasksService, TasksService>();
            services.AddTransient<ITestsService, TestsService>();
            services.AddTransient<IBugsService, BugsService>();
            services.AddTransient<ISprintsService, SprintsService>();
            services.AddTransient<IBoardsService, BoardsService>();
            services.AddTransient<IBurndownDataService, BurndownDataService>();
            services.AddTransient<ITeamRolesService, TeamRolesService>();
            services.AddTransient<INotificationsService, NotificationsService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, 
            IRecurringJobManager recurringJobManager, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                using (var serviceScope = app.ApplicationServices.CreateScope())
                {
                    var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    dbContext.Database.Migrate();
                    new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
                }

                app.UseCookiePolicy();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseSeedAdminAndRolesMiddleware();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                            "areaRoute",
                            "{area:exists}/{controller=Home}/{action=Index}/{projectId?}/{id?}");

                endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller=Home}/{action=Index}/{projectId?}/{id?}");

                endpoints.MapHangfireDashboard();
                endpoints.MapRazorPages();
            });

            CallHangfireJobs(recurringJobManager, serviceProvider);
        }

        private void CallHangfireJobs(IRecurringJobManager recurringJobManager, IServiceProvider serviceProvider)
        {
            //recurringJobManager.AddOrUpdate<IPrintDemo>(
            //    "Run every minute Demo",
            //    x => x.Print("Called from hangfire recurring job!"),
            //    "* * * * *"
            //    );

            recurringJobManager.AddOrUpdate<ISprintsService>(
                "Update sprint status",
                x => x.UpdateSprintStatus(),
                "0 0 * * *"
                );

            recurringJobManager.AddOrUpdate<IBurndownDataService>(
                "Update burndown data",
                x => x.UpdateData(),
                "0 0 * * *"
                );
        }
    }
}
