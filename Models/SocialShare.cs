using System.ComponentModel.DataAnnotations;

namespace EventPlanningCapstoneProject.Models
{
    public class SocialShare
    {
        [Key]
        public int Id { get; set; }
        public int EventId { get; set; }
        public string Platform { get; set; } // 'Facebook', 'Twitter', 'Instagram'
        public int UserId { get; set; }
        public DateTime ShareDate { get; set; }

        // Foreign Keys
        public virtual Event Event { get; set; }
        public virtual User User { get; set; }
    }
    public class SocialShareDto
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string Platform { get; set; } // 'Facebook', 'Twitter', 'Instagram'
        public int UserId { get; set; }
        public DateTime ShareDate { get; set; }
    }

}
