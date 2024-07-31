using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradesWomanBE.Models
{
    public class CTWIStipendsModel
    {
        public int Id { get; set;}
        public int UserID { get; set;}
        public string? TypeOfStipend { get; set;}
        public string? PreApprenticeshipProgram { get; set;}
        public string? StipendDetails { get; set;}
        public int? StipendAmountRequested { get; set;}
        public string? StipendPaymentMethod { get; set;}
    }
}