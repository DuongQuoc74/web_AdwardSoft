using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    public class DashboardSumary
    {
        [ProtoMember(1)]
        public int NewCustomer { get; set; }
        [ProtoMember(2)]
        public int TotalOrder { get; set; }
        [ProtoMember(3)]
        public decimal TotalSale { get; set; }
        [ProtoMember(4)]
        public decimal TotalSaleMonth { get; set; }
        [ProtoMember(5)]
        public decimal TotalCashier1 { get; set; } 
        [ProtoMember(6)]
        public decimal TotalCashier2 { get; set; }
        [ProtoMember(7)]
        public decimal TotalDayCashier1 { get; set; }
        [ProtoMember(8)]
        public decimal TotalDayCashier2 { get; set; }
    }
    [ProtoContract]
    public class DashboardPie
    {
        [ProtoMember(1)]
        public string Name { get; set; }
        [ProtoMember(2)]
        public decimal Value { get; set; }
    }
    [ProtoContract]
    public class DashboardBar
    {
        [ProtoMember(1)]
        public int Month { get; set; }
        [ProtoMember(2)]
        public decimal Amount { get; set; }
    }
}
