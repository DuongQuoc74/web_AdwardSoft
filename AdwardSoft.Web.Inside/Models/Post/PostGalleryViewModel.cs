using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    public class PostGalleryViewModel
    {
        [ProtoMember(1)]
        public string Id { get; set; }

        [ProtoMember(2)]
        public int PostId { get; set; }

        [ProtoMember(3)]
        public string Image { get; set; }
    }
}
