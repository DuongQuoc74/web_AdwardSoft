using ProtoBuf;
using System;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    public class EmployeeSalary
    {
        [ProtoMember(1)]
        public int EmployeeId { get; set; }
        [ProtoMember(2)]
        public DateTime EffectiveDate { get; set; }
        [ProtoMember(3)]
        public decimal BasicSalary { get; set; }
        [ProtoMember(4)]
        public short Type { get; set; }
        [ProtoMember(5)]
        public short Wage { get; set; }
        [ProtoMember(6)]
        public decimal ActualWage { get; set; }
    }
}
