﻿//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Web.Http;
//using coreERP;
//using coreLogic;
//using coreERP.Providers;
//using System.Linq.Dynamic;

//namespace coreErpApi.Controllers.Controllers.Inventory.ShrinkageManagement
//{
//    [AuthorizationFilter()]
//    public class UnitOfMeasurementController : ApiController
//    {
//        ICommerceEntities le;

//        public UnitOfMeasurementController()
//        {
//            le = new CommerceEntities();
//            le.Configuration.LazyLoadingEnabled = false;
//            le.Configuration.ProxyCreationEnabled = false;
//        }

//        public UnitOfMeasurementController(ICommerceEntities lent)
//        {
//            le = lent;
//        }

//        [HttpGet]
//        // GET: api/Product
//        public IEnumerable<unitOfMeasurement> Get()
//        {
//            return le.unitOfMeasurements
//                .OrderBy(p => p.unitOfMeasurementName)
//                .ToList();
//        }

       
//    }
//}