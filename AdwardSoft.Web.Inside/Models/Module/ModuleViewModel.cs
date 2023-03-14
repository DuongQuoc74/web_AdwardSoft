using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    public class ModuleViewModel
    {

        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        [Display(Name = "Tiêu đề")]
        //[DisplayFormat(ConvertEmptyStringToNull = true)]
        [Required(ErrorMessage = "Tiêu đề không được rỗng")]
        public string Title { get; set; }

        [ProtoMember(3)]
        [Display(Name = "Đường dẫn")]
        //[DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(ErrorMessage = "Đường dẫn không được rỗng")]
        public string Link { get; set; }

        [ProtoMember(4)]
        [Display(Name = "Icon")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ClassName { get; set; }

        [ProtoMember(5)]
        [Display(Name = "Controller")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ControllerName { get; set; }

        [ProtoMember(6)]
        [Display(Name = "Phụ thuộc")]
        public int ParentId { get; set; }

        [ProtoMember(7)]
        public int Sort { get; set; }

        [ProtoMember(8)]
        [Display(Name = "Áp dụng chung")]
        public bool IsPublic { get; set; }

        [ProtoMember(9)]
        public string Types { get; set; }

        [Display(Name = "Nhóm người dùng áp dụng")]
        public List<short> UserTypes { get; set; } = new List<short>();

        public ModuleViewModel()
        {
            ListModule = new List<ModuleViewModel>();
        }
        public IEnumerable<ModuleViewModel> ListModule { get; set; }
    }

    [ProtoContract]
    public class ModuleSortViewModel : JsonData
    {
    }

    [ProtoContract]
    public class ModuleTypeViewModel
    {
        public int ModuleId { get; set; }
        public short Type { get; set; }
    }

    public class ModuleNestable : Nestable
    {
        public string Types { get; set; }
    }
}
