using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreNotificationsLibrary.Modules
{
    public interface IModule<T> where T: Processors.IProcessor
    {
        bool stopFlag { get; set; }
        bool stopped { get;}
        void main();
    }

    public interface IEventModule<T> : IModule<T> where T : Processors.IProcessor { }
}
