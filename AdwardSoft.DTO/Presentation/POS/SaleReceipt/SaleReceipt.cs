using ProtoBuf;
using System;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    [ProtoInclude(100, typeof(SaleReceiptDataTable))]
    [ProtoInclude(101, typeof(SaleReceiptDisplay))]
    [ProtoInclude(102, typeof(SaleReceiptHistory))]
    public class SaleReceipt
    {
        [ProtoMember(1)]
        public string Id { get; set; }
        [ProtoMember(2)]
        public DateTime Date { get; set; }
        [ProtoMember(3)]
        public int CustomerId { get; set; }
        [ProtoMember(4)]
        public int BranchId { get; set; }
        [ProtoMember(5)]
        public string No { get; set; }
        [ProtoMember(6)]
        public string Description { get; set; }
        [ProtoMember(7)]
        public short Status { get; set; }
        [ProtoMember(8)]
        public bool IsShipping { get; set; }
        [ProtoMember(9)]
        public bool IsCustomerOrder { get; set; }
        [ProtoMember(10)]
        public decimal TotalQuantity { get; set; }
        [ProtoMember(11)]
        public decimal SubTotal { get; set; }
        [ProtoMember(12)]
        public decimal ShippingFee { get; set; }
        [ProtoMember(13)]
        public decimal TaxRate { get; set; }

        [ProtoMember(14)]
        public decimal TaxFee { get; set; }
        [ProtoMember(15)]
        public decimal TotalDiscount { get; set; }
        [ProtoMember(16)]
        public decimal TotalAmount { get; set; }
        [ProtoMember(17)]
        public decimal PaymentMethodId { get; set; }
        [ProtoMember(18)]
        public DateTime CreateDate { get; set; }
        [ProtoMember(19)]
        public long CreatedUser { get; set; }
        [ProtoMember(20)]
        public DateTime ModifiedDate { get; set; }
        [ProtoMember(21)]
        public long ModifiedUser { get; set; }
        [ProtoMember(22)]
        public long EmployeeId { get; set; }
        [ProtoMember(23)]
        public int ShiftId { get; set; }
        [ProtoMember(24)]
        public int CheckoutCounterId { get; set; }
        [ProtoMember(25)]
        public byte PriceType { get; set; }
        [ProtoMember(26)]
        public int StockId { get; set; }
        [ProtoMember(27)]
        public decimal ExchangePoint { get; set; }
        [ProtoMember(28)]
        public decimal ExchangeAmount { get; set; }
        [ProtoMember(29)]
        public decimal Cash { get; set; }
        [ProtoMember(30)]
        public string SaleReceiptIdRef { get; set; }
    }

    [ProtoContract]
    public class SaleReceiptDisplay : SaleReceipt
    {
        [ProtoMember(31)]
        public string CustomerName { get; set; }
        [ProtoMember(32)]
        public string CustomerPhone { get; set; }
        [ProtoMember(33)]
        public string CustomerAddress { get; set; }
        [ProtoMember(34)]
        public string BranchName { get; set; }
        [ProtoMember(35)]
        public string PaymentMethodName { get; set; }
        [ProtoMember(36)]
        public string CheckoutCounterName { get; set; }
    }
    [ProtoContract]
    public class SaleReceiptDataTable : SaleReceipt
    {
        [ProtoMember(31)]
        public long Count { get; set; }

        [ProtoMember(32)]
        public string CustomerName { get; set; }

        [ProtoMember(33)]
        public string CreatedUserName { get; set; }

        [ProtoMember(34)]
        public string ModifiedUserName { get; set; }

        [ProtoMember(35)]
        public string BranchName { get; set; }
    }

    [ProtoContract]
    public class SaleReceiptHistory : SaleReceipt
    {
        [ProtoMember(30)]
        public long Count { get; set; }

        [ProtoMember(31)]
        public string CustomerName { get; set; }

        [ProtoMember(32)]
        public string CreatedUserName { get; set; }

        [ProtoMember(33)]
        public string ModifiedUserName { get; set; }

        [ProtoMember(34)]
        public string BranchName { get; set; }
        [ProtoMember(35)]
        public string PriceTypeName { get; set; }
        [ProtoMember(36)]
        public string SaleReceiptId { get; set; }
        [ProtoMember(37)]
        public string SaleReceiptDetailId { get; set; }
        [ProtoMember(38)]
        public int ProductId { get; set; }
        [ProtoMember(39)]
        public decimal Quantity { get; set; }
        [ProtoMember(40)]
        public int UnitId { get; set; }
        [ProtoMember(41)]
        public decimal Price { get; set; }
        [ProtoMember(42)]
        public int PromotionId { get; set; }
        [ProtoMember(43)]
        public decimal Discount { get; set; }
        [ProtoMember(44)]
        public decimal Amount { get; set; }
        [ProtoMember(45)]
        public bool IsPromo { get; set; }
        [ProtoMember(46)]
        public decimal RetailPrice { get; set; }
        [ProtoMember(47)]
        public string ProductName { get; set; }
        [ProtoMember(48)]
        public string PromotionName { get; set; }
        [ProtoMember(49)]
        public string ProductCode { get; set; }
        [ProtoMember(50)]
        public string UnitName { get; set; }
        [ProtoMember(51)]
        public bool IsUnitConverter { get; set; }
    }
}
