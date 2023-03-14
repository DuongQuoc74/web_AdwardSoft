using AdwardSoft.Utilities.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdwardSoft.DTO.Identity
{
    public class OTPRedis
    {
        [AdsKey]
        public string Id { get; set; }
        [AdsKey]
        public string PhoneNumber { get; set; }
        [AdsKey]
        public string OTP { get; set; }
        public DateTime ExpireTime { get; set; }
    }
}
