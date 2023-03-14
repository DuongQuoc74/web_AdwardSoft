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
    [ProtoInclude(100, typeof(CustomerWalletDatatable))]
    public class CustomerWallet
    {
        [ProtoMember(1)]
        public string Id { get; set; }

        [ProtoMember(2)]
        public DateTime Date { get; set; }

        [ProtoMember(3)]
        public string Note { get; set; }
        [ProtoMember(4)]
        public int BankAccountId { get; set; }
        [ProtoMember(5)]
        public decimal Amount { get; set; }
        [ProtoMember(6)]
        public short Status { get; set; } = 0;
        [ProtoMember(7)]
        public int CustomerId { get; set; }
        [ProtoMember(8)]
        public short Type { get; set; } = 0;
    }

    [ProtoContract]
    public class CustomerWalletDatatable : CustomerWallet
    {
        [ProtoMember(101)]
        public string BankNo { get; set; }
        [ProtoMember(102)]
        public string BankName { get; set; }
        [ProtoMember(103)]
        public string CustomerName { get; set; }
    }
    public class CustomerWalletRedis :CustomerWallet
    {
        [AdsKey]
        public new string Id { get; set; }
    }
}
