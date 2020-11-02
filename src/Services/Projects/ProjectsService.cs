﻿using AutoMapper;
using Data.Models;
using DataModels.Dtos;
using DataModels.Models.Project;
using Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Projects
{
    public class ProjectsService : IProjectsService
    {
        private readonly IRepository<Project> repo;
        private readonly IMapper mapper;

        public ProjectsService(IRepository<Project> repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task<int> CreateAsync(CreateProjectInputModel inputModel)
        {
            await this.repo.AddAsync(new Project
            {
                AddedOn = DateTime.UtcNow,
                Name = inputModel.Name,
                Description = inputModel.Description
            });
            await repo.SaveChangesAsync();

            return this.repo.AllAsNoTracking().Where(x => x.Name == inputModel.Name).FirstOrDefault().Id;
        }

        public bool IsNameTaken(string name)
        {
            return this.repo.AllAsNoTracking().Any(x => x.Name == name);
        }

        public IEnumerable<ProjectDto> GetAll()
        {
            return mapper.Map<IEnumerable<ProjectDto>>(this.repo.All().ToList());
        }

        public async Task<ProjectDto> GetAsync(int id)
        {
            return mapper.Map<ProjectDto>(this.repo .AllAsNoTracking().Where(x => x.Id == id).FirstOrDefault());
        }

        
    }
}