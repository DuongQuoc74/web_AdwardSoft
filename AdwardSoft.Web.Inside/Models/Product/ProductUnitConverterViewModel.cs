using Microsoft.AspNetCore.Mvc;
using ProtoBuf;
using System.ComponentModel.DataAnnotations;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    [ProtoInclude(100, typeof(ProductUnitConverterDatatableViewModel))]
    public class ProductUnitConverterViewModel
    {
        [ProtoMember(1)]
        public int ProductId { get; set; }
        [ProtoMember(2)]
        [Required]
        [Display(Name = "Unit")]
        [Remote(action: "CheckExist", controller: "ProductUnitConverter", ErrorMessage = "Already exist", AdditionalFields = "ProductId,UnitIdCurrnet")]
        public int UnitId { get; set; }
        [ProtoMember(3)]
        [Required]
        [Display(Name = "Quantity From")]
        public decimal QuantityFrom { get; set; }
        [ProtoMember(4)]
        [Required]
        [Display(Name = "To")]
        public decimal QuantityTo { get; set; }

        public int UnitIdCurrnet { get; set; }
    }

    [ProtoContract]
    public class ProductUnitConverterDatatableViewModel : ProductUnitConverterViewModel
    {
        [ProtoMember(5)]
        public string ProductName { get; set; }
        [ProtoMember(6)]
        public string UnitName { get; set; }
    }
}
