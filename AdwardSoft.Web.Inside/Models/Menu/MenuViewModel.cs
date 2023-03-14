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
    public class MenuViewModel
    {
        [ProtoMember(1)]
        [DisplayName("Mã menu")]
        public int Id { get; set; }

        [ProtoMember(2)]
        [DisplayName("Menu cha")]
        [Remote(action: "CheckMenu_IsExisting", controller: "Menu", ErrorMessage = "Không có nhóm menu này")]
        public int ParentId { get; set; }

        [ProtoMember(3)]
        [DisplayName("xếp hạng")]
        public int Order { get; set; }

        [ProtoMember(5)]
        [DisplayName("Loại menu")]
        [Required(ErrorMessage = "Không được để trống")]
        public short Type { get; set; }

        [ProtoMember(6)]
        [DisplayName("Nhóm menu")]
        [Required(ErrorMessage = "Không được để trống")]
        [Remote(action: "CheckMenuGroup_IsExisting", controller: "Menu", ErrorMessage = "Không có nhóm menu này")]
        public int MenuGroupId { get; set; }

        [ProtoMember(7)]
        [DisplayName("Mã ngôn ngữ")]
        public string LanguageCode { get; set; }

        [ProtoMember(8)]
        [DisplayName("Tên đường dẫn")]
        [Required(ErrorMessage = "Không được để trống")]
        [MaxLength(70, ErrorMessage = "Không vượt quá 150 ký tự")]
        public string NavigationLabel { get; set; }

        [ProtoMember(9)]
        [DisplayName("Đường dẫn")]
        [Required(ErrorMessage = "Không được để trống")]
        [MaxLength(2048, ErrorMessage = "Không vượt quá 150 ký tự")]
        public string URL { get; set; }

        [ProtoMember(10)]
        public int ReferenceId { get; set; }

        [ProtoMember(11)]
        public bool IsInMenu { get; set; }

        [ProtoMember(12)]
        [DisplayName("Nhóm menu")]
        [Required(ErrorMessage = "Không được để trống")]
        public string MenuGroupName { get; set; }

        public bool IsNew { get => Id == default(int); }
    }

    [ProtoContract]
    public class MenuTranslationViewModel
    {
        [ProtoMember(1)]
        public int MenuId { get; set; }

        [ProtoMember(2)]
        public string LanguageCode { get; set; }

        [ProtoMember(3)]
        public string URL { get; set; }

        [ProtoMember(4)]
        public string NavigationLabel { get; set; }
    }

    [ProtoContract]
    public class ListMenuViewModel
    {
        public string Title { get; set; }

        public List<MenuViewModel> Data { get; set; }
    }

    public class MenuJson
    {
        public int ReferenceId { get; set; }
        public string Label { get; set; }
        public string Url { get; set; }
        public short Type { get; set; }
        public string LanguageCode { get; set; }
        public int MenuGroupId { get; set; }
        public bool IsInMenu { get; set; }
    }
}
