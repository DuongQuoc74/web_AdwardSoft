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
    public class PriceList
    {
        [ProtoMember(1)]
        public DateTime Date { get; set; }
        [ProtoMember(2)]
        public string Title { get; set; }
        [ProtoMember(3)]
        public string Note { get; set; }
        [ProtoMember(4)]
        public short Status { get; set; } = 1;
    }
    public class PriceListRedis : PriceList
    {
        [AdsKey]
        public new DateTime Date { get; set; }

    }
}
