using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    public class ReportServiceDisplayViewModel
    {
        [ProtoMember(1)]
        public string Id { get; set; }
        [ProtoMember(2)]
        [Display(Name = "From")]
        public DateTime DateFrom { get; set; }
        [ProtoMember(3)]
        [Display(Name = "To")]
        public DateTime DateTo { get; set; }
        [ProtoMember(4)]
        public string Supplier { get; set; }
        [ProtoMember(5)]
        [Display(Name = "Payment Period")]
        public int PaymentPeriod { get; set; }
        [ProtoMember(6)]
        public decimal Fee { get; set; }
        [ProtoMember(7)]
        public string Description { get; set; }
        [ProtoMember(8)]
        [Display(Name = "Total Supplier")]
        public int TotalSupplier { get; set; }
        [ProtoMember(9)]
        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }
        [ProtoMember(10)]
        public int Count { get; set; }
    }
}
