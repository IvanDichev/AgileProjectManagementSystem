using System;

namespace DataModels.Models.Notifications.Dtos
{
    public class NotificationDto
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public DateTime AddedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
