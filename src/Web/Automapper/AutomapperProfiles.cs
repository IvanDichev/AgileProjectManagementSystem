using AutoMapper;
using Data.Models;
using DataModels.Dtos.Teams;
using DataModels.Models.Teams;

namespace Web.Automapper
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<CreateTeamInputModel, TeamDto>().ReverseMap();
            CreateMap<Team, TeamDto>().ReverseMap();
        }
    }
}
