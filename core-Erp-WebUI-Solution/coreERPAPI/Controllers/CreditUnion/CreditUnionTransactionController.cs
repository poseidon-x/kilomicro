using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using iTextSharp.text;
using iTextSharp.text.pdf;
using coreERP;
using coreLogic;
using System.IO;
using System.Net.Http.Headers;
using System.Web;
using coreERP.Providers;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Data.Entity;

namespace coreERP.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    [AuthorizationFilter()]
    public class CreditUnionTransactionController : ApiController
    {
        CreditUnionModels le = new CreditUnionModels();

        public CreditUnionTransactionController()
        {
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        [HttpGet]
        // GET: api/Category
        public IEnumerable<creditUnionShareTransaction> Get()
        {
            return le.creditUnionShareTransactions  
                .AsNoTracking() 
                .ToList();
        }

        [HttpGet]
        // GET: api/Category
        public IEnumerable<creditUnionShareTransaction> Pending()
        {
            return le.creditUnionShareTransactions
                .Where(p=> p.posted == false)
                .OrderBy(p=> p.transactionDate)
                .AsNoTracking()
                .ToList();
        }

        [HttpPost]
        // GET: api/Category
        public creditUnionShareTransaction Approve(long id)
        {
            var tran = le.creditUnionShareTransactions
                .Include(p=> p.creditUnionMember)
                .Include(p=> p.creditUnionMember.creditUnionChapter)
                .FirstOrDefault(p => p.creditUnionShareTransactionID == id
                    && p.posted == false
                );
            using (var ent = new core_dbEntities())
            {
                IJournalExtensions je = new JournalExtensions();
                if (tran.transactionType == "C")
                {
                    var jb = je.Post("CU", tran.creditUnionMember.creditUnionChapter.vaultAccountID,
                        tran.creditUnionMember.creditUnionChapter.membersEquityAccountID,
                        tran.numberOfShares * tran.creditUnionMember.creditUnionChapter.pricePerShare,
                        "Purchase of shares", 1, tran.transactionDate, "", ent, "TO_BE_CHANGED",
                        null);
                    ent.jnl_batch.Add(jb);
                    tran.creditUnionMember.sharesBalance += tran.numberOfShares;
                }
                else if (tran.transactionType == "D")
                {
                    var jb = je.Post("CU",
                        tran.creditUnionMember.creditUnionChapter.membersEquityAccountID, 
                        tran.creditUnionMember.creditUnionChapter.vaultAccountID,
                        tran.numberOfShares * tran.creditUnionMember.creditUnionChapter.pricePerShare,
                        "Selling of shares", 1, tran.transactionDate, "", ent, "TO_BE_CHANGED",
                        null);
                    ent.jnl_batch.Add(jb);
                    tran.creditUnionMember.sharesBalance -= tran.numberOfShares;
                }
                else if (tran.transactionType == "O")
                {
                    tran.creditUnionMember.sharesBalance += tran.numberOfShares;
                }
                tran.posted = true;
                tran.postedBy = "TO_BE_CHANGED";
                le.SaveChanges();
                ent.SaveChanges();
            }
            return tran;
        }

        [HttpPost]
        // POST: api/momoProvider
        public creditUnionShareTransaction Post([FromBody]creditUnionShareTransaction value)
        {
            value.entryDate = DateTime.Now;
            value.enteredBy = "TO_BE_SET";
            var member = le.creditUnionMembers.FirstOrDefault(p => p.creditUnionMemberID == value.creditUnionMemberID);
            value.sharePrice = le.creditUnionChapters.FirstOrDefault(p => p.creditUnionChapterID == member.creditUnionChapterID).pricePerShare;
            value.posted = false;
            le.creditUnionShareTransactions.Add(value);
            le.SaveChanges();

            return value;
        }

        [HttpPut]
        // PUT: api/Category/5
        public void Put(creditUnionShareTransaction value)
        {
            var toBeUpdated = new creditUnionShareTransaction
            {
                bankID = value.bankID,
                checkNumber = value.checkNumber,
                creditUnionMemberID = value.creditUnionMemberID,
                creditUnionShareTransactionID = value.creditUnionShareTransactionID,
                modeOfPaymentID = value.modeOfPaymentID,
                numberOfShares = value.numberOfShares,
                sharePrice = value.sharePrice,
                transactionDate = value.transactionDate,
                transactionType = value.transactionType
            };
            le.Entry(toBeUpdated).State = System.Data.Entity.EntityState.Modified;
            le.SaveChanges();
        }

        [HttpDelete]
        // DELETE: api/Category/5
        public void Delete([FromBody]creditUnionShareTransaction value)
        {
            var forDelete = le.creditUnionShareTransactions.FirstOrDefault(p => p.creditUnionShareTransactionID == value.creditUnionShareTransactionID);
            if (forDelete != null)
            {
                le.creditUnionShareTransactions.Remove(forDelete);
                le.SaveChanges();
            }
        }


        [HttpGet]
        // GET: crud/MomoWallet/NewLoading
        public creditUnionShareTransaction NewTransaction()
        {
            var transaction= new creditUnionShareTransaction();
            transaction.sharePrice = le.creditUnionChapters.FirstOrDefault().pricePerShare;

            return transaction;
        }
       
    }
}
