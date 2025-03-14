using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventPlanningCapstoneProject.Models
{
    public class User : IdentityUser<int>
    {
        [MaxLength(50)]
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
     
        public DateTime ModifiedDate { get; set; }
    
        public DateTime LastLogin { get; set; } 
        public bool IsAdmin { get; set; } = false;
        public string PhoneNumber { get; set; }

        // Navigation Property (Foreign Key Relations)
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Purchase> Purchases { get; set; }
        public virtual ICollection<EventCalendar> Calendars { get; set; }
        public virtual ICollection<SocialShare> SocialShares { get; set; }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedDate { get; set; } 
        public DateTime ModifiedDate { get; set; } 
        public DateTime LastLogin { get; set; } 
        public bool IsAdmin { get; set; } = false;
        public string? Email { get; internal set; }
    }
}
