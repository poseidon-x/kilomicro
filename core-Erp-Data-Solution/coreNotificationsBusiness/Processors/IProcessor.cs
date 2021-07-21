using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreNotificationsLibrary.Processors
{
    public interface IProcessor
    {
        int messageEventCategoryID { get; }
        DateTime lastProcessTime { get; }
        void process();
    }

    public interface IEventCollectionProcessor : IProcessor { }

    public interface IEventLoanRepaymentsCollectionProcessor : IProcessor { }
}
