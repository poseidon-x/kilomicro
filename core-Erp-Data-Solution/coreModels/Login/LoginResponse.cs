using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreModels.Login
{
    public class LoginResponse
    {
        public string token { get; set; }
        public DateTime expiryDate { get; set; }

        public string name { get; set; }
    }
}
