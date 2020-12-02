using AutoMapper;
using Data.Models;
using DataModels.Models.BacklogPriorities;
using DataModels.Models.Comments;
using DataModels.Models.Comments.Dtos;
using DataModels.Models.Projects;
using DataModels.Models.Projects.Dtos;
using DataModels.Models.WorkItems;
using DataModels.Models.WorkItems.Bugs.Dtos;
using DataModels.Models.WorkItems.Tasks;
using DataModels.Models.WorkItems.Tasks.Dtos;
using DataModels.Models.WorkItems.Tests;
using DataModels.Models.WorkItems.Tests.Dtos;
using DataModels.Models.WorkItems.UserStory;
using DataModels.Models.WorkItems.UserStory.Dtos;

namespace Web.Automapper
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            // Projects
            CreateMap<Project, ProjectDto>().ReverseMap();
            CreateMap<ProjectDto, ProjectViewModel>().ReverseMap();
            CreateMap<ProjectDto, EditProjectInputModel>().ReverseMap();
            CreateMap<PaginatedProjectDto, PaginatedProjectViewModel>().ReverseMap();
            
            // User stories
            CreateMap<UserStory, UserStoryDto>().ReverseMap(); 
            CreateMap<UserStory, UserStoryAllDto>().ReverseMap(); 
            CreateMap<UserStory, UserStoryInputModel>().ReverseMap();
            CreateMap<UserStory, UserStoryDropDownModel>().ReverseMap();
            CreateMap<UserStoryViewModel, UserStoryUpdateModel>().ReverseMap(); 
            CreateMap<UserStoryViewModel, UserStoryDto>().ReverseMap(); 
            CreateMap<UserStoryAllViewmodel, UserStoryAllDto>().ReverseMap();

            // User story tasks
            CreateMap<UserStoryTask, TaskAllDto>().ReverseMap();
            CreateMap<TaskInputModel, TaskInputModelDto>().ReverseMap();

            // Tests
            CreateMap<Test, TestAllDto>().ReverseMap();
            CreateMap<TestInputModel, TestInputModelDto>().ReverseMap();

            // Bugs
            CreateMap<Bug, BugAllDto>().ReverseMap();

            // Backlog priorities
            CreateMap<BacklogPriority, BacklogPrioritiesDto>().ReverseMap();
            CreateMap<BacklogPrioritiesDto, BacklogPriorityDropDownModel>().ReverseMap();

            // User story comments
            CreateMap<UserStoryComment, CommentDto>().ReverseMap();
            CreateMap<UserStoryComment, CommentViewModel>().ReverseMap()
                .ForMember(x => x.Description, opt => opt.MapFrom(x => x.SanitizedDescription));
            CreateMap<UserStoryComment, CommentInputModel>().ReverseMap(); //
            CreateMap<CommentViewModel, CommentDto>().ReverseMap();
            CreateMap<CommentViewModel, CommentInputModel>().ReverseMap();
            CreateMap<CommentsUpdateModel, CommentInputModel>().ReverseMap();
        }
    }
}
