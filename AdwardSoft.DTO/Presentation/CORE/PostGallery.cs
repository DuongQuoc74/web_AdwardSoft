using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdwardSoft.DTO.Presentation.CMS
{
    [ProtoContract]
    public class PostGallery
    {
        [ProtoMember(1)]
        public string Id { get; set; }

        [ProtoMember(2)]
        public int PostId { get; set; }

        [ProtoMember(3)]
        public string Image { get; set; }
    }
}
