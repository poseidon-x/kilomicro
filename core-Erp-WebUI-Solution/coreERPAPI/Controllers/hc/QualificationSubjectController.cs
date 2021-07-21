//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web.Http;
//using coreLogic;
//using coreERP.Providers;

//namespace coreERP.Controllers.hc
//{
//    [AuthorizationFilter()]
//    public class QualificationSubjectController : ApiController
//    {
//        coreLoansEntities le = new coreLoansEntities();

//        public QualificationSubjectController()
//        {
//            le.Configuration.LazyLoadingEnabled = false;
//            le.Configuration.ProxyCreationEnabled = false;
//        }

//        // GET: api/qualificationSubject
//        public IEnumerable<qualificationSubject> Get()
//        {
//            return le.qualificationSubjects
//                .OrderBy(p => p.qualificationSubjectID)
//                .ToList();
//        }

//        // GET: api/qualificationSubject/5
//        [HttpGet]
//        public qualificationSubject Get(int id)
//        {
//            return le.qualificationSubjects
//                .FirstOrDefault(p => p.qualificationSubjectID == id);
//        }

//        [HttpPost]
//        // POST: api/qualificationSubject
//        public qualificationSubject Post([FromBody]qualificationSubject value)
//        {
//            le.qualificationSubjects.Add(value);
//            le.SaveChanges();

//            return value;
//        }

//        [HttpPut]
//        // PUT: api/qualificationSubject/5
//        public qualificationSubject Put([FromBody]qualificationSubject value)
//        {
//            var toBeUpdated = new qualificationSubject
//            {
//                qualificationSubjectID = value.qualificationSubjectID,
//                qualificationSubjectName = value.qualificationSubjectName,
//            };
//            le.Entry(toBeUpdated).State = System.Data.Entity.EntityState.Modified;
//            le.SaveChanges();

//            return toBeUpdated;
//        }

//        [HttpDelete]
//        // DELETE: api/qualificationSubject/5
//        public void Delete([FromBody]qualificationSubject value)
//        {
//            var forDelete = le.qualificationSubjects.FirstOrDefault(p => p.qualificationSubjectID == value.qualificationSubjectID);
//            if (forDelete != null)
//            {
//                le.qualificationSubjects.Remove(forDelete);
//                le.SaveChanges();
//            }
//        }
//    }
//}
