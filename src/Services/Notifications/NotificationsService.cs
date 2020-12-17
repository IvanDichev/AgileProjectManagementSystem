using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Models;
using DataModels.Models.Notifications.Dtos;
using Microsoft.EntityFrameworkCore;
using Repo;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Notifications
{
    public class NotificationsService : INotificationsService
    {
        private readonly IMapper mapper;
        private readonly IRepository<Notification> notificationsRepo;

        public NotificationsService(IMapper mapper, IRepository<Notification> notificationsRepo)
        {
            this.mapper = mapper;
            this.notificationsRepo = notificationsRepo;
        }

        public async Task<ICollection<NotificationDto>> GetNotificationsForUserAsync(int userId)
        {
            var notifications = await this.notificationsRepo.AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .ProjectTo<NotificationDto>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            return notifications;
        }

        public async Task<ICollection<NotificationDto>> GetLastFiveNotifications(int userId)
        {
            var notifications = await this.notificationsRepo.AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.AddedOn)
                .Take(5)
                .ProjectTo<NotificationDto>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            return notifications;
        }
    }
}
