using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Web.Models.Team;

namespace Web.Automapper
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            //CreateMap<CreateTeamInputModel, TeamDto>().ReverseMap();
        }
    }
}
