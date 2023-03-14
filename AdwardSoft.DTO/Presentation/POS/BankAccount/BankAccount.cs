using AdwardSoft.Utilities.Attributes;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    [ProtoInclude(100, typeof(BankAccountDatatable))]
    public class BankAccount
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string BankNo { get; set; }
        [ProtoMember(3)]
        public string BankName { get; set; }
        [ProtoMember(4)]
        public short Status { get; set; }
    }

    [ProtoContract]
    public class BankAccountDatatable : BankAccount
    {
        [ProtoMember(5)]
        public int Count { get; set; }
    }

    public class BankAccountRedis : BankAccount
    {
        [AdsKey]
        public new int Id { get; set; }
    }
}
