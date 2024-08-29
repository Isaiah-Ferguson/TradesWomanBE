using System.ComponentModel.DataAnnotations.Schema;

namespace TradesWomanBE.Models
{
    public class MeetingsModel
    {
        public int Id { get; set; }
        public int ClientID { get; set; }
        public string? RecruiterName { get; set; }
        public int NumOfContacts { get; set; }
        public string? LastDateContacted { get; set; }
        public string? LastContactMethod { get; set; }
        public string? PreferedContact { get; set; }
        public string? GrantName { get; set; }

        [ForeignKey("MeetingId")]
        public virtual ICollection<MeetingNotesModel> MeetingNotes { get; set; } = new List<MeetingNotesModel>();
    }
}
