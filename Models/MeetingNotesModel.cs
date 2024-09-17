namespace TradesWomanBE.Models
{
    public class MeetingNotesModel
    {
        public int Id { get; set; }
        public string? RecruiterInfo { get; set; }
        public int MeetingId { get; set; }
        public string? Notes { get; set; }
    }
}
