using System.ComponentModel.DataAnnotations.Schema;

namespace TradesWomanBE.Models
{
    public class ClientModel
    {
        public int Id { get; set; }
        public int Age { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public Char? MiddleInnitial { get; set; }
        public string? Email { get; set; }
        public int? ChildrenUnderSix { get; set; }
        public int? ChildrenOverSix { get; set; }
        public int? SSNLastFour { get; set; }
        public string? Salt { get; set; }
        public string? Hash { get; set; }
        public string? ValidSSNAuthToWrk { get; set; }
        public string? CriminalHistory { get; set;}
        public string? Disabled { get; set;}
        public string? FoundUsOn { get; set;}
        public string? DateJoinedEAW { get; set;}
        public string? CTWIStipends { get; set;}
        public string? Address  { get; set;}
        
        [ForeignKey("ProgramInfoId")]
        public virtual ProgramModel? ProgramInfo { get; set; }
        public virtual ICollection<MeetingsModel> Meetings { get; set; } = new List<MeetingsModel>();
        public bool IsDeleted { get; set; } 
    }
}