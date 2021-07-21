//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web.Http;
//using coreLogic;
//using coreERP.Providers;

//namespace coreERP.Controllers.hc
//{
//    [AuthorizationFilter()]
//    public class QualificationTypeController : ApiController
//    {
//        coreLoansEntities le = new coreLoansEntities();

//        public QualificationTypeController()
//        {
//            le.Configuration.LazyLoadingEnabled = false;
//            le.Configuration.ProxyCreationEnabled = false;
//        }

//        // GET: api/qualificationType
//        public IEnumerable<qualificationType> Get()
//        {
//            return le.qualificationTypes
//                .OrderBy(p => p.qualificationTypeID)
//                .ToList();
//        }

//        // GET: api/qualificationType/5
//        [HttpGet]
//        public qualificationType Get(int id)
//        {
//            return le.qualificationTypes
//                .FirstOrDefault(p => p.qualificationTypeID == id);
//        }

//        [HttpPost]
//        // POST: api/qualificationType
//        public qualificationType Post([FromBody]qualificationType value)
//        {
//            le.qualificationTypes.Add(value);
//            le.SaveChanges();

//            return value;
//        }

//        [HttpPut]
//        // PUT: api/qualificationType/5
//        public qualificationType Put([FromBody]qualificationType value)
//        {
//            var toBeUpdated = new qualificationType
//            {
//                qualificationTypeID = value.qualificationTypeID,
//                qualificationTypeName = value.qualificationTypeName,
//            };
//            le.Entry(toBeUpdated).State = System.Data.Entity.EntityState.Modified;
//            le.SaveChanges();

//            return toBeUpdated;
//        }

//        [HttpDelete]
//        // DELETE: api/qualificationType/5
//        public void Delete([FromBody]qualificationType value)
//        {
//            var forDelete = le.qualificationTypes.FirstOrDefault(p => p.qualificationTypeID == value.qualificationTypeID);
//            if (forDelete != null)
//            {
//                le.qualificationTypes.Remove(forDelete);
//                le.SaveChanges();
//            }
//        }
//    }
//}
