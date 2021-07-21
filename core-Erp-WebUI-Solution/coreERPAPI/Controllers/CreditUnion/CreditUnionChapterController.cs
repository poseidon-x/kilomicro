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
    public class CreditUnionChapterController : ApiController
    {
        CreditUnionModels le = new CreditUnionModels();

        public CreditUnionChapterController()
        {
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        // GET: api/Category
        public IEnumerable<creditUnionChapter> Get()
        {
            return le.creditUnionChapters  
                .AsNoTracking() 
                .ToList();
        }
         
        [HttpPost]
        // POST: api/momoProvider
        public creditUnionChapter Post(creditUnionChapter value)
        {
            try
            {
                le.creditUnionChapters.Add(value);
                le.SaveChanges();

                return value;
            }
            catch (Exception x)
            {
                if (x.InnerException != null) x = x.InnerException;
                if (x.InnerException != null) x = x.InnerException;
                if (x.InnerException != null) x = x.InnerException;
                throw new ApplicationException(value.dateFormed.ToString());
            }
        }

        [HttpPut]
        // PUT: api/Category/5
        public void Put(creditUnionChapter value)
        {
            var toBeUpdated = new creditUnionChapter
            {
                creditUnionChapterID = value.creditUnionChapterID,
                chapterName = value.chapterName,
                dateFormed = value.dateFormed,
                docRegistrationNumber = value.docRegistrationNumber,
                dividendsExpenseAccountID = value.dividendsExpenseAccountID,
                membersEquityAccountID = value.membersEquityAccountID,
                vaultAccountID = value.vaultAccountID,
                emailAddress = value.emailAddress,
                postalAddress = value.postalAddress,
                pricePerShare = value.pricePerShare,
                telePhoneNumber = value.telePhoneNumber,
                town = value.town
            };
            le.Entry(toBeUpdated).State = System.Data.Entity.EntityState.Modified;
            le.SaveChanges();
        }

        [HttpDelete]
        // DELETE: api/Category/5
        public void Delete([FromBody]creditUnionChapter value)
        {
            var forDelete = le.creditUnionChapters.FirstOrDefault(p => p.creditUnionChapterID == value.creditUnionChapterID);
            if (forDelete != null)
            {
                le.creditUnionChapters.Remove(forDelete);
                le.SaveChanges();
            }
        }


        [HttpGet]
        // GET: crud/MomoWallet/NewLoading
        public creditUnionChapter NewChapter()
        {
            return new creditUnionChapter();
        }
       
        [HttpGet]
        // GET: crud/MomoWallet/WalletLookup
        public IEnumerable<Models.LookupEntry> ChapterLookUp()
        {
            var wallets =  le.creditUnionChapters 
                .AsNoTracking() 
                .ToList();
            var lookups = new List<Models.LookupEntry>();

            foreach (var wallet in wallets)
            {
                lookups.Add(new Models.LookupEntry
                {
                    ID = wallet.creditUnionChapterID,
                    Description = wallet.chapterName
                });
            }

            return lookups;
        }
    }
}
