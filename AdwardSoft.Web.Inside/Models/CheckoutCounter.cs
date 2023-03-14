using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    [ProtoInclude(100, typeof(CheckoutCounterDatatableViewModel))]
    public class CheckoutCounterViewModel
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [Required]
        [ProtoMember(2)]
        public string Name { get; set; }

        [ProtoMember(3)]
        public int BranchId { get; set; }

        // 0.Unavailable (Ngừng hoạt động)
        // 1.Available(Đang hoạt động)
        [ProtoMember(4)]
        public short Status { get; set; } = 1;
    }

    [ProtoContract]
    public class CheckoutCounterDatatableViewModel : CheckoutCounterViewModel
    {

    }
}
