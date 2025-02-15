using System.ComponentModel.DataAnnotations;

namespace EventPlanningCapstoneProject.Models
{
    public class Analytics
    {
        [Key]
        public int Id { get; set; }
        public int EventId { get; set; }
        public string SalesData { get; set; }  // JSON-like structure for sales
        public string Demographics { get; set; } // JSON-like structure for demographics

        // Foreign Key
        public virtual Event Event { get; set; }
    }
    public class AnalyticsDto
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string SalesData { get; set; }
        public string Demographics { get; set; }
    }


}
