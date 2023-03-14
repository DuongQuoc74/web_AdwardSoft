using AdwardSoft.DTO.Helper;
using AdwardSoft.Utilities.Attributes;
using ProtoBuf;
using System;
using System.Collections.Generic;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    [ProtoInclude(100, typeof(EmployeeDataTable))]
    public class Employee
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(3)]
        public string Phone { get; set; }
        [ProtoMember(4)]
        public string Address { get; set; }
        [ProtoMember(5)]
        public string IdentityCode { get; set; }
        [ProtoMember(6)]
        public string Email { get; set; }
        [ProtoMember(7)]
        public int GenderId { get; set; }
        [ProtoMember(8)]
        public int DepartmentId { get; set; }
        [ProtoMember(9)]
        public int PositionId { get; set; }
        [ProtoMember(10)]
        public DateTime DoB { get; set; }
        [ProtoMember(11)]
        public string Avatar { get; set; }
        [ProtoMember(12)]
        public string Code { get; set; }
        [ProtoMember(13)]
        public string PlaceOfBirth { get; set; }
        [ProtoMember(14)]
        public string Nationality { get; set; }
        [ProtoMember(15)]
        public short Religious { get; set; }
        [ProtoMember(16)]
        public string PermanentAddress { get; set; }
        [ProtoMember(17)]
        public string CurrentAddress { get; set; }
        [ProtoMember(18)]
        public DateTime LeavingDate { get; set; }
        [ProtoMember(19)]
        public DateTime StartingDate { get; set; }
        [ProtoMember(20)]
        public short Status { get; set; }

    }

    [ProtoContract]
    public class EmployeeDataTable : Employee
    {
        [ProtoMember(21)]
        public int Count { get; set; }
        [ProtoMember(22)]
        public decimal ActualWage { get; set; }
        [ProtoMember(23)]
        public string UserName { get; set; }
    }

    public class EmployeeRedis : Employee
    {
        [AdsKey]
        public new int Id { get; set; }
    }

    [ProtoContract]
    public class EmployeeElastic
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Code { get; set; }
        [ProtoMember(3)]
        public string Name { get; set; }
        [ProtoMember(4)]
        public string Phone { get; set; }
        [ProtoMember(5)]
        public string IdentityCode { get; set; }

        public string Tag
        {
            get => " " + Name.ToLower() + " " + StringConvert.convertToUnSign3(Name.ToLower())
            + " " + (Phone ?? "") + " " + (IdentityCode ?? "") + " " + (Code ?? "") + " ";
        }
    }

    [ProtoContract]
    public class EmployeeSearch
    {
        [ProtoMember(1)]
        public long Total { get; set; }
        [ProtoMember(2)]
        public List<EmployeeElastic> Items { get; set; }
    }
}
