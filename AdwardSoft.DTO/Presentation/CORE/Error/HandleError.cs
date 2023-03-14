using ProtoBuf;

namespace AdwardSoft.DTO.Presentation.Core
{
    [ProtoContract]
    public class HandleError
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public int StatusCode { get; set; }
        [ProtoMember(3)]
        public string LanguageCode { get; set; }
        [ProtoMember(4)]
        public string Title { get; set; }
        [ProtoMember(5)]
        public string Message { get; set; }
    }
}
