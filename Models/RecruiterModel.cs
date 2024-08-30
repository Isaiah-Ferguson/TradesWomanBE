namespace TradesWomanBE.Models
{
    public class RecruiterModel
    {
        public int Id { get; set; }
        public int? EmployeeID { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public Char? MiddleInnitial { get; set; }
        public string? Email { get; set; }
        public string? Department { get; set; }
        public string? Location { get; set; }
        public string? SuperviserName { get; set; }
        public int? PhoneNumber { get; set; }
        public string? CreatedBy { get; set; }
        public string? Status { get; set;}
        public string? Salt {get; set;}
        public string? Hash { get; set; }
        public bool IsDeleted { get; set; }
    }
}