namespace TradesWomanBE.Models.DTO
{
    public class LoginResultDTO
    {
        public string Token { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public bool IsAdmin { get; set; }
        public int Id { get; set; }
        public bool FirstTimeLogin { get; set; }
        public string? Role { get; set; }
    }
}
