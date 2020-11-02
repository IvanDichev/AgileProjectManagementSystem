using AutoMapper;
using Data.Models;
using DataModels.Dtos;
using DataModels.Models.Project;

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
