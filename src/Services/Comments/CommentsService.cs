﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Models;
using DataModels.Models.Comments.Dtos;
using Microsoft.EntityFrameworkCore;
using Repo;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Comments
{
    public class CommentsService : ICommentsService
    {
        private readonly IRepository<Comment> repo;
        private readonly IMapper mapper;

        public CommentsService(IRepository<Comment> repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task<CommentDto> Get(int commentId)
        {
            return await this.repo.All().Where(x => x.Id == commentId)
                .ProjectTo<CommentDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public bool IsUsersComment(int userId, int commentId)
        {
            return this.repo.AllAsNoTracking()
                .Where(x => x.Id == commentId)
                .Any(x => x.UserId == userId);
        }

        public async Task Update(CommentsUpdateModel updateModel)
        {
            var toUpdate = await this.repo.All()
                .Where(x => x.Id == updateModel.Id)
                .FirstOrDefaultAsync();

            toUpdate.ModifiedOn = DateTime.UtcNow;
            toUpdate.Description = updateModel.Description;

            this.repo.Update(toUpdate);
            await this.repo.SaveChangesAsync();
        }
    }
}
