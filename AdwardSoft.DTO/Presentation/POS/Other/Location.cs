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
    [ProtoInclude(100, typeof(LocationDatatable))]
    public class Location
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Code { get; set; }
        [ProtoMember(3)]
        public string Name { get; set; }
        [ProtoMember(4)]
        public string PostalCode { get; set; }
        [ProtoMember(5)]
        public decimal Rate { get; set; }
        [ProtoMember(6)]
        public int ParentId { get; set; } = 0;
        [ProtoMember(7)]
        public short Status { get; set; } = 1;
    }
    [ProtoContract]
    public class LocationDatatable : Location
    {
        [ProtoMember(101)]
        public int Count { get; set; }
        [ProtoMember(102)]
        public string ParentName { get; set; }
    }
    public class LocationRedis : Location
    {
        [AdsKey]
        public new int Id { get; set; }
        [AdsKey]
        public new int ParentId { get; set; }
    }
}
