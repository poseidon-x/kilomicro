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
    public class LoanScheduleController : ApiController
    {
        coreLoansEntities le;
        private IIDGenerator idGen;
        private IScheduleManager schMgr;
        private core_dbEntities ent;



        public LoanScheduleController()
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


        [HttpGet]
        public List<LoanVM> GetClientLoan(int id)
        {
            //get an entry with its id
            List<LoanVM> value = null;
            value = le.loans
                .Where(p => p.clientID == id && p.disbursementDate != null)
                .ToList()
                .Select(p => new LoanVM
                {
                    loanID = p.loanID,
                    Description = p.loanNo + " || " + p.amountDisbursed + " || " + p.disbursementDate.Value.ToString("dd-MMM-yyyy"),
                    repaymentSchedules = p.repaymentSchedules.ToList()
                }) 
                .ToList();
            
            return value;

        }


        [HttpGet]
        public List<repaymentSchedule> GetLoanSchedule(int id)
        {
            //get an entry with its id
            List<repaymentSchedule> value = null;
            value = le.repaymentSchedules
                .Where(p => p.loanID == id)
                .ToList();
            return value;
        }

        [HttpPost]
        public bool  ScheduleUpdate(LoanVM ln)

        {
            var loanid = 0;
            foreach (var repaymentschedule in ln.repaymentSchedules)
            {
                //Validate the input value
                //validateRequest(update);

                repaymentSchedule toBeSaved = null;
                if (repaymentschedule.repaymentScheduleID> 0)
                {
                    toBeSaved = le.repaymentSchedules
                        .FirstOrDefault(p => p.repaymentScheduleID == repaymentschedule.repaymentScheduleID);
                    loanid = toBeSaved.loanID;
                    populateFields(toBeSaved, repaymentschedule);

                        
                    

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

            //var schedules = le.repaymentSchedules.Where(p => p.loanID == loanid).ToList();
            //var totalBalance = 0.0;
            //var exbalance = 0.0;
            //foreach (var balances in schedules)
            //{
            //    exbalance = balances.principalBalance;
            //    totalBalance = totalBalance + exbalance;

            //}

            //var loan = le.loans.FirstOrDefault(p => p.loanID == loanid);
            //var thebalance = loan.balance;
            //thebalance = totalBalance;

            //try
            //{
            //    le.SaveChanges();
            //}

            //catch
            //{
            //    throw new ArgumentException("failed");
            //}


            return true ;
        }

        private void populateFields(repaymentSchedule target, repaymentSchedule source)
        {

            target.principalBalance = source.principalBalance;
            target.interestBalance = source.interestBalance;
        }

    }




}
