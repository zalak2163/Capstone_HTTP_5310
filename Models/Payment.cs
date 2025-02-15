using System.ComponentModel.DataAnnotations;

namespace EventPlanningCapstoneProject.Models

{
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        public int PurchaseId { get; set; }
        public string PaymentMethod { get; set; } // 'Stripe', 'PayPal', 'CreditCard'
        public decimal Amount { get; set; }
        public string PaymentStatus { get; set; } // 'Success', 'Failed'
        public string TransactionId { get; set; }
        public DateTime PaymentDate { get; set; }

        // Foreign Key
        public virtual Purchase Purchase { get; set; }
    }
    public class PaymentDto
    {
        public int Id { get; set; }
        public int PurchaseId { get; set; }
        public string PaymentMethod { get; set; } // 'Stripe', 'PayPal', 'CreditCard'
        public decimal Amount { get; set; }
        public string PaymentStatus { get; set; } // 'Success', 'Failed'
        public string TransactionId { get; set; }
        public DateTime PaymentDate { get; set; }
    }

}
