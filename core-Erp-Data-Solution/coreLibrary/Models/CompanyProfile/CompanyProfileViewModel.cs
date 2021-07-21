using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreLogic.Models.CompanyProfile
{
    public class CompanyProfileViewModel
    {
        public int companyProfileId { get; set; }
        public string companyName { get; set; }
        public byte[] companyLogo { get; set; }
        public string companyAddressLine { get; set; }
        public string companyPhoneNumber { get; set; }
        public string companyEmail { get; set; }
        public int? companyCityId { get; set; }
        public string companyCity { get; set; }
        public int companyRegionId { get; set; }
        public string companyRegion { get; set; }
        public int? companyCountryId { get; set; }
        public string companyCountry { get; set; }
      
    }
}
