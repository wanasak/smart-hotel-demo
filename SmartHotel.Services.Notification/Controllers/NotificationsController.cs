using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHotel.Services.Notification.Services;

namespace SmartHotel.Services.Notification.Controllers
{
    [Route("notifications")]
    public class NotificationsController : Controller
    {
        private readonly NotificationService _notiService;
        public NotificationsController(NotificationService notiService)
        {
            this._notiService = notiService;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetByUser(int seq = -1)
        {
            var now = DateTime.Now;
            var userId = User.Claims.FirstOrDefault(c => c.Type == "emails").Value;
            var data = this._notiService.GetNotificationsForUser(userId)
                .Where(n => n.Seq > seq)
                .Where(n => n.Time <= now)
                .Take(3);
            
            return Ok(data);   
        }
    }
}
