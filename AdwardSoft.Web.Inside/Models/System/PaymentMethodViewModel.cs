using Microsoft.AspNetCore.Http;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    public class PaymentMethodViewModel
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        [Required]
        [Display(Name = "Tên phương thức")]
        public string Name { get; set; }
        [ProtoMember(3)]
        [Display(Name = "Biểu tượng")]
        public string Icon { get; set; }

        public IFormFile IconFile { get; set; }
    }
}
