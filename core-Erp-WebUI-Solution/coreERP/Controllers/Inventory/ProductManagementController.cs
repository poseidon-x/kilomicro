using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace coreERP.Controllers.Inventory
{
    public class ProductManagementController: Controller
    {
        [HttpGet]
        public ActionResult Product(long? id)
        {
            if (id == null)
            {
                ViewBag.id = -1;
            }
            else
            {
                ViewBag.id = id.Value;
            }
            return View();
        }


        public ActionResult ProductCategory(long? id)
        {
            
            ViewBag.id = id ?? -1;
            return View();
        }

        public ActionResult ProductSubCategory()
        {
            return ViewBag();
        }


        public ActionResult Products()
        {
            return View();
        }

        public ActionResult ProductCategories()
        {
            return View();
        }
    }
}
