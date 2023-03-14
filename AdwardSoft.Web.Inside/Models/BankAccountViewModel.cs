using Microsoft.AspNetCore.Mvc;
using ProtoBuf;
using System.ComponentModel.DataAnnotations;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    [ProtoInclude(100, typeof(BankAccountDatatableViewModel))]
    public class BankAccountViewModel
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        [RegularExpression("^[A-Za-z0-9]*$", ErrorMessage = "Không nhập ký tự đặc biệt")]
        [Remote(action: "CheckDuplicateBankNo", controller: "BankAccount", ErrorMessage = "Số tài khoản đã tồn tại", AdditionalFields = "Id")]
        [Display(Name = "Số tài khoản")]
        [Required(ErrorMessage = "Số tài khoản không được để trống")]
        public string BankNo { get; set; }

        [ProtoMember(3)]
        [RegularExpression("^[a-z0-9A-Z ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễếệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ]*$", ErrorMessage = "Không nhập ký tự đặc biệt")]
        [Display(Name = "Tên ngân hàng")]
        [Required(ErrorMessage = "Tên ngân hàng không được để trống")]
        public string BankName { get; set; }

        [ProtoMember(4)]
        [Display(Name = "Trạng thái")]
        public short Status { get; set; }
    }

    [ProtoContract]
    public class BankAccountDatatableViewModel : BankAccountViewModel
    {
        [ProtoMember(5)]
        public int Count { get; set; }
    }
}
