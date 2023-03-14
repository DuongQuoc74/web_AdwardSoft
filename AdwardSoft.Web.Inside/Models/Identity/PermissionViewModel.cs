using Microsoft.AspNetCore.Mvc;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    public class PermissionViewModel
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Description { get; set; }
        [ProtoMember(3)]
        [Remote("IsActionControllerExist", "Permission", AdditionalFields = "Action, Id", ErrorMessage = "Action và Controller đã tồn tại")]
        public string Controller { get; set; }
        [ProtoMember(4)]
        [Remote("IsActionControllerExist", "Permission", AdditionalFields = "Controller, Id", ErrorMessage = "Action và Controller đã tồn tại")]
        public string Action { get; set; }
    }
}
