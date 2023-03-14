using Microsoft.AspNetCore.Mvc;
using ProtoBuf;
using System.ComponentModel.DataAnnotations;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    [ProtoInclude(100, typeof(DeliveryVehicleDatatableViewModel))]
    public class DeliveryVehicleViewModel
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        [RegularExpression("^[a-z0-9A-Z-]*$", ErrorMessage = "Không nhập ký tự đặc biệt")]
        [Remote(action: "CheckDuplicateLicensePlate", controller: "DeliveryVehicle", ErrorMessage = "Biển số đã tồn tại", AdditionalFields = "Id")]
        [Required(ErrorMessage = "Biển số không được để trống")]
        [Display(Name = "Biển số")]
        public string LicensePlate { get; set; }
        [ProtoMember(3)]
        [RegularExpression("^[a-z0-9A-Z ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễếệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ]*$", ErrorMessage = "Không nhập ký tự đặc biệt")]
        [Required(ErrorMessage = "Tên phương tiện không được để trống")]
        [Display(Name = "Tên phương tiện")]
        public string Name { get; set; }
        [ProtoMember(4)]
        [RegularExpression("^[a-z0-9A-Z ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễếệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ]*$", ErrorMessage = "Không nhập ký tự đặc biệt")]
        [Required(ErrorMessage = "Tên tài xế không được để trống")]
        [Display(Name = "Tài xế")]
        public string DriverName { get; set; }
        [ProtoMember(5)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Không nhập ký tự chữ")]
        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [Display(Name = "Điện thoại")]
        public string DriverPhone { get; set; }
        [ProtoMember(6)]
        [RegularExpression(@"^\d{1}(?:\.\d{1,3}){0,4}$", ErrorMessage = "Không vượt quá 1 chữ số phần nguyên và 3 chữ số phần thập phân")]
        [Required(ErrorMessage = "Tải trọng không được để trống")]
        [Display(Name = "Tải trọng")]
        public decimal Load { get; set; }
        [ProtoMember(7)]
        [Display(Name = "Loại phương tiện")]
        public int VehicleTypeId { get; set; }
        [ProtoMember(8)]
        [Display(Name = "Nhà phân phối")]
        public int CustomerId { get; set; }
        [ProtoMember(9)]
        [Display(Name = "Trạng thái")]
        public short Status { get; set; } = 1;
    }

    [ProtoContract]
    public class DeliveryVehicleDatatableViewModel : DeliveryVehicleViewModel
    {
        [ProtoMember(101)]
        public string VehicleTypeName { get; set; }
        [ProtoMember(102)]
        public string CustomerName { get; set; }
    }
}
