using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    [ProtoInclude(100, typeof(ShiftDatatableViewModel))]
    public class ShiftViewModel
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [Required]
        [ProtoMember(2)]
        public string Name { get; set; }

        [Display(Name = "Is Monday")]
        [ProtoMember(3)]
        public bool IsMonday { get; set; }

        [Display(Name = "Is Tuesday")]
        [ProtoMember(4)]
        public bool IsTuesday { get; set; }

        [Display(Name = "Is Wednesday")]
        [ProtoMember(5)]
        public bool IsWednesday { get; set; }

        [Display(Name = "Is Thursday")]
        [ProtoMember(6)]
        public bool IsThursday { get; set; }

        [Display(Name = "Is Friday")]
        [ProtoMember(7)]
        public bool IsFriday { get; set; }

        [Display(Name = "Is Saturday")]
        [ProtoMember(8)]
        public bool IsSaturday { get; set; }

        [Display(Name = "Is Monday")]
        [ProtoMember(9)]
        public bool IsSunday { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        [ProtoMember(10)]
        public string StartTime { get; set; }

        [Required]
        [Display(Name = "End Time")]
        [ProtoMember(11)]
        public string EndTime { get; set; }

        [ProtoMember(12)]
        public int BranchId { get; set; }
    }

    [ProtoContract]
    public class ShiftDatatableViewModel : ShiftViewModel
    {

    }
}
