using AutoMapper;
using Data.Models;
using DataModels.Models.Project;
using DataModels.Models.Project.Dtos;

namespace Web.Automapper
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<Project, ProjectDto>().ReverseMap();
            CreateMap<ProjectDto, CreateProjectInputModel>().ReverseMap();
            CreateMap<ProjectDto, ProjectViewModel>().ReverseMap();
        }
    }
}
