using AdwardSoft.DTO.Helper;
using AdwardSoft.Utilities.Attributes;
using Nest;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    [ProtoInclude(100, typeof(CustomerOrderDetailDatatable))]
    public class CustomerOrderDetail
    {
        [ProtoMember(1)]
        public int ProductId { get; set; }
        [ProtoMember(2)]
        public int UnitId { get; set; }
        [ProtoMember(3)]
        public decimal Price { get; set; }
        [ProtoMember(4)]
        public decimal QuantityReg { get; set; }
        [ProtoMember(5)]
        public decimal Quantity { get; set; }
        [ProtoMember(6)]
        public string OrderId { get; set; }
        [ProtoMember(7)]
        public decimal Discount { get; set; }
        [ProtoMember(8)]
        public decimal Amount { get; set; }
        [ProtoMember(9)]
        public bool IsPromo { get; set; }
    }

    [ProtoContract]
    public class CustomerOrderDetailDatatable : CustomerOrderDetail
    {
        [ProtoMember(101)]
        public string ProductName { get; set; }
        [ProtoMember(102)]
        public string ProductCode { get; set; }
        [ProtoMember(103)]
        public string UnitName { get; set; }
        [ProtoMember(104)]
        public bool IsOrdered { get; set; } = false;
    }
}
