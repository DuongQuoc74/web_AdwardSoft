using Microsoft.AspNetCore.Mvc;
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
    [ProtoInclude(100, typeof(ScoreConfigurationDatatableViewModel))]
    public class ScoreConfigurationViewModel
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        [Required]
        [DisplayName("Effective Date")]
        public DateTime EffectiveDate { get; set; }
        [ProtoMember(3)]
        [Required]
        [DisplayName("From Amount")]
        public double FromAmount { get; set; }
        [ProtoMember(4)]
        [Required]
        [DisplayName("To Amount")]
        public double ToAmount { get; set; }
        [ProtoMember(5)]
        [Required]
        [DisplayName("From Point")]
        public double FromPoint { get; set; }
        [ProtoMember(6)]
        [Required]
        [DisplayName("To Point")]
        public double ToPoint { get; set; }

        [Remote(action: "CheckDate", controller: "ScoreConfiguration", AdditionalFields = "Id")]
        public string DateStr { get; set; }


        //public string DateStr { get; set; }
    }
    [ProtoContract]
    public class ScoreConfigurationDatatableViewModel : ScoreConfigurationViewModel
    {
        [ProtoMember(7)]
        public int Count { get; set; }
    }
}
