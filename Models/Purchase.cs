using System.ComponentModel.DataAnnotations;

namespace EventPlanningCapstoneProject.Models
{
    public class Purchase
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
        public int TicketId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime PurchaseDate { get; set; }

        // Foreign Key Relationships
        public virtual User User { get; set; }
        public virtual Event Event { get; set; }
        public virtual Ticket Ticket { get; set; }

        // Navigation Property
        public virtual ICollection<Payment> Payments { get; set; }
    }
    public class PurchaseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
        public int TicketId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public string PaymentStatus { get; set; } // 'Pending', 'Success', 'Failed'
        public DateTime PurchaseDate { get; set; }
    }

}