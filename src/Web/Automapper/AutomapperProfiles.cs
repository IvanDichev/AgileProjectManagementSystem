﻿using AutoMapper;
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
            CreateMap<ProjectDto, ProjectViewModel>().ReverseMap();
            CreateMap<ProjectDto, EditProjectInputModel>().ReverseMap();
            CreateMap<PaginatedProjectDto, PaginatedProjectViewModel>().ReverseMap();

            CreateMap<UserStory, WorkItemDto>().ReverseMap(); 
            CreateMap<UserStory, WokrItemAllDto>().ReverseMap(); 
            CreateMap<UserStory, WorkItemInputModel>().ReverseMap(); 
            CreateMap<WorkItemViewModel, WorkItemUpdateModel>().ReverseMap(); 
            CreateMap<WorkItemViewModel, WorkItemDto>().ReverseMap(); 
            CreateMap<WorkItemAllViewModel, WokrItemAllDto>().ReverseMap(); 

            CreateMap<BacklogPriority, BacklogPrioritiesDto>().ReverseMap();

            CreateMap<BacklogPrioritiesDto, BacklogPriorityDropDownModel>().ReverseMap();

            CreateMap<WorkItemTypesDto, WorkItemType>().ReverseMap();
            CreateMap<WorkItemTypesDto, WorkItemTypesDropDownModel>().ReverseMap();

            CreateMap<Comment, CommentDto>().ReverseMap();
            CreateMap<Comment, CommentViewModel>().ReverseMap()
                .ForMember(x => x.Description, opt => opt.MapFrom(x => x.SanitizedDescription));
            CreateMap<Comment, CommentInputModel>().ReverseMap(); //
            CreateMap<CommentViewModel, CommentDto>().ReverseMap();
            CreateMap<CommentViewModel, CommentInputModel>().ReverseMap();
            CreateMap<CommentsUpdateModel, CommentInputModel>().ReverseMap();
        }
    }
}
