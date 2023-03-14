using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Models
{
    public class SaleReceiptAllViewModel
    {
        public SaleReceiptViewModel Infor { get; set; }
        public List<SaleReceiptDetailDataTableViewModel> Detail { get; set; }
    }

    [ProtoContract]
    [ProtoInclude(100, typeof(SaleReceiptDataTableViewModel))]
    [ProtoInclude(101, typeof(SaleReceiptDisplayViewModel))]
    public class SaleReceiptViewModel
    {
        [ProtoMember(1)]
        public string Id { get; set; }
        [ProtoMember(2)]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }
        [ProtoMember(3)]
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }
        [ProtoMember(4)]
        [Display(Name = "Branch")]
        public int BranchId { get; set; }
        [ProtoMember(5)]
        [Display(Name = "No")]
        public string No { get; set; }
        [ProtoMember(6)]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [ProtoMember(7)]
        [Display(Name = "Status")]
        public short Status { get; set; }
        [ProtoMember(8)]
        [Display(Name = "Shipping")]
        public bool IsShipping { get; set; }
        [ProtoMember(9)]
        [Display(Name = "Order for customer")]
        public bool IsCustomerOrder { get; set; }
        [ProtoMember(10)]
        [Display(Name = "Total Quantity")]
        public decimal TotalQuantity { get; set; }
        [ProtoMember(11)]
        [Display(Name = "Sub Total")]
        public decimal SubTotal { get; set; }
        [ProtoMember(12)]
        [Display(Name = "Shipping Fee")]
        public decimal ShippingFee { get; set; }
        [ProtoMember(13)]
        [Display(Name = "Tax Rate")]
        public decimal TaxRate { get; set; }
        [ProtoMember(14)]
        [Display(Name = "Tax Fee")]
        public decimal TaxFee { get; set; }
        [ProtoMember(15)]
        [Display(Name = "Total Discount")]
        public decimal TotalDiscount { get; set; }
        [ProtoMember(16)]
        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }
        [ProtoMember(17)]
        [Display(Name = "Payment Method")]
        public decimal PaymentMethodId { get; set; }
        [ProtoMember(18)]
        [Display(Name = "Create Date")]
        public DateTime CreateDate { get; set; }
        [ProtoMember(19)]
        [Display(Name = "Created User")]
        public long CreatedUser { get; set; }
        [ProtoMember(20)]
        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }
        [ProtoMember(21)]
        [Display(Name = "Modified User")]
        public long ModifiedUser { get; set; }
        [ProtoMember(22)]
        [Display(Name = "Employee")]
        public long EmployeeId { get; set; }
        [ProtoMember(23)]
        [Display(Name = "Shift")]
        public int ShiftId { get; set; }
        [ProtoMember(24)]
        [Display(Name = "Checkout Counter")]
        public int CheckoutCounterId { get; set; }
        [ProtoMember(25)]
        [Display(Name = "Price Types")]
        public byte PriceType { get; set; }
        [ProtoMember(26)]
        [Display(Name = "Stock")]
        public int StockId { get; set; }
    }

    [ProtoContract]
    public class SaleReceiptDisplayViewModel: SaleReceiptViewModel
    {
        [ProtoMember(27)]
        [Display(Name = "Name")]
        public string CustomerName { get; set; }
        [ProtoMember(28)]
        [Display(Name = "Phone")]
        public string CustomerPhone { get; set; }
        [ProtoMember(29)]
        [Display(Name = "Address")]
        public string CustomerAddress { get; set; }
        [ProtoMember(30)]
        [Display(Name = "Branch")]
        public string BranchName { get; set; }
        [ProtoMember(31)]
        [Display(Name = "Payment Method")]
        public string PaymentMethodName { get; set; }
        [ProtoMember(32)]
        [Display(Name = "Checkout Counter")]
        public string CheckoutCounterName { get; set; }
    }

    [ProtoContract]
    [ProtoInclude(101, typeof(SaleReceiptViewModel))]
    public class SaleReceiptDataTableViewModel : SaleReceiptViewModel
    {
        [ProtoMember(27)]
        public long Count { get; set; }

        [ProtoMember(28)]
        [Display(Name = "Customer")]
        public string CustomerName { get; set; }

        [ProtoMember(29)]
        [Display(Name = "Created")]
        public string CreatedUserName { get; set; }

        [ProtoMember(30)]
        [Display(Name = "Modified")]
        public string ModifiedUserName { get; set; }

        [ProtoMember(31)]
        [Display(Name = "Branch")]
        public string BranchName { get; set; }
    }
}
