namespace TradesWomanBE.Models
{
    public class CTWIStipendsModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string? TypeOfStipend { get; set; }
        public string? PreApprenticeshipProgram { get; set; }
        public string? StipendDetails { get; set; }
        public int? StipendAmountRequested { get; set; }
        public string? StipendPaymentMethod { get; set; }
        public string? IssuedDate { get; set; }
        public string? RequestedDate { get; set; }
    }
}
