using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    public class ProductPrintViewModel
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public string Code { get; set; }

        [ProtoMember(3)]
        public string Name { get; set; }
        [ProtoMember(4)]
        public decimal RetailPrice2 { get; set; }
        [ProtoMember(5)]
        public int Count { get; set; }
    }

    [ProtoContract]
    [ProtoInclude(100, typeof(ProductDataViewModel))]
    public class ProductViewModel
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        
        [Display(Name = "Mã sản phẩm")]
        [RegularExpression(@"^[A-Z0-9]*$", ErrorMessage = "Nhập mã in hoa và không có ký tự đặc biệt")]
        [StringLength(maximumLength: 13, MinimumLength  = 5, ErrorMessage = "Mã tối thiểu 5 ký tự, tối đa 13 ký tự")]
        [Remote(action: "CheckCode", controller: "Product", AdditionalFields = "Id")]
        [Required(ErrorMessage = "Mã sản phẩm không được để trống")]
        //[Required]
        public string Code { get; set; }

        [ProtoMember(3)]
        [Display(Name = "Tên sản phẩm")]
       // [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Tên sản phẩm không được chứa ký tự đặc biệt")]
        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [MaxLength(120)]
        public string Name { get; set; }
        
        [ProtoMember(4)]
        [MaxLength(2048)]
        [Display(Name = "Hình ảnh")]
        public string Image { get; set; }

        [ProtoMember(5)]
        [DisplayName("Tồn kho tối tiểu")]
        public decimal MinStock { get; set; }

        [ProtoMember(6)]
        [DisplayName("Tôn kho tối đa")]
        public decimal MaxStock { get; set; }

        [ProtoMember(7)]
        [Display(Name = "Đơn vị tính")]
        [Required(ErrorMessage = "Đơn vị tính không được để trống")]
        public int UnitId { get; set; }

        [ProtoMember(8)]
        [Display(Name = "Kho mặc định")]
        [Required(ErrorMessage = "Kho không được để trống")]
        public int StockId { get; set; }
        [ProtoMember(9)]
        [Display(Name = "Kiểu tính giá")]
        public short PriceType { get; set; } = 0;
        [ProtoMember(10)]
        [Display(Name = "Cho phép giao dịch")]
        public bool IsTrade { get; set; }
        [ProtoMember(11)]
        [Display(Name = "Ngày cập nhật")]
        public DateTime ModifyDate { get; set; }

        [ProtoMember(12)]
        [StringLength(50)]
        [Display(Name = "Mô tả sản phẩm")]
        public string Description { get; set; }

        [ProtoMember(13)]
        [Display(Name = "Trạng thái")]
        public short Status { get; set; } = 0;
        [ProtoMember(14)]
        [DisplayName("Lợi nhuận (%)")]
        public decimal Profit { get; set; }

    }

    [ProtoContract]
    [ProtoInclude(100, typeof(ProductDatatableViewModel))]
    public class ProductDataViewModel : ProductViewModel
    {

        [ProtoIgnore]
        public IFormFile ImgFile { get; set; }

        [ProtoMember(15)]
        [DisplayName("Phân loại")]
        public int CategoryId { get; set; }
        [ProtoMember(16)]
        [DisplayName("Mặc định")]
        public bool IsDefault { get; set; }
        [ProtoMember(17)]
        [DisplayName("Wholesale Price")]
        public decimal WholesalePrice { get; set; }
        [ProtoMember(18)]
        [DisplayName("Retail Price")]
        public decimal RetailPrice { get; set; }
        [ProtoMember(19)]
        public List<SellingPriceUnitViewModel> UnitPrice { get; set; } = new List<SellingPriceUnitViewModel>();
        
    }

    [ProtoContract]
    public class ProductDatatableViewModel: ProductDataViewModel
    {
        [ProtoMember(80)]
        public int Count { get; set; }

        [ProtoMember(81)]
        public string CategoryName { get; set; }
        [ProtoMember(82)]
        public string UnitName { get; set; }
    }

    [ProtoContract]
    public class ProductVoucherViewModel
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Code { get; set; }
        [ProtoMember(3)]
        public string Name { get; set; }
        [ProtoMember(4)]
        public int UnitId { get; set; }
        [ProtoMember(5)]
        public int StockId { get; set; }
        [ProtoMember(6)]
        public List<SellingPriceViewModel> SellingPrices { get; set; }
    }

    [ProtoContract]
    public class ProductUnitSearch
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(3)]
        public string Code { get; set; }
        [ProtoMember(4)]
        public List<Select> Units { get; set; }
    }

    [ProtoContract]
    public class SellingPriceUnitViewModel
    {
        [ProtoMember(1)]
        [Required]
        [Display(Name = "Quantity From")]
        public decimal QuantityFrom { get; set; }
        [ProtoMember(2)]
        [Required]
        [Display(Name = "To")]
        public decimal QuantityTo { get; set; }
        [ProtoMember(3)]
        [Required]
        [Display(Name = "Wholesale Price")]
        public decimal WholesalePrice { get; set; }
        [ProtoMember(4)]
        [Required]
        [Display(Name = "Retail Price")]
        public decimal RetailPrice { get; set; }
        [ProtoMember(5)]
        [Required]
        [Display(Name = "Unit")]
        public int UnitId { get; set; }
        [ProtoMember(6)]
        public int ProductId { get; set; }
    }
    
}
