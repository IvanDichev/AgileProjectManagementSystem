using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Models;
using Data.Models.Users;
using DataModels.Models.Projects;
using DataModels.Models.Projects.Dtos;
using DataModels.Models.Users;
using DataModels.Models.Users.Dtos;
using DataModels.Pagination;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Repo;
using Services.Notifications;
using Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Mailing;

namespace Services.Projects
{
    public class ProjectsService : IProjectsService
    {
        private readonly IRepository<Project> projectRepo;
        private readonly IMapper mapper;
        private readonly ILogger<ProjectsService> logger;
        private readonly IRepository<KanbanBoardColumn> boardRepo;
        private readonly IRepository<User> usersRepo;
        private readonly IEmailSender emailSender;
        private readonly IConfiguration config;
        private readonly UserManager<User> userManager;
        private readonly IRepository<TeamsUsers> teamUsersRepo;
        private readonly INotificationsService notificationsService;

        public ProjectsService(IRepository<Project> projectRepo, 
            IMapper mapper, 
            ILogger<ProjectsService> logger,
            IRepository<KanbanBoardColumn> boardRepo, 
            IRepository<User> usersRepo,
            IEmailSender emailSender,
            IConfiguration config,
            UserManager<User> userManager,
            IRepository<TeamsUsers> teamUsersRepo,
            INotificationsService notificationsService)
        {
            this.projectRepo = projectRepo;
            this.mapper = mapper;
            this.logger = logger;
            this.boardRepo = boardRepo;
            this.usersRepo = usersRepo;
            this.emailSender = emailSender;
            this.config = config;
            this.userManager = userManager;
            this.teamUsersRepo = teamUsersRepo;
            this.notificationsService = notificationsService;
        }

        public async Task<ProjectDto> GetAsync(int projectId)
        {
            var project = await this.projectRepo.AllAsNoTracking()
                .Where(x => x.Id == projectId)
                .ProjectTo<ProjectDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            project.Users = await GetUsersInProject(projectId);

            return project;
        }

        private async Task<ICollection<UserDto>> GetUsersInProject(int projectId)
        {
            var users = await this.usersRepo.AllAsNoTracking()
                .Where(x => x.TeamsUsers.Any(x => x.Team.ProjectId == projectId))
                .ProjectTo<UserDto>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            return users;
        }

        public async Task<PaginatedProjectDto> GetAllAsync(int userId, PaginationFilter paginationFilter)
        {
            var query = this.projectRepo.All();
            var allForUser = query.Where(x => x.Team.ProjectId == x.Id)
                .Where(p => p.Team.TeamsUsers.Any(x => x.TeamId == p.Team.Id && x.UserId == userId));

            var paginatedQuery = allForUser
                .OrderBy(x => x.Id)
                .Skip(paginationFilter.PageSize * (paginationFilter.PageNumber - 1))
                .Take(paginationFilter.PageSize);

            var paginatedResult = new PaginatedProjectDto()
            {
                PaginatedProjects = await paginatedQuery.ProjectTo<ProjectDto>(this.mapper.ConfigurationProvider).ToListAsync(),
                RecordsPerPage = paginationFilter.PageSize,
                TotalPages = (int)Math.Ceiling(await allForUser.CountAsync(x => x.Id == x.Id) / (double)paginationFilter.PageSize),
            };

            return paginatedResult;
        }

        public async Task CreateAsync(CreateProjectInputModel inputModel, int userId)
        {
            var project = new Project()
            {
                AddedOn = DateTime.UtcNow,
                Name = inputModel.Name,
                Description = inputModel.Description,
            };

            project.Team = new Team
            {
                AddedOn = DateTime.UtcNow,
                Name = project.Name + " Team"
            };

            project.Team.TeamsUsers.Add(new TeamsUsers()
            {
                UserId = userId,
                TeamId = project.Team.Id,
            });

            await AddDefaultKanbanBoardToProjectAsync(project);

            await this.projectRepo.AddAsync(project);
            await projectRepo.SaveChangesAsync();
        }

        private async Task AddDefaultKanbanBoardToProjectAsync(Project project)
        {
            var defaultBacklogColumnOptions = new KanbanBoardColumnOption()
            {
                AddedOn = DateTime.UtcNow,
                ColumnName = DefaultKanbanOptionsConstants.Backlog,
                MaxItems = DefaultKanbanOptionsConstants.BacklogMaxItems,
                PositionLTR = DefaultKanbanOptionsConstants.BacklogPosition,
            };

            var defaultDoingColumnOptions = new KanbanBoardColumnOption()
            {
                AddedOn = DateTime.UtcNow,
                ColumnName = DefaultKanbanOptionsConstants.InProgress,
                MaxItems = DefaultKanbanOptionsConstants.InProgressMaxItems,
                PositionLTR = DefaultKanbanOptionsConstants.InProgressPosition,
            };

            var defaultDoneColumnOptions = new KanbanBoardColumnOption()
            {
                AddedOn = DateTime.UtcNow,
                ColumnName = DefaultKanbanOptionsConstants.Done,
                MaxItems = DefaultKanbanOptionsConstants.DoneMaxItems,
                PositionLTR = DefaultKanbanOptionsConstants.DonePosition,
            };

            var backlogkanbanBoardColumn = new KanbanBoardColumn()
            {
                AddedOn = DateTime.UtcNow,
                KanbanBoardColumnOption = defaultBacklogColumnOptions,
            };

            var doingkanbanBoardColumn = new KanbanBoardColumn()
            {
                AddedOn = DateTime.UtcNow,
                KanbanBoardColumnOption = defaultDoingColumnOptions,
            };

            var donekanbanBoardColumn = new KanbanBoardColumn()
            {
                AddedOn = DateTime.UtcNow,
                KanbanBoardColumnOption = defaultDoneColumnOptions,
            };

            project.KanbanBoardColumnOptions.Add(defaultDoneColumnOptions);
            project.KanbanBoardColumnOptions.Add(defaultDoingColumnOptions);
            project.KanbanBoardColumnOptions.Add(defaultBacklogColumnOptions);

            await this.boardRepo.AddAsync(backlogkanbanBoardColumn);
            await this.boardRepo.AddAsync(doingkanbanBoardColumn);
            await this.boardRepo.AddAsync(donekanbanBoardColumn);

            var board = this.boardRepo.AllAsNoTracking()
                .Where(x => x.KanbanBoardColumnOption.ProjectId == 1);
            var boarde = this.boardRepo.AllAsNoTracking()
                .Where(x => x.KanbanBoardColumnOption.ProjectId == 3);
        }

