using Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HouseM8API.Models
{
    public class InvoiceModel
    {
        [Required]
        public float Value { get; set; }
        [Required]
        public Payment PaymentType { get; set; }
        public bool ConfirmedPayment { get; set; }
        public string Link { get; set; }
    }
}
