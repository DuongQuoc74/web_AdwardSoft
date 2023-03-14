using Microsoft.AspNetCore.Mvc;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    public class UnitViewModel
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        [Required(ErrorMessage = "Không được để trống")]
        [Display(Name = "Mã đơn vị tính")]
        [StringLength(10)]
        [Remote(action: "CheckCode",  controller: "Unit", AdditionalFields = "Id")]
        
        public string Code { get; set; }
        [ProtoMember(3)]
        [Display(Name = "Tên đơn vị tính")]
        [Required(ErrorMessage = "Không được để trống")]
        
        [StringLength(10)]

        public string Name { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        
        // 0. Unavailable 
        // 1. Available  
        [ProtoMember(4)]
        [Display(Name = "Trạng thái")]
        public short Status { get; set; } = 1;
        
    }
}
