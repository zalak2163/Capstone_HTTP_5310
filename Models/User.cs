using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EventPlanningCapstoneProject.Models
{
    public class User : IdentityUser<int>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation Property (Foreign Key Relations)
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Purchase> Purchases { get; set; }
        public virtual ICollection<EventCalendar> Calendars { get; set; }
        public virtual ICollection<SocialShare> SocialShares { get; set; }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
