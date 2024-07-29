using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradesWomanBE.Models
{
    public class MeetingNotesModel
    {
        public int Id { get; set;}
        public int MeetingID { get; set;}
        public string RecruiterInfo { get; set;}

        public List<string> Notes { get; set;} = new List<string>();
    }
}