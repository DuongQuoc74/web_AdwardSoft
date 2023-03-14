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
    [ProtoInclude(100, typeof(CategoryDatatableViewModel))]
    public class CategoryViewModel
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        [Display(Name = "Tên loại")]
        [Required(ErrorMessage ="Tên loại bắt buộc nhập")]
        [MaxLength(60)]
        public string Name { get; set; }
        
        [ProtoMember(3)]

        [Display(Name = "Mô tả")]
        [Required(ErrorMessage = "Mô tả bắt buộc nhập")]       
        [MaxLength(100)]
        public string Description { get; set; }

        [ProtoMember(4)]
        [Display(Name = "Trạng thái")]
        public int Status { get; set; } = 1;
    }
    [ProtoContract]
    public class CategoryDatatableViewModel : CategoryViewModel
    {
        [ProtoMember(5)]
        public int Count { get; set; }
    }
}
