using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public DateTime DateTime { get; private set; }
        public NotificationType Type { get; private set; }
        public DateTime? OriginalDateTime { get; private set; }
        public string OriginalVenue { get; private set; }
        [Required]
        public Gig Gig { get; private set; }

        protected Notification()
        {

        }

        private Notification(Gig gig, NotificationType type)
        {
            if (gig == null)
                throw new ArgumentNullException("Notification");

            DateTime = DateTime.Now;
            Gig = gig;
            Type = type;
        }

        public static Notification GigCreated(Gig gig)
        {
            return new Notification(gig, NotificationType.GigCreated);
        }

        public static Notification GigUpdated(Gig newGig, DateTime originalDateTime, string originalVenue)
        {
            var notify = new Notification(newGig, NotificationType.GigUpdated);
            notify.OriginalDateTime = originalDateTime;
            notify.OriginalVenue = originalVenue;
            return notify;
        }

        public static Notification GigCancelled(Gig gig)
        {
            return new Notification(gig, NotificationType.GigCancelled);
        }

        public static Notification GigUncancelled(Gig gig)
        {
            return new Notification(gig, NotificationType.GigUncancelled);
        }
    }
}
