//using AutoMapper;
//using Data;
//using Data.Models;
//using DataModels.Dtos;
//using DataModels.Dtos.Teams;
//using DataModels.Models.Teams;
//using Repo;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Services.TeamsServices
//{
//    public class TeamsService : ITeamsService
//    {
//        private readonly ApplicationDbContext db;
//        private readonly IRepository<Team> repo;
//        private readonly IMapper mapper;

//        public TeamsService(ApplicationDbContext db, IRepository<Team> repo, IMapper mapper)
//        {
//            this.db = db;
//            this.repo = repo;
//            this.mapper = mapper;
//        }

//        public async Task<int> CreateAsync(Team team)
//        {
//            var newTeam = this.db.Teams.Add(team);
//            await db.SaveChangesAsync();
//            return newTeam.Entity.Id;
            
//        }

//        public Task<int> CreateAsync(CreateTeamInputModel team)
//        {
//            throw new System.NotImplementedException();
//        }

//        public TeamDto Get(int id)
//        {
//            throw new System.NotImplementedException();
//        }

//        public ICollection<TeamDto> GetAllAsync()
//        {
//            return mapper.Map<ICollection<TeamDto>>(this.repo.All().ToList());
//            //return this.db.Teams.ToList();
//        }

//        ICollection<TeamDto> ITeamsService.GetAllAsync()
//        {
//            throw new System.NotImplementedException();
//        }
//    }
//}
