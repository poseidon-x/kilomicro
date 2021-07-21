﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreLogic.Models
{
    public class JournalTransactionLine
    {
        public int accountId { get; set; }
        public string description { get; set; }
        public double debit { get; set; }
        public double credit { get; set; }
        public string refNo { get; set; }

    }
}
