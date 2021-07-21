using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreLogic.Models.CompanyProfile;


namespace coreLogic.Models.Email
{
    public class EmailViewModel
    {
        public string senderName { get; set; }
        public string fromAddress { get; set; }
        public string toAddress { get; set; }
        public string fromAddressPassword { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
    }

    public class EmailWithCopyViewModel
    {
        public string smtpHost { get; set; }
        public string senderName { get; set; }
        public string fromAddress { get; set; }
        public string fromAddressPassword { get; set; }
        public List<string> receiptsAddress { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
    }
}
