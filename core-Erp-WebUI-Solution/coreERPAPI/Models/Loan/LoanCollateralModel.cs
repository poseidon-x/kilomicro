using System;
using System.Collections.Generic;
using coreErpApi.Models;
using coreLogic;

namespace coreErpApi.Models.Loan
{
    public class LoanCollateralModel : loanCollateral
    {
        public int idTypeId { get; set; }
        public string IdNumber { get; set; }
        public int phoneTypeId { get; set; }
        public string phoneNumer { get; set; }
        public string addressLine { get; set; }
        public int cityId { get; set; }
        public string city { get; set; }
        public List<CollateralImageViewModel> collateralPhotos { get; set; }
    }
}