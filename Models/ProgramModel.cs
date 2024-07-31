using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradesWomanBE.Models
{
    public class ProgramModel
    {
        public int Id { get; set; }
        public int ClientID { get; set; }
        public string? ProgramEnrolled { get; set; }
        public string? EnrollDate { get; set; }
        public string? ProgramEndDate { get; set; }
        public string? CurrentStatus { get; set; }
    }
}