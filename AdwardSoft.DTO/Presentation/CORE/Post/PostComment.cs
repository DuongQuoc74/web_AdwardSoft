using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdwardSoft.DTO.Presentation.Core
{
    [ProtoContract]
    public class PostComment
    {
        [ProtoMember(1)]
        public string Id { get; set; }
        [ProtoMember(2)]
        public long PostId { get; set; }
        [ProtoMember(3)]
        public long UserId { get; set; }
        [ProtoMember(4)]
        public DateTime Date { get; set; }
        [ProtoMember(5)]
        public string ParentId { get; set; }
        [ProtoMember(6)]
        public string Comment { get; set; }
        [ProtoMember(7)]
        public short Status { get; set; }
        [ProtoMember(8)]
        public string UserName { get; set; }
        [ProtoMember(9)]
        public string Avatar { get; set; }
    }

    [ProtoContract]
    public class PostCommentDataTable
    {
        [ProtoMember(1)]
        public long Id { get; set; }
        [ProtoMember(2)]
        public string Title { get; set; }
        [ProtoMember(3)]
        public int Comment { get; set; }
        [ProtoMember(4)]
        public int CommentNA { get; set; }
        [ProtoMember(5)]
        public int Count { get; set; }
    }

    [ProtoContract]
    public class PostCommentStatus
    {
        [ProtoMember(1)]
        public string Id { get; set; }
        [ProtoMember(2)]
        public int Status { get; set; }
    }
}
