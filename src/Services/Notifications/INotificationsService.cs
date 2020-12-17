using DataModels.Models.Notifications.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Notifications
{
    public interface INotificationsService
    {
        Task<ICollection<NotificationDto>> GetNotificationsForUser(int userId);
    }
}
