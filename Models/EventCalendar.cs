using System.ComponentModel.DataAnnotations;

namespace EventPlanningCapstoneProject.Models
{
    public class EventCalendar
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }  
        public int EventId { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime CreatedAt { get; set; }

        // Foreign Key Relations
        public virtual User User { get; set; }
        public virtual Event Event { get; set; }
    }

    public class CalendarDto
    {
        public int Id { get; set; }
        public int UserId { get; set; } 
        public int EventId { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
