namespace EventPlanningCapstoneProject.Models
{
    public class RegisterUserResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime LastLogin { get; set; }
        public bool IsAdmin { get; set; }
    }

}
