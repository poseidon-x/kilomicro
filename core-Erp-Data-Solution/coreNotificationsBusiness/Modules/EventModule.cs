using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreNotificationsLibrary.Modules
{
    public class EventModule<Processor>:IModule<Processor> where Processor:Processors.IProcessor
    {
        private Processor processor;
        public EventModule(Processor processor)
        {
            this.processor = processor;
        }
        public bool stopFlag
        {
            get;
            set;
        }

        public bool stopped
        {
            get;
            private set;
        }

        public void main()
        {
            stopped = false;
            while (stopFlag == false)
            {
                processor.process();
                System.Threading.Thread.Sleep(15000);
            }
            stopped = true;
        }
    }
}
