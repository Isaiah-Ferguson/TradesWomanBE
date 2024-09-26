namespace TradesWomanBE.Models
{
    public class AdminUser
    {
        public int Id { get; set; }
        public string? FirstName {get; set;}
        public string? LastName {get; set;}
        public string? Email { get; set; }
        public string? Salt {get; set;}
        public string? Hash { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}