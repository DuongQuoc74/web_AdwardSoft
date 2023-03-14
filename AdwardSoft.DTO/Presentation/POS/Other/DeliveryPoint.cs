using Nest;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using AdwardSoft.Utilities.Attributes;


namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    [ProtoInclude(100, typeof(DeliveryPointDatatable))]
    public class DeliveryPoint
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]      
        public string Code { get; set; }
        [ProtoMember(3)]       
        public string Name { get; set; }
        [ProtoMember(4)]    
        public int Rate { get; set; }
        [ProtoMember(5)]  
        public int LocationId { get; set; }
        [ProtoMember(6)]     
        public short Status { get; set; } = 1;
    }
    [ProtoContract]
    public class DeliveryPointDatatable : DeliveryPoint
    {
        [ProtoMember(101)]
        public int Count { get; set; }
        [ProtoMember(102)]
        public string LocationName { get; set; }
    }
    public class DeliveryPointRedis : DeliveryPoint
    {
        [AdsKey]
        public new int Id { get; set; }
        [AdsKey]
        public new int LocationId { get; set; }
    }
}
