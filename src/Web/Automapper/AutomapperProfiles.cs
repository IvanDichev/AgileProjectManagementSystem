using AutoMapper;
using Data.Models;
using DataModels.Models.BacklogPriorities;
using DataModels.Models.Comments;
using DataModels.Models.Comments.Dtos;
using DataModels.Models.Projects;
using DataModels.Models.Projects.Dtos;
using DataModels.Models.UserStories;
using DataModels.Models.UserStories.Dtos;
using System;

namespace Web.Automapper
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<Project, ProjectDto>().ReverseMap();
            CreateMap<Project, ProjectViewModel>().ReverseMap();
            CreateMap<Project, CreateProjectInputModel>().ReverseMap()
                .ForPath(m => m.Team.Name, opt => opt.MapFrom(x => x.Name + " Team"))
                .ForPath(m => m.Team.AddedOn, opt => opt.MapFrom(x => DateTime.Now))
                .ForMember(m => m.AddedOn, opt => opt.MapFrom(x => DateTime.Now));

            CreateMap<ProjectDto, CreateProjectInputModel>().ReverseMap();
            CreateMap<ProjectDto, ProjectViewModel>().ReverseMap();
            CreateMap<ProjectDto, EditProjectInputModel>().ReverseMap();
            CreateMap<PaginatedProjectDto, PaginatedProjectViewModel>().ReverseMap();

            CreateMap<WorkItem, UserStoryDto>().ReverseMap();
            CreateMap<WorkItem, UserStoryAllDto>().ReverseMap();
            CreateMap<WorkItem, UserStoryUpdateModel>().ReverseMap();             
            CreateMap<WorkItem, UserStoryInputModel>().ReverseMap()
                .ForMember(x => x.Description, opt => opt.MapFrom(x => x.SanitizedDescription))
                .ForMember(x => x.AcceptanceCriteria, opt => opt.MapFrom(x => x.AcceptanceCriteria));                           
            CreateMap<UserStoryViewModel, UserStoryUpdateModel>().ReverseMap();                
            CreateMap<UserStoryViewModel, UserStoryDto>().ReverseMap();
            CreateMap<UserStoriesAllViewModel, UserStoryDto>().ReverseMap();
            CreateMap<UserStoriesAllViewModel, UserStoryAllDto>().ReverseMap();
            CreateMap<UpdateUserStoriesViewModel, UserStoryDto>().ReverseMap();
            CreateMap<UpdateUserStoriesViewModel, UserStoryUpdateModel>().ReverseMap()
                .ForPath(x => x.ViewModel.SanitizedAcceptanceCriteria, opt => opt.MapFrom(x => x.AcceptanceCriteria))
                .ForPath(x => x.ViewModel.SanitizedDescription, opt => opt.MapFrom(x => x.Description))
                .ForMember(x => x.Comment, opt => opt.MapFrom(x => x.Comment));

            CreateMap<BacklogPriority, BacklogPrioritiesDto>().ReverseMap();

            CreateMap<BacklogPrioritiesDto, BacklogPriorityDropDownModel>().ReverseMap();

            CreateMap<Comment, CommentDto>().ReverseMap();
            CreateMap<Comment, CommentViewModel>().ReverseMap()
                .ForMember(x => x.Description, opt => opt.MapFrom(x => x.SanitizedDescription));
            CreateMap<Comment, CommentInputModel>().ReverseMap()
                .ForMember(x => x.Description, opt => opt.MapFrom(x => x.SanitizedDescription));
            CreateMap<CommentViewModel, CommentDto>().ReverseMap();
            CreateMap<CommentViewModel, CommentInputModel>().ReverseMap();
            CreateMap<CommentsUpdateModel, CommentInputModel>().ReverseMap();
        }
    }
}
