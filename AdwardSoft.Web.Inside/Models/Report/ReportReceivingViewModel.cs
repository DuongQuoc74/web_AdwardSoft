using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    public class ReportReceivingViewModel
    {
        [ProtoMember(1)]
        public string Id { get; set; }
        [ProtoMember(2)]
        public DateTime Date { get; set; }
        [ProtoMember(3)]
        public string No { get; set; }
        [ProtoMember(4)]
        public string Supplier { get; set; }
        [ProtoMember(5)]
        public string Address { get; set; }
        [ProtoMember(6)]
        public string Phone { get; set; }
        [ProtoMember(7)]
        [Display(Name = "Total Quantity")]
        public decimal TotalQuantity { get; set; }
        [ProtoMember(8)]
        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }
        [ProtoMember(9)]
        public short Status { get; set; }
        [ProtoMember(10)]
        public int Count { get; set; }
    }
}
