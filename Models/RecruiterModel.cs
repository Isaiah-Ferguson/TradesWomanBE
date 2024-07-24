using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradesWomanBE.Models
{
    public class RecruiterModel
    {
        public int Id { get; set; }
        public int? UserID { get; set; }
        public int? EmployeeID { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public Char? MiddleInnitial { get; set; }
        public string? Email { get; set; }
        public string? Department { get; set; }
        public string? SuperviserName { get; set; }
        public int? PhoneNumber { get; set; }
        public string? CreatedBy { get; set; }
        public string? Status { get; set;}
        public bool IsDeleted { get; set; }

        public RecruiterModel(){}
    }
}