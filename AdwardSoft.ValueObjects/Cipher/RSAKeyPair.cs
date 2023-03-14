using System;
using System.Collections.Generic;
using System.Text;

namespace AdwardSoft.ValueObjects.Cipher
{
    public class KeyPairCipher
    {
        public string BusinessCode { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string HashKey { get; set; }
    }
}
