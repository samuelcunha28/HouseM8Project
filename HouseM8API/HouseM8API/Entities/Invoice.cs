using Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HouseM8API.Entities
{
    public class Invoice
    {
        public int Id { get; set; }
        public double Value { get; set; }
        public Payment PaymentType { get; set; }
        public DateTime Date { get; set; }
        public bool ConfirmedPayment { get; set; }
        public string Link { get; set; }
    }
}
