using AdwardSoft.Utilities.Attributes;
using Nest;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    [ProtoInclude(100, typeof(PriceDetailDatatable))]
    public class PriceDetail
    {
        [ProtoMember(1)]
        public int ProductId { get; set; }
        [ProtoMember(2)]
        public int LocationId { get; set; }
        [ProtoMember(3)]
        public int DeliveryPointId { get; set; }
        [ProtoMember(4)]
        public short DeliveryType { get; set; } = 1;
        [ProtoMember(5)]
        public decimal Price { get; set; }
        [ProtoMember(6)]
        public DateTime Date { get; set; }
    }
    [ProtoContract]
    public class PriceDetailDatatable : PriceDetail
    {
        [ProtoMember(101)]
        public int Count { get; set; }
        [ProtoMember(102)]
        public string ProductName { get; set; }
        [ProtoMember(103)]
        public string LocationName { get; set; }
        [ProtoMember(104)]
        public string DeliveryPointName { get; set; }
        [ProtoMember(105)]
        public string UnitName { get; set; }
    }
    public class PriceDetailRedis : PriceDetail
    {
        [AdsKey]
        public new int ProductId{ get; set; }
        [AdsKey]
        public new int LocationId { get; set; }
        [AdsKey]
        public new int DeliveryPointId { get; set; }
        [AdsKey]
        public new short DeliveryType { get; set; }
    }
}