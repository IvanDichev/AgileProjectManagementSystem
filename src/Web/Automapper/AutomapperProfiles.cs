using AutoMapper;
using Data.Models;
using DataModels.Models.BacklogPriorities;
using DataModels.Models.Projects;
using DataModels.Models.Projects.Dtos;
using DataModels.Models.UserStories;
using DataModels.Models.UserStories.Dtos;

namespace Web.Automapper
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<Project, ProjectDto>().ReverseMap();
            CreateMap<ProjectDto, CreateProjectInputModel>().ReverseMap();
            CreateMap<ProjectDto, ProjectViewModel>().ReverseMap();
            CreateMap<ProjectDto, EditProjectViewModel>().ReverseMap();

            CreateMap<UserStory, UserStoryDto>().ReverseMap();
            CreateMap<UserStoryViewModel, UserStoryDto>().ReverseMap();

            CreateMap<BacklogPriority, BacklogPrioritiesDto>().ReverseMap();

            CreateMap<BacklogPrioritiesDto, BacklogPriorityDropDownModel>().ReverseMap();
        }
    }
}
