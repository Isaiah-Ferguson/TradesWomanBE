using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradesWomanBE.Models
{
    public class AdminUser
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string? Salt {get; set;}
        public string? Hash { get; set; }
        public bool IsDeleted { get; set; }
    }
}