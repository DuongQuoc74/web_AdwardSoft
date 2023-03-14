using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    public class UserBranchViewModel
    {
        [ProtoMember(1)]
        public long UserId { get; set; }
        [ProtoMember(2)]
        [Display(Name = "Branchs")]
        [Required]
        public int BranchId { get; set; }
    }
}
