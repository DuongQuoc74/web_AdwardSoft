using ProtoBuf;
using System;
using System.Collections.Generic;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    public class ReceivingReport
    {
        [ProtoMember(1)]
        public string Id { get; set; }
        [ProtoMember(2)]
        public DateTime Date { get; set; } = DateTime.Now;
        [ProtoMember(3)]
        public int SupplierId { get; set; }
        [ProtoMember(4)]
        public int BranchId { get; set; }
        [ProtoMember(5)]
        public string No { get; set; }
        [ProtoMember(6)]
        public string Description { get; set; }
        [ProtoMember(7)]
        public short Status { get; set; }
        [ProtoMember(8)]
        public bool IsPurchaseOrder { get; set; }
        [ProtoMember(9)]
        public decimal TotalQuantity { get; set; }
        [ProtoMember(10)]
        public decimal SubTotal { get; set; }
        [ProtoMember(11)]
        public decimal TaxRate { get; set; }
        [ProtoMember(12)]
        public decimal TaxFee { get; set; }
        [ProtoMember(13)]
        public decimal TotalDiscount { get; set; }
        [ProtoMember(14)]
        public decimal TotalAmount { get; set; }
        [ProtoMember(15)]
        public int PaymentMethodId { get; set; }
        [ProtoMember(16)]
        public long UserId { get; set; }
        [ProtoMember(17)]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [ProtoMember(18)]
        public long CreatedUser { get; set; }
        [ProtoMember(19)]
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        [ProtoMember(20)]
        public long ModifiedUser { get; set; }
    }

    [ProtoContract]
    public class ReceivingReportView
    {
        [ProtoMember(1)]
        public string Id { get; set; }
        [ProtoMember(2)]
        public string No { get; set; }
        [ProtoMember(3)]
        public DateTime Date { get; set; }
        [ProtoMember(4)]
        public short Status { get; set; }
        [ProtoMember(5)]
        public string Supplier { get; set; }
        [ProtoMember(6)]
        public decimal TotalQuantity { get; set; }
        [ProtoMember(7)]
        public decimal TotalAmount { get; set; }
        [ProtoMember(8)]
        public int Count { get; set; }
        [ProtoMember(9)]
        public string Address { get; set; }
        [ProtoMember(10)]
        public string Phone { get; set; }
    }

    [ProtoContract]
    public class ReceivingReportStatus
    {
        [ProtoMember(1)]
        public string Id { get; set; }
        [ProtoMember(2)]
        public short Status { get; set; }
        [ProtoMember(3)]
        public long ModifiedUser { get; set; }
    }

    [ProtoContract]
    public class ReceivingReportTmp
    {
        [ProtoMember(1)]
        public ReceivingReport ReceivingReport { get; set; }
        [ProtoMember(2)]
        public List<ReceivingReportDetail> ReceivingReportDetail { get; set; }
    }
}
