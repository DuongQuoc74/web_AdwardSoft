using ProtoBuf;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    [ProtoInclude(100, typeof(EmployeeShiftDataTable))]
    public class EmployeeShift
    {
        [ProtoMember(1)]
        public int EmployeeId { get; set; }
        [ProtoMember(2)]
        public int ShiftId { get; set; }
        [ProtoMember(3)]
        public decimal Month { get; set; }
        [ProtoMember(4)]
        public decimal Year { get; set; } 
        [ProtoMember(5)]
        public short Type { get; set; }
        [ProtoMember(6)]
        public int CheckoutCounterId { get; set; }
    }

    [ProtoContract]
    public class EmployeeShiftDataTable : EmployeeShift
    {
        [ProtoMember(7)]
        public int Count { get; set; }
        [ProtoMember(8)]
        public string ShiftName { get; set; }
        [ProtoMember(9)]
        public string EmployeeName { get; set; }
        [ProtoMember(10)]
        public string EmployeeCode { get; set; }
    }
}
