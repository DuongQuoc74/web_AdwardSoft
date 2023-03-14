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
    public class MenuGroupViewModel
    {
        [ProtoMember(1)]
        [DisplayName("Mã nhóm menu")]
        public int Id { get; set; }

        [ProtoMember(2)]
        [DisplayName("Nhóm menu")]
        [Required(ErrorMessage = "Không được để trống")]
        [MaxLength(150, ErrorMessage = "Không vượt quá 150 ký tự")]
        public string Name { get; set; }

        [ProtoMember(3)]
        [DisplayName("Vị trí")]
        public short Position { get; set; }

        [ProtoMember(4)]
        public int Count { get; set; }

        public bool IsNew { get => Id == default(int); }
    }

    [ProtoContract]
    public class MenuGroupSortViewModel
    {
        [ProtoMember(1)]
        public int SelectedId { get; set; }
        [ProtoMember(2)]
        public bool IsUp { get; set; }
    }
}
