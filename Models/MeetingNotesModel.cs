using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradesWomanBE.Models
{
    public class MeetingNotesModel
    {
        public int Id { get; set;}
        public int MeetingID { get; set;}
        public string? RecruiterInfo { get; set;}
        [ForeignKey("MeetingId")]
        public virtual MeetingsModel? Meeting { get; set; }
        public virtual ICollection<NoteModel> Notes { get; set; } = new List<NoteModel>();
    }
}