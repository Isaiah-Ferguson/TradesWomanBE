using System.ComponentModel.DataAnnotations.Schema;

namespace TradesWomanBE.Models
{
    public class ClientModel
    {
        public int Id { get; set; }
        public int Age { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public Char? MiddleInitial { get; set; }
        public string? Email { get; set; }
        public int? ChildrenUnderSix { get; set; }
        public int? ChildrenOverSix { get; set; }
        public int? TotalHouseholdFamily { get; set; }
        public int? SSNLastFour { get; set; }
        public string? ValidSSNAuthToWrk { get; set; }
        public string? CriminalHistory { get; set; }
        public string? Disabled { get; set; }
        public string? FoundUsOn { get; set; }
        public string? DateJoinedEAW { get; set; }
        public string? Address  { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public int? ZipCode { get; set; }
        public string? Gender { get; set; }
        public string? Employed { get; set; }
        public string? RecruiterName { get; set; }
        public string? DateRegistered { get; set; }
        public string? DateOfBirth { get; set; }
        public string? ActiveOrFormerMilitary { get; set; }
        public int? TotalMonthlyIncome { get; set; }
        public string? PhoneNumber { get; set; }
        public int? ProgramInfoId { get; set; }
        public string? HighestEducation { get; set; }
        public string? ValidCALicense { get; set; }
        public string? County { get; set; }
        public string? Ethnicity { get; set; }

        [ForeignKey("ProgramInfoId")]
        public virtual ProgramModel? ProgramInfo { get; set; }

        public virtual MeetingsModel? Meetings { get; set; }

        public virtual StipendsModel? Stipends { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
