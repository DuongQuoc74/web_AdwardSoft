using Microsoft.AspNetCore.Mvc;
using ProtoBuf;
using System.ComponentModel.DataAnnotations;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    public class MembershipClassViewModel
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        [Required]
        [StringLength(60)]
        [Remote( action: "CheckName", controller: "MembershipClass", AdditionalFields = "Id" )]
        public string Name { get; set; }

        [ProtoMember(3)]
        [Display(Name = "Lowest value")]
        [Remote(action: "CheckRange", controller: "MembershipClass", AdditionalFields = "HighestValue, Id")]
        public decimal LowestValue { get; set; }

        [ProtoMember(4)]
        [Display(Name = "Highest value")]
        [Remote(action: "CheckRange", controller: "MembershipClass", AdditionalFields = "LowestValue, Id")]
        public decimal HighestValue { get; set; }

        [ProtoMember(5)]
        public short Level { get; set; }

        [ProtoMember(6)]
        public bool IsDefault { get; set; }
    }

    [ProtoContract]
    public class MembershipClassSortViewModel
    {
        [ProtoMember(1)]
        public int SelectId { get; set; }
        [ProtoMember(2)]
        public bool IsUp { get; set; }
    }
}
