using AutoMapper;
using Data.Models;
using DataModels.Models.BacklogPriorities;
using DataModels.Models.Comments;
using DataModels.Models.Comments.Dtos;
using DataModels.Models.Projects;
using DataModels.Models.Projects.Dtos;
using DataModels.Models.WorkItems;
using DataModels.Models.WorkItems.Bugs.Dtos;
using DataModels.Models.WorkItems.Tasks.Dtos;
using DataModels.Models.WorkItems.Tests.Dtos;
using DataModels.Models.WorkItems.UserStory;
using DataModels.Models.WorkItems.UserStory.Dtos;

namespace Web.Automapper
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<Project, ProjectDto>().ReverseMap();
            CreateMap<ProjectDto, ProjectViewModel>().ReverseMap();
            CreateMap<ProjectDto, EditProjectInputModel>().ReverseMap();
            CreateMap<PaginatedProjectDto, PaginatedProjectViewModel>().ReverseMap();
            
            CreateMap<UserStory, UserStoryDto>().ReverseMap(); 
            CreateMap<UserStory, UserStoryAllDto>().ReverseMap(); 
            CreateMap<UserStory, UserStoryInputModel>().ReverseMap();
            CreateMap<UserStoryViewModel, UserStoryUpdateModel>().ReverseMap(); 
            CreateMap<UserStoryViewModel, UserStoryDto>().ReverseMap(); 
            CreateMap<UserStoryAllViewmodel, UserStoryAllDto>().ReverseMap();

            CreateMap<UserStoryTask, TaskAllDto>().ReverseMap();

            CreateMap<Test, TestAllDto>().ReverseMap();

            CreateMap<Bug, BugAllDto>().ReverseMap();

            CreateMap<BacklogPriority, BacklogPrioritiesDto>().ReverseMap();

            CreateMap<BacklogPrioritiesDto, BacklogPriorityDropDownModel>().ReverseMap();

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
