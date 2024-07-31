using System.ComponentModel.DataAnnotations.Schema;

namespace TradesWomanBE.Models
{
     public class NoteModel
    {
        public int Id { get; set; }
        
        public int MeetingNoteId { get; set; }
        
        public string? Content { get; set; }

        [ForeignKey("MeetingNoteId")]
        public virtual MeetingNotesModel? MeetingNote { get; set; }
    }
}