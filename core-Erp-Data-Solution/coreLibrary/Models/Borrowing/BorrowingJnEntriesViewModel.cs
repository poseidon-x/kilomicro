using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreLogic.Models.Borrowing
{
    public class BorrowingJnEntriesViewModel
    {
        public int no { get; set; }
        public DateTime? date { get; set; }
        public string dateString { get; set; }

        public string desc { get; set; }
        public double drAmount { get; set; }
        public double crAmount { get; set; }
        public double bal { get; set; }
    }
}
