namespace TradesWomanBE.Models
{
    public class RecruiterModel
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Char? MiddleInitial { get; set; }
        public string? Email { get; set; }
        public string? Department { get; set; }
        public string? Location { get; set; }
        public string? SupervisorName { get; set; }
        public int? PhoneNumber { get; set; }
        public string? Status { get; set;}
        public bool FirstTimeLogin { get; set; }
        public string? Salt { get; set; }
        public string? JobTitle { get; set; }
        public string? HireDate { get; set; }
        public string? Hash { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsAdmin { get; set; }
    }
}