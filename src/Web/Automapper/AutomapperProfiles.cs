﻿using AutoMapper;
using Data.Models;
using Data.Models.Users;
using DataModels.Models.BacklogPriorities;
using DataModels.Models.Board.Dtos;
using DataModels.Models.Comments;
using DataModels.Models.Comments.Dtos;
using DataModels.Models.Mockups.Dtos;
using DataModels.Models.Notifications;
using DataModels.Models.Notifications.Dtos;
using DataModels.Models.Projects;
using DataModels.Models.Projects.Dtos;
using DataModels.Models.Severity;
using DataModels.Models.Sprints;
using DataModels.Models.Sprints.Dto;
using DataModels.Models.TeamRoles;
using DataModels.Models.TeamRoles.Dtos;
using DataModels.Models.Users;
using DataModels.Models.Users.Dtos;
using DataModels.Models.WorkItems;
using DataModels.Models.WorkItems.Bugs;
using DataModels.Models.WorkItems.Bugs.Dtos;
using DataModels.Models.WorkItems.Tasks;
using DataModels.Models.WorkItems.Tasks.Dtos;
using DataModels.Models.WorkItems.Tests;
using DataModels.Models.WorkItems.Tests.Dtos;
using DataModels.Models.WorkItems.UserStory;
using DataModels.Models.WorkItems.UserStory.Dtos;

namespace Web.Automapper
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            // Projects
            CreateMap<Project, ProjectDto>().ReverseMap();
            CreateMap<ProjectDto, ProjectViewModel>().ReverseMap();
            CreateMap<ProjectDto, EditProjectInputModel>().ReverseMap();
            CreateMap<PaginatedProjectDto, PaginatedProjectViewModel>().ReverseMap();
            
            // User stories
            CreateMap<UserStory, UserStoryDto>().ReverseMap(); 
            CreateMap<UserStory, UserStoryAllDto>().ReverseMap(); 
            CreateMap<UserStory, UserStoryInputModel>().ReverseMap();
            CreateMap<UserStory, UserStoryDropDownModel>().ReverseMap();
            CreateMap<UserStoryInputDto, UserStoryInputModel>().ReverseMap();
            CreateMap<UserStoryViewModel, UserStoryUpdateModel>().ReverseMap(); 
            CreateMap<UserStoryViewModel, UserStoryDto>().ReverseMap(); 
            CreateMap<UserStoryAllViewmodel, UserStoryAllDto>().ReverseMap();

            // User story tasks
            CreateMap<UserStoryTask, TaskAllDto>().ReverseMap();
            CreateMap<TaskInputModel, TaskInputModelDto>().ReverseMap();

            // Tests
            CreateMap<Test, TestAllDto>().ReverseMap();
            CreateMap<TestInputModel, TestInputModelDto>().ReverseMap();

            // Bugs
            CreateMap<Bug, BugAllDto>().ReverseMap();
            CreateMap<BugInputModel, BugInputModelDto>().ReverseMap();

            // Sprints
            CreateMap<Sprint, SprintDto>().ReverseMap();
            CreateMap<Sprint, SprintDropDownModel>().ReverseMap();
            CreateMap<Sprint, OldSprintsBurndownData>().ReverseMap();
            CreateMap<SprintAllViewModel, SprintDto>().ReverseMap();
            CreateMap<SprintInputModel, SprintInputDto>().ReverseMap();

            // Backlog priorities
            CreateMap<BacklogPriority, BacklogPrioritiesDto>().ReverseMap();
            CreateMap<BacklogPrioritiesDto, BacklogPriorityDropDownModel>().ReverseMap();

            // Board columns
            CreateMap<KanbanBoardColumn, BoardColumnAllDto>().ReverseMap();

            // Board options
            CreateMap<KanbanBoardColumnOption, ColumnOptionsDto>().ReverseMap();

            // User story comments
            CreateMap<UserStoryComment, CommentDto>().ReverseMap();
            CreateMap<UserStoryComment, CommentViewModel>().ReverseMap()
                .ForMember(x => x.Description, opt => opt.MapFrom(x => x.SanitizedDescription));
            CreateMap<UserStoryComment, CommentInputModel>().ReverseMap(); 
            CreateMap<CommentViewModel, CommentDto>().ReverseMap();
            CreateMap<CommentViewModel, CommentInputModel>().ReverseMap();
            CreateMap<CommentsUpdateModel, CommentInputModel>().ReverseMap();

            // Severity
            CreateMap<Severity, SeverityDropDownModel>().ReverseMap();

            // Mockup
            CreateMap<Mockup, MockupDto>().ReverseMap();

            // User
            CreateMap<User, UsersDropdown>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UsersDropdown, UserDto>().ReverseMap();

            // Team roles
            CreateMap<TeamRole, TeamRolesDto>().ReverseMap();
            CreateMap<TeamRolesDropdown, TeamRolesDto>().ReverseMap();

            // Notifications
            CreateMap<Notification, NotificationDto>().ReverseMap();
            CreateMap<NotificationViewModel, NotificationDto>().ReverseMap();
        }
    }
}
