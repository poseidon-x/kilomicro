using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace coreERP.Controllers
{
    public class InventoryConfigurationController: Controller
    {
   
        [HttpGet]
        public ActionResult Brand()
        {
            return View();
        }
     
        public ActionResult InventoryMethod()
        {
            return View();
        }       
        public ActionResult InventoryLocation()
        {
            return View();
        }
      
        public ActionResult LocationType()
        {
            return View();
        }

        public ActionResult EnterLocationType()
        {
            return View();
        }

        public ActionResult ShrinkageReason()
        {
            return View();
        }
        public ActionResult UnitOfMeasurement()
        {
            return View();
        }

        public ActionResult InventoryItem()
        {
            return View();
        }
    }
}
