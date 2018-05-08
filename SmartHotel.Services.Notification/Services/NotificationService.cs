using System;
using System.Collections.Generic;

namespace SmartHotel.Services.Notification.Services
{
    public class NotificationService
    {
        private const int START_HOUR = 8;
        private const int INC_MINUTES = 15;
        private const int MAX_NOTIFS = 50;

        public IEnumerable<Notification> GetNotificationsForUser(string userid)
        {
            var notifs = new List<Notification>() {
                new Notification()
                {
                    Type = NotificationType.BeGreen,
                    Text = "Temperature on your room has been adjusted to save energy while mantaining your comfort."
                },
                new Notification()
                {
                    Type = NotificationType.Hotel,
                    Text = "New show is about to begin at Saloon Bar."
                },
                new Notification()
                {
                    Type = NotificationType.Room,
                    Text = "Your room has been cleaned."
                },
                new Notification()
                {
                    Type = NotificationType.Hotel,
                    Text = "SPA is available today until 11pm"
                },
                new Notification()
                {
                    Type = NotificationType.BeGreen,
                    Text = "Ambient lights had been adjusted according to new weather conditions"
                },
                new Notification()
                {
                    Type = NotificationType.Hotel,
                    Text = "Enjoy our craft beers selection at Saloon Bar."
                },
                new Notification()
                {
                    Type = NotificationType.BeGreen,
                    Text = "One towel has been replaced in your room. Thanks for helping us to keep the nature in good health!"
                },
                new Notification()
                {
                    Type = NotificationType.Room,
                    Text = "A present, courtesy of the hotel, is waiting in your room. Enjoy it!"
                }
            };

            var today = DateTime.Now.Date;
            for (int i = 0; i < MAX_NOTIFS; i++)
            {
                var notif = notifs[i % notifs.Count];
                notif.Seq = i;
                notif.Time = today.AddMinutes(i * INC_MINUTES);
                yield return notif;
            }
        }
    }
}