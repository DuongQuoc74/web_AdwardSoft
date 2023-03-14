using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    [ProtoInclude(100, typeof(ServiceDisplayDataViewModel))]
    public class ServiceDisplayViewModel
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        [Required]
        [DisplayName("Name")]
        public string Name { get; set; }
        [ProtoMember(3)]
        [Required]
        [DisplayName("Supplier")]
        public int SupplierId { get; set; }
        [ProtoMember(4)]
        public DateTime Date { get; set; } = DateTime.Now;
        [ProtoMember(5)]
        [Required]
        [DisplayName("From")]
        public DateTime DateFrom { get; set; } = DateTime.Now;
        [ProtoMember(6)]
        [Required]
        [DisplayName("To")]
        public DateTime DateTo { get; set; } = DateTime.Now;
        [ProtoMember(7)]
        [DisplayName("Description")]
        public string Description { get; set; }
        [ProtoMember(8)]
        [Required]
        [DisplayName("Fee")]
        public decimal Fee { get; set; }
        [ProtoMember(9)]
        [Required]
        [DisplayName("Payment Period")]
        public short PaymentPeriod { get; set; }
        public string DateFromStr { get; set; }
        public string DateToStr { get; set; }
    }

    [ProtoContract]
    public class ServiceDisplayDataViewModel : ServiceDisplayViewModel
    {
        [ProtoMember(10)]
        public int Count { get; set; }

        [ProtoMember(11)]
        public string Supplier { get; set; }
    }
}
