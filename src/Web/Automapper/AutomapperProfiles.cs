using AutoMapper;
using Data.Models;
using DataModels.Models.BacklogPriorities;
using DataModels.Models.Comments;
using DataModels.Models.Comments.Dtos;
using DataModels.Models.Projects;
using DataModels.Models.Projects.Dtos;
using DataModels.Models.WorkItems;
using DataModels.Models.WorkItems.Dtos;
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

            CreateMap<WorkItem, WorkItemDto>().ReverseMap();
            CreateMap<WorkItem, WokrItemAllDto>().ReverseMap();
            CreateMap<WorkItem, WorkItemUpdateModel>().ReverseMap()
                .ForMember(x => x.AddedOn, opt => opt.Ignore());             
            CreateMap<WorkItem, WorkItemInputModel>().ReverseMap()
                .ForMember(x => x.Description, opt => opt.MapFrom(x => x.SanitizedDescription))
                .ForMember(x => x.AcceptanceCriteria, opt => opt.MapFrom(x => x.AcceptanceCriteria))
                .ForMember(x => x.WorkItemTypeId, opt => opt.MapFrom(x => x.WorkItemTypesId));
            CreateMap<WorkItemViewModel, WorkItemUpdateModel>().ReverseMap();
            CreateMap<WorkItemViewModel, WorkItemDto>().ReverseMap();
            CreateMap<WorkItemAllViewModel, WorkItemDto>().ReverseMap();
            CreateMap<WorkItemAllViewModel, WokrItemAllDto>().ReverseMap();
            CreateMap<UpdateWorkItemViewModel, WorkItemDto>().ReverseMap();
            CreateMap<UpdateWorkItemViewModel, WorkItemUpdateModel>().ReverseMap()
                .ForPath(x => x.ViewModel.SanitizedAcceptanceCriteria, opt => opt.MapFrom(x => x.AcceptanceCriteria))
                .ForPath(x => x.ViewModel.SanitizedDescription, opt => opt.MapFrom(x => x.Description))
                .ForPath(x => x.ViewModel.WorkItemTypeId, opt => opt.MapFrom(x => x.WorkItemTypeId))
                .ForMember(x => x.Comment, opt => opt.MapFrom(x => x.Comment));

            CreateMap<BacklogPriority, BacklogPrioritiesDto>().ReverseMap();

            CreateMap<BacklogPrioritiesDto, BacklogPriorityDropDownModel>().ReverseMap();

            CreateMap<WorkItemTypesDto, WorkItemType>().ReverseMap();
            CreateMap<WorkItemTypesDto, WorkItemTypesDropDownModel>().ReverseMap();

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
