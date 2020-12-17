using AutoMapper;
using DataModels.Models.Notifications;
using Microsoft.AspNetCore.Mvc;
using Services.Notifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Web.ViewComponents
{
    [ViewComponent(Name = "Notifications")]
    public class NotificationsViewComponent : ViewComponent
    {
        private readonly IMapper mapper;
        private readonly INotificationsService notificationsService;

        public NotificationsViewComponent(IMapper mapper, INotificationsService notificationsService)
        {
            this.mapper = mapper;
            this.notificationsService = notificationsService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int userId)
        {
            var notifications = await this.GetLastFiveNotifications(userId);

            return View(notifications);
        }

        private async Task<ICollection<NotificationViewModel>> GetLastFiveNotifications(int userId)
        {
            var notifications = this.mapper.Map<ICollection<NotificationViewModel>>
                (await this.notificationsService.GetLastFiveNotifications(userId));

            return notifications;
        }
    }
}
