using DocumentFormat.OpenXml.Wordprocessing;
using ProtoBuf;
using System.ComponentModel.DataAnnotations;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    [ProtoInclude(100, typeof(LocationDatatableViewModel))]
    public class LocationViewModel
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        [Display(Name = "Mã")]
        [Required(ErrorMessage ="Mã không được để trống")]
        public string Code { get; set; }
        [ProtoMember(3)]
        [Display(Name = "Tên khu vực")]
        [Required(ErrorMessage = "Tên không được để trống")]
        public string Name { get; set; }
        [ProtoMember(4)]
        [Display(Name = "Mã bưu chính")]
        [Required(ErrorMessage = "Mã bưu chính không được để trống")]
        public string PostalCode { get; set; }
        [ProtoMember(5)]
        [Display(Name = "Tỷ lệ (%)")]
        public decimal Rate { get; set; }
        [ProtoMember(6)]
        [Display(Name = "Trực thuộc")]
        public int ParentId { get; set; } = 0;
        [ProtoMember(7)]
        [Display(Name = "Trạng thái")]
        public short Status { get; set; } = 1;
    }
    [ProtoContract]
    public class LocationDatatableViewModel : LocationViewModel
    {
        [ProtoMember(101)]
        public int Count { get; set; }
        [ProtoMember(102)]
        [Display(Name = "Trực thuộc")]
        public string ParentName { get; set; }
    }
}
