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
    [ProtoInclude(100, typeof(CustomerDatatable))]
    public class Customer
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public int PaymentMethodId { get; set; }

        [ProtoMember(3)]
        public int CustomerGroupId { get; set; }

        [ProtoMember(4)]
        public string Name { get; set; }

        [ProtoMember(5)]
        public string IdentityCode { get; set; }

        // 0. Personal – Cá nhân
        // 1. Organization – Tổ chức
        [ProtoMember(6)]
        public short Type { get; set; }

        [ProtoMember(7)]
        public string Phone { get; set; }

        [ProtoMember(8)]
        public string Email { get; set; }

        [ProtoMember(9)]
        public string Address { get; set; }

        [ProtoMember(10)]
        public bool IsInvoice { get; set; } = false;

        [ProtoMember(11)]
        public string Note { get; set; }

        // 0. Unavailable(Ngừng hoạt động)
        // 1. Available(Đang hoạt động))
        [ProtoMember(12)]
        public short Status { get; set; } = 1;

        [ProtoMember(13)]
        public bool IsDefault { get; set; } = false;

        [ProtoMember(14)]
        public int GenderId { get; set; }

        [ProtoMember(15)]
        public DateTime DoB { get; set; }

        [ProtoMember(16)]
        public string Tag { get; set; }
        [ProtoMember(17)]
        public decimal CreditLimit { get; set; }
        [ProtoMember(18)]
        public int PaymentTerm { get; set; }
        [ProtoMember(19)]
        public int StockId { get; set; }
        [ProtoMember(20)]
        public string Representative { get; set; }
        [ProtoMember(21)]
        public string RepPhone { get; set; }
        [ProtoMember(22)]
        public string Operator { get; set; }
        [ProtoMember(23)]
        public string OpePhone { get; set; }
        [ProtoMember(24)]
        public string Map { get; set; }
        [ProtoMember(25)]
        public long UserId { get; set; }
    }

    [ProtoContract]
    public class CustomerDatatable : Customer
    {
        [ProtoMember(101)]
        public int Count { get; set; }

        [ProtoMember(102)]
        public string CustomerGroupName { get; set; }

        [ProtoMember(103)]
        public string GenderName { get; set; }
        [ProtoMember(104)]
        [Display(Name = "Hình thức thanh toán")]
        public string PaymentMethodName { get; set; }
        [ProtoMember(105)]
        [Display(Name = "Kho hàng")]
        public string StockName { get; set; }
        [ProtoMember(106)]
        public byte PricingPolicy { get; set; }
    }

    public class CustomerRedis : Customer
    {
        [AdsKey]
        public new int Id { get; set; }
        [AdsKey]
        public new int PaymentMethodId { get; set; }
        [AdsKey]
        public new int CustomerGroupId { get; set; }
    }

    [ProtoContract]
    public class CustomerElastic
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(3)]
        public string Phone { get; set; }
        [ProtoMember(4)]
        public string IdentityCode { get; set; }
        [ProtoMember(5)]
        public int PaymentMethodId { get; set; }

        public string Tag
        {
            get => " " + Name.ToLower() + " " + StringConvert.convertToUnSign3(Name.ToLower())
                + " " + (Phone ?? "") + " " + (IdentityCode ?? "") + " ";
        }
    }

    [ProtoContract]
    public class CustomerSearch
    {
        [ProtoMember(1)]
        public long Total { get; set; }
        [ProtoMember(2)]
        public List<CustomerElastic> Items { get; set; }
    }

}
