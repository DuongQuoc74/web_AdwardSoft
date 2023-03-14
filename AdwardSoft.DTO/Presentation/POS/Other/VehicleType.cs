using AdwardSoft.Utilities.Attributes;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdwardSoft.DTO.Presentation.POS
{
        [ProtoContract]
        public class VehicleType
        {
            [ProtoMember(1)]
            public int Id { get; set; }
            [ProtoMember(2)]
            public string Name { get; set; }
            [ProtoMember(3)]
            public short Status { get; set; } = 1;
        }

        public class VehicleTypeRedis : VehicleType
        {
            [AdsKey]
            public new int Id { get; set; }
        }
}
