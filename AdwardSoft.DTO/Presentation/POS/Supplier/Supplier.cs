using AdwardSoft.DTO.Helper;
using AdwardSoft.Utilities.Attributes;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    [ProtoInclude(100, typeof(SupplierDatatable))]
    public class Supplier
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(3)]
        public string Address { get; set; }
        [ProtoMember(4)]
        public string Tel { get; set; }
        [ProtoMember(5)]
        public string Email { get; set; }
        [ProtoMember(6)]
        public bool IsDefault { get; set; }
    }

    public class SupplierInfo
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(3)]
        public string Address { get; set; }
        [ProtoMember(4)]
        public string Tel { get; set; }
        [ProtoMember(5)]
        public string Email { get; set; }
        [ProtoMember(6)]
        public bool IsDefault { get; set; }
        [ProtoMember(7)]
        public List<SupplierContact> Contacts { get; set; }
    }
    public class SupplierPOS
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(3)]
        public string Address { get; set; }
        [ProtoMember(4)]
        public string Tel { get; set; }
        [ProtoMember(5)]
        public string Email { get; set; }
        [ProtoMember(6)]
        public bool IsDefault { get; set; }
        [ProtoMember(7)]
        public int SupplierId { get; set; }
        [ProtoMember(8)]
        public int Idx { get; set; }
        [ProtoMember(9)]
        public string Phone { get; set; }
        [ProtoMember(10)]
        public string ContactName { get; set; }
        [ProtoMember(11)]
        public string Position { get; set; }
        [ProtoMember(12)]
        public string Note { get; set; }
    }

    [ProtoContract]
    public class SupplierDatatable : Supplier
    {
        [ProtoMember(7)]
        public int Count { get; set; }
    }

    public class SupplierRedis : Supplier
    {
        [AdsKey]
        public new int Id { get; set; }
    }

    public class SupplierElastic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Tel { get; set; }
        public string Phone { get; set; }
        public string NameContact { get; set; }
        public string Tag
        {
            get => " " + Name.ToLower() + " " + StringConvert.convertToUnSign3(Name.ToLower())
                + " " + NameContact.ToLower() + " " + StringConvert.convertToUnSign3(NameContact.ToLower())
                + " " + (Tel ?? "") + " " + (Phone ?? "") + " ";
        }
    }

    [ProtoContract]
    public class SupplierContact
    {
        [ProtoMember(1)]
        public int SupplierId { get; set; }
        [ProtoMember(2)]
        public int Idx { get; set; }
        [ProtoMember(3)]
        public string Phone { get; set; }
        [ProtoMember(4)]
        public string Name { get; set; }
        [ProtoMember(5)]
        public string Position { get; set; }
        [ProtoMember(6)]
        public string Note { get; set; }
        [ProtoMember(7)]
        public bool IsDefault { get; set; }
    }
}
