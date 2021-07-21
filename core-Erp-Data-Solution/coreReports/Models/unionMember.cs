using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreReports
{
    public class unionMember : vwMember
    {
        public double shareValue
        {
            get
            {
                return this.sharesBalance * this.pricePerShare;
            }
        }
    }
}
