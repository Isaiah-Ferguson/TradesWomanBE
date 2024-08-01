using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradesWomanBE.Models
{
    public class MeetingNotesModel
    {
        public int Id { get; set;}
        public string? RecruiterInfo { get; set;}
        [ForeignKey("MeetingId")]
        public virtual MeetingsModel? Meeting { get; set; }
        public string? Notes { get; set; }

    }
}