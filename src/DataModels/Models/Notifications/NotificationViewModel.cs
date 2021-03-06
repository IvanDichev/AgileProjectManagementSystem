﻿using System;

namespace DataModels.Models.Notifications
{
    public class NotificationViewModel
    {
        public string Message { get; set; }

        public DateTime AddedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
