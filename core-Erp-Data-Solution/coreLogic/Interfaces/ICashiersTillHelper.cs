using System;
using System.Collections.Generic;
namespace coreLogic
{
    public interface ICashiersTillHelper
    {
        bool Post(string userName, DateTime dtStartDate, DateTime dtEndDate,
            Dictionary<string, List<int>> table);
    }
}
