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
    [ProtoInclude(100, typeof(DeliveryVehicleDatatable))]
    public class DeliveryVehicle
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string LicensePlate { get; set; }
        [ProtoMember(3)]
        public string Name { get; set; }
        [ProtoMember(4)]
        public string DriverName { get; set; }
        [ProtoMember(5)]
        public string DriverPhone { get; set; }
        [ProtoMember(6)]
        public decimal Load { get; set; }
        [ProtoMember(7)]
        public int VehicleTypeId { get; set; }
        [ProtoMember(8)]
        public int CustomerId { get; set; }
        [ProtoMember(9)]
        public short Status { get; set; }
    }

    [ProtoContract]
    public class DeliveryVehicleDatatable : DeliveryVehicle
    {
        [ProtoMember(101)]
        public string VehicleTypeName { get; set; }
        [ProtoMember(102)]
        public string CustomerName { get; set; }
    }

    public class DeliveryVehicleRedis : DeliveryVehicle
    {
        [AdsKey]
        public new int Id { get; set; }
    }
}
