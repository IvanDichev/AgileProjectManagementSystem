using AutoMapper;
using Data.Models;
using DataModels.Models.BacklogPriorities;
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

            CreateMap<UserStory, UserStoryDto>().ReverseMap();
            CreateMap<UserStory, CreateUserStoryInputModel>().ReverseMap();
            CreateMap<UserStoryViewModel, UserStoryDto>().ReverseMap();
            CreateMap<UserStoriesAllViewModel, UserStoryDto>().ReverseMap();
            CreateMap<DetailsUserStoriesViewModel, UserStoryDto>().ReverseMap();

            CreateMap<BacklogPriority, BacklogPrioritiesDto>().ReverseMap();

            CreateMap<BacklogPrioritiesDto, BacklogPriorityDropDownModel>().ReverseMap();
        }
    }
}
