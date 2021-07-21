using System;
using System.Collections.Generic;
using System.Linq;/*
using System.Net;
using System.Net.Http; 
using iTextSharp.text;
using iTextSharp.text.pdf;
using coreERP;*/
using coreLogic;
using System.IO;
using System.Net.Http.Headers;
using System.Web;
using coreERP.Providers;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Data.Entity;
using coreData.Constants;
using coreErp.Models.Loan;
using coreERP.Models.Loan;
using coreErpApi.Controllers.Models;
using coreErpApi.Models;
using coreErpApi.Models.Loan;


namespace coreERP.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    [AuthorizationFilter()]
    public class ControllerFileMonthlyDed : ApiController
    {
        coreLoansEntities le;
        private IIDGenerator idGen;
        private IScheduleManager schMgr;
        private core_dbEntities ent;

        

        public ControllerFileMonthlyDed()
        {
            ent = new core_dbEntities();
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
            ent.Configuration.LazyLoadingEnabled = false;
            ent.Configuration.ProxyCreationEnabled = false;

            idGen = new IDGenerator();
            schMgr = new ScheduleManager();
        }


        //[HttpGet]
        //public List<controllerRepaymentType> GetContRepaymentType()
        //{
        //    //get an entry with its id
        //    List<controllerRepaymentType> value = null;
        //    value = le.controllerRepaymentTypes.ToList();
        //    return value;

        //}


        [HttpGet]
        public List<controllerRemark> GetRemarks()
        {
            //get an entry with its id
            List<controllerRemark> value = null;
            value = le.controllerRemarks.ToList();
            return value;

        }

        [HttpGet]
        public List<loanNo_by_staffID> GetLoanNo(int id)
        {
            //get an entry with its id
            List<loanNo_by_staffID> value = null;
            value = le.loanNo_by_staffID
                .Where(p=>p.fileID==id && p.balance>=1)
                .ToList();
            return value;
        }

        [HttpGet]
        public List<ControllerFileVM> GetControllerFile()
        {
            //get an entry with its id
            List<ControllerFileVM> value = null;
            value = le.controllerFiles
                .Where(p=>!p.controllerFileDetails.Any(r=>r.authorized))
                .ToList()
                .Select(p => new ControllerFileVM
                {
                    fileID = p.fileID,
                    fileName = p.fileMonth + " || " + p.fileName,
                    controllerFileDetails = p.controllerFileDetails.ToList()
                }) 
                .ToList();
            
            return value;

        }



        [HttpGet]
        public List<vw_controllerFile_outstandingLoan> GetControllerFileOutstanding(int id)
        {
            //get an entry with its id
            List<vw_controllerFile_outstandingLoan> value = null;
            value = le.vw_controllerFile_outstandingLoan
                .Where(p => p.fileID == id)
                .ToList();
            return value;
        }

      

        [HttpPost]
        public bool ControllerFileOutstandingUpdate(ControllerFileMonthDedVM ln)

        {
            

            foreach (var controllerFileDetail in ln.controllerFileDetails)
            {
                //Validate the input value
                //validateRequest(update);

                vw_controllerFile_outstandingLoan toBeSaved = null;
                if (controllerFileDetail.fileDetailID> 0)
                {
                    toBeSaved = le.vw_controllerFile_outstandingLoan
                        .FirstOrDefault(p => p.fileDetailID == controllerFileDetail.fileDetailID);
                    populateFields(toBeSaved, controllerFileDetail);
                    

                }
                else
                {
                    throw new ArgumentException("Update Failed");

                }

                
                {
                    le.SaveChanges();
                  
                }


                // return toBeSaved;
            }
            return true ;
        }

        private void populateFields(vw_controllerFile_outstandingLoan target, vw_controllerFile_outstandingLoan source)
        {
            
                target.monthlyDeduction = source.monthlyDeduction;
            
        }

    }
}
