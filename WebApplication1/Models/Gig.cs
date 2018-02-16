using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebApplication1.Models
{
    public class Gig
    {
        public int Id { get; set; }
        public bool IsCancelled { get; private set; }
        public ApplicationUser Artist { get; set; }
        [Required]
        public string ArtistId { get; private set; }
        public DateTime DateTime { get; private set; }
        [Required]
        [StringLength(255)]
        public string Venue { get; private set; }
        public Genre Genre { get; set; }
        [Required]
        public int GenreId { get; private set; }
        public ICollection<Attendance> Attendances { get; private set; }

        public Gig()
        {
            Attendances = new Collection<Attendance>();
        }

        public static Gig NewGig(DateTime dt, string artistId, int genreId, string venue)
        {
            var gig = new Gig();

            gig.ArtistId = artistId;
            gig.DateTime = dt;
            gig.GenreId = genreId;
            gig.Venue = venue;

            return gig;
        }

        public void Cancel()
        {
            IsCancelled = true;

            var notification = Notification.GigCancelled(this);

            foreach (var attendee in Attendances.Select(a => a.Attendee))
            {
                attendee.Notify(notification);
            }
        }

        public void Uncancel()
        {
            IsCancelled = false;

            var notification = Notification.GigUncancelled(this);

            foreach (var attendee in Attendances.Select(a => a.Attendee))
            {
                attendee.Notify(notification);
            }
        }

        public void Update(DateTime dateTime, string venue, int genre)
        {
            var notification = Notification.GigUpdated(this, DateTime, Venue);

            DateTime = dateTime;
            Venue = venue;
            GenreId = genre;

            foreach (var attendee in Attendances.Select(a => a.Attendee))
            {
                attendee.Notify(notification);
            }
        }
    }
}
