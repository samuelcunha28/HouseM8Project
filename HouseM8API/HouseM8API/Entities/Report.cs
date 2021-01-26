using HouseM8API.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseM8API.Entities
{
    public class Report
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public int ReporterId { get; set; }
        public int ReportedId { get; set; }
        public Reasons Reason { get; set; } 
    }
}
