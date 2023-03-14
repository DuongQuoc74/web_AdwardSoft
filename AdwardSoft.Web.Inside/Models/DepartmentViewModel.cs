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
    public class DepartmentViewModel
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        [Required]
        [Display(Name = "Name")]
        [MaxLength(80, ErrorMessage = "Do not exceed 120 characters")]
        public string Name { get; set; }
        [ProtoMember(3)]
        [Required]
        [Display(Name = "Description")]
        [MaxLength(300, ErrorMessage = "Do not exceed 128 characters")]
        public string Description { get; set; }
        [ProtoMember(4)]
        [Required]
        [DisplayName("Main")]
        public int ParentId { get; set; }
        [ProtoMember(5)]
        public int Sort { get; set; }
    }
}
