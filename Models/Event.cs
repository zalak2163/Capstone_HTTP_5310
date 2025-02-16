using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Net.Sockets;

namespace EventPlanningCapstoneProject.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime EventDate { get; set; }
        public string Image { get; set; }
        public DateTime CreatedAt { get; set; }

        // Foreign Key
        public int? OrganizerId { get; set; }  // Nullable because it can be null if no organizer
        public virtual User Organizer { get; set; }

        // Navigation Property
        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<Purchase> Purchases { get; set; }
        public virtual ICollection<SocialShare> SocialShares { get; set; }
        public virtual ICollection<Analytics> Analytics { get; set; }
        public virtual ICollection<EventCalendar> Calendars { get; set; }
    }
    public class EventDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public string Location { get; set; }
        public string Image { get; set; }
        public string OrganizerName { get; set; }
        public DateTime CreatedAt { get; set; }

        // Allow OrganizerId to be nullable (since it's a foreign key in the Event model)
        public int? OrganizerId { get; set; }
    }



}
