using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradesWomanBE.Models.DTO
{
    public class SSNDTO
    {
        public string? Salt { get; set; }
        public string? Hash { get; set; }
    }
}