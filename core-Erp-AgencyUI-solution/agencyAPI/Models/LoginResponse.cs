using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace agencyAPI.Models
{
    public class LoginResponse
    {
        public string token { get; set; }
        public DateTime expiryDate { get; set; }
        public string name { get; set; }
    }
}