using System.ComponentModel.DataAnnotations;

namespace EventPlanningCapstoneProject.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }
        public string TicketType { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int Available { get; set; }
        public DateTime CreatedAt { get; set; }

        // Foreign Key
        public int EventId { get; set; }
        public virtual Event Event { get; set; }

        // Navigation Property
        public virtual ICollection<Purchase> Purchases { get; set; }
    }
    public class TicketDto
    {
        public int Id { get; set; }
        public string TicketType { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int Available { get; set; }
        public DateTime CreatedAt { get; set; }
        public int EventId { get; set; }
    }

}
