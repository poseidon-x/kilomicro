using OpenAndCloseTillService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenAndCloseTillService
{
    public partial class TillConfigService : ServiceBase
    {
        OpenTillProcessor OpenMOd;
       // CloseTillProcessor CloseMod;



        public TillConfigService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
           OpenMOd = new OpenTillProcessor();


            new Thread(new ThreadStart(OpenMOd.Main)).Start();
        }

        protected override void OnStop()
        {
            OpenMOd.StopFlag = true;
        }
    }
}
