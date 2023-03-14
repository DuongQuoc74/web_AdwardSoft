using AdwardSoft.DTO.Helper;
using AdwardSoft.Utilities.Attributes;
using Nest;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    [ProtoInclude(100, typeof(CustomerOrderDatatable))]
    public class CustomerOrder
    {
        [ProtoMember(1)]
        public string Id { get; set; }
        [ProtoMember(2)]
        public DateTime Date { get; set; } = DateTime.Now;
        [ProtoMember(3)]
        public int CustomerId { get; set; }
        [ProtoMember(4)]
        public int BranchId { get; set; }
        [ProtoMember(5)]
        public string No { get; set; }
        [ProtoMember(6)]
        public string Description { get; set; }
        [ProtoMember(7)]
        [Required]
        public DateTime? DeliveryDate { get; set; }
        [ProtoMember(8)]
        [Required]
        public int DeliveryPointId { get; set; }
        [ProtoMember(9)]
        [Required]
        public int DeliveryVehicleId { get; set; }
        [ProtoMember(10)]
        [Required]
        public string Receiver { get; set; }
        [ProtoMember(11)]
        [Required]
        public string ReceiverPhone { get; set; }
        [ProtoMember(12)]
        [Required]
        public string ReceiverAddress { get; set; }
        [ProtoMember(13)]
        [Required]
        public short DeliveryType { get; set; }
        [ProtoMember(14)]
        public string VoucherCode { get; set; }
        [ProtoMember(15)]
        public short Status { get; set; } = 0;
        [ProtoMember(16)]
        public bool IsShipping { get; set; }
        [ProtoMember(17)]
        public decimal TotalQuantityReg { get; set; }
        [ProtoMember(18)]
        public decimal TotalQuantity { get; set; }
        [ProtoMember(19)]
        public decimal SubTotal { get; set; }
        [ProtoMember(20)]
        public decimal ShippingFee { get; set; }
        [ProtoMember(21)]
        public decimal TaxRate { get; set; }
        [ProtoMember(22)]
        public decimal TaxFee { get; set; }
        [ProtoMember(23)]
        public decimal TotalDiscount { get; set; }
        [ProtoMember(24)]
        public decimal TotalAmount { get; set; }
        [ProtoMember(25)]
        public DateTime? PaymentDate { get; set; }
        [ProtoMember(26)]
        public long PaymentUser { get; set; }
        [ProtoMember(27)]
        public short PaymentStatus { get; set; }
        [ProtoMember(28)]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [ProtoMember(29)]
        public long CreatedUser { get; set; }
        [ProtoMember(30)]
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        [ProtoMember(31)]
        public long ModifiedUser { get; set; }
        [ProtoMember(32)]
        public int PaymentMethodId { get; set; }
        [ProtoMember(33)]
        public List<CustomerOrderDetailDatatable> OrderDetail { get; set; } = new List<CustomerOrderDetailDatatable>();
    }

    [ProtoContract]
    public class CustomerOrderDatatable : CustomerOrder
    {
        [ProtoMember(101)]
        public int Count { get; set; }

        [ProtoMember(102)]
        public string CustomerName { get; set; }
    }

    public class CustomerOrderRedis : CustomerOrder
    {
        [AdsKey]
        public new string Id { get; set; }
    }
}
