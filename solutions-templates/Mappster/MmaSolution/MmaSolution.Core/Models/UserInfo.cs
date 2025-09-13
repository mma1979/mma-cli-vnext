using System;

namespace MmaSolution.Core.Models
{
    public class UserInfo
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
