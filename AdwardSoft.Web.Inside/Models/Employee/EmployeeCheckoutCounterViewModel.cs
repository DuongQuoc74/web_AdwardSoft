using ProtoBuf;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    public class EmployeeCheckoutCounterViewModel
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
        public int CheckoutCounterId { get; set; }
    }
}
