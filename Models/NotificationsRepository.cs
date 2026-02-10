using DAL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotosManager.Models
{
    public class NotificationsRepository : Repository<Notification>
    {
        public void Push(int targetUserId, string Message)
        {
            User connectedUser = (User)HttpContext.Current.Session["ConnectedUser"];
            User targetUser = DB.Users.Get(targetUserId); 
            if (connectedUser != null && targetUser.Notify)
                Add(new Notification { TargetUserId = targetUserId, SourceUserId = connectedUser.Id, Message = Message });
        }
        public Notification Pop()
        {
            User connectedUser = (User)HttpContext.Current.Session["ConnectedUser"];
            if (connectedUser != null)
            {
                Notification notification = ToList().Where(n => n.TargetUserId == connectedUser.Id).FirstOrDefault()?.Copy();
                if (notification != null)
                {
                    Delete(notification.Id);
                    return notification;
                }
            }
            return null;
        }
    }
}