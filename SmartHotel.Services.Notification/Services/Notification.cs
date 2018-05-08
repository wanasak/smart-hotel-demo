using System;

namespace SmartHotel.Services.Notification.Services
{
    public enum NotificationType
    {
        BeGreen,
        Room,
        Hotel,
        Other
    }

    public class Notification
    {
        public int Seq { get; set; }
        public DateTime Time { get; set; }
        public string Text { get; set; }
        public NotificationType Type { get; set; }
    }
}