        public bool IsNameTaken(string name)
        {
            return this.projectRepo.AllAsNoTracking()
                .Any(x => x.Name == name);
        }

        public async Task DeleteAsync(int id)
        {
            var toRemove = await this.projectRepo.All()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            this.projectRepo.Delete(toRemove);
            await this.projectRepo.SaveChangesAsync();
        }

        public async Task UpdateAsync(EditProjectInputModel editModel)
        {
            var project = await this.projectRepo.All()
                .Where(x => x.Id == editModel.ProjectId)
                .FirstOrDefaultAsync();

            project.Description = editModel.Description;
            project.ModifiedOn = DateTime.UtcNow;

            this.projectRepo.Update(project);

            await this.projectRepo.SaveChangesAsync();
        }

        public bool IsUserInProject(int projectId, int userId)
        {
            return this.projectRepo.AllAsNoTracking()
                .Where(x => x.Id == projectId)
                .Any(x => x.Team.TeamsUsers
                    .Any(x => x.UserId == userId));
        }

        public async Task<int> GetNextIdForWorkItemAsync(int projectId)
        {
            var project = this.projectRepo.All()
                .Where(x => x.Id == projectId)
                .FirstOrDefault();

            project.WorkItemsId += 1;
            this.projectRepo.Update(project);
            await this.projectRepo.SaveChangesAsync();
            return project.WorkItemsId;
        }

        public async Task<ICollection<UserDto>> GetUsersDropDown(int projectId)
        {
            var users = await this.usersRepo.AllAsNoTracking()
                .Where(x => x.IsPublic == true && (x.TeamsUsers.Any(x => x.Team.ProjectId != projectId) || x.TeamsUsers.Count == 0))
                .ProjectTo<UserDto>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            return users;
        }

        public async Task AddUserToProject(int userId, int projectId)
        {
            var project = await this.projectRepo.All()
                .Where(x => x.Id == projectId)
                .Include(x => x.Team)
                .FirstOrDefaultAsync();

            project.Team.TeamsUsers.Add(new TeamsUsers { UserId = userId, TeamId = project.Team.Id });
            await this.projectRepo.SaveChangesAsync();

            var message = EmailConstants.AddedToProject + project.Name;

            await this.notificationsService.AddNotificationToUserAsync(userId, message);

            // Send email to user to inform them about being added to a project.
            var user = await userManager.FindByIdAsync(userId.ToString());
            var email = new Email(this.config["EmailSenderInformation:Email"], 
                user.Email,
                message, 
                EmailConstants.AddedToProjectSubject);

            await this.emailSender.SendAsync(email, this.config["EmailSenderInformation:Password"],
                        this.config["EmailSenderOptions:SmtpServer"], int.Parse(this.config["EmailSenderOptions:Port"]));
        }

        public async Task RemoveUserFromProjectAsync(int userId, int projectId)
        {
            var project = await this.projectRepo.All()
                .Where(x => x.Id == projectId)
                .Include(x => x.Team)
                .ThenInclude(x => x.TeamsUsers)
                .FirstOrDefaultAsync();

            var teamUsersToRemove = project.Team.TeamsUsers.FirstOrDefault(x => x.UserId == userId && x.TeamId == project.Team.Id);

            this.teamUsersRepo.Delete(teamUsersToRemove);
            await teamUsersRepo.SaveChangesAsync();

            var message = EmailConstants.RemoveFromProject + project.Name;

            await this.notificationsService.AddNotificationToUserAsync(userId, message);

            // Send email to user to inform them about being added to a project.
            var user = await userManager.FindByIdAsync(userId.ToString());
            var email = new Email(this.config["EmailSenderInformation:Email"],
                user.Email,
                message,
                EmailConstants.RemoveFromProjectSubject);

            await this.emailSender.SendAsync(email, this.config["EmailSenderInformation:Password"],
                        this.config["EmailSenderOptions:SmtpServer"], int.Parse(this.config["EmailSenderOptions:Port"]));
        }

        public async Task<bool> IsLastUserInProjectAsync(int userId, int projectId)
        {
            var project = await this.projectRepo.All()
               .Where(x => x.Id == projectId)
               .Include(x => x.Team)
               .ThenInclude(x => x.TeamsUsers)
               .FirstOrDefaultAsync();

            var isLast = project.Team.TeamsUsers.Count == 1;

            return isLast;
        }
    }
}