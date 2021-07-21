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
    public class CreditUnionMemberController : ApiController
    {
        CreditUnionModels le = new CreditUnionModels();

        public CreditUnionMemberController()
        {
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        // GET: api/Category
        public IEnumerable<creditUnionMember> Get()
        {
            return le.creditUnionMembers  
                .Include(p=> p.creditUnionChapter)
                .AsNoTracking() 
                .ToList();
        }

        // GET: api/Category
        public creditUnionMember Get(int id)
        {
            return le.creditUnionMembers
                .Where(p=> p.creditUnionMemberID == id)
                .Include(p => p.creditUnionChapter)
                .AsNoTracking()
                .FirstOrDefault();
        }
        
        [HttpPost]
        // POST: api/momoProvider
        public creditUnionMember Post([FromBody]creditUnionMember value)
        {
            value.sharesBalance = 0.0; 
            le.creditUnionMembers.Add(value);
            le.SaveChanges();

            return value;
        }

        [HttpPut]
        // PUT: api/Category/5
        public void Put([FromBody]creditUnionMember value)
        {
            var toBeUpdated = new creditUnionMember
            {
                clientID = value.clientID,
                creditUnionChapterID = value.creditUnionChapterID,
                joinedDate = value.joinedDate,
                sharesBalance = value.sharesBalance,
                creditUnionMemberID = value.creditUnionMemberID
            };
            le.Entry(toBeUpdated).State = System.Data.Entity.EntityState.Modified;
            le.SaveChanges();
        }

        [HttpDelete]
        // DELETE: api/Category/5
        public void Delete([FromBody]creditUnionMember value)
        {
            var forDelete = le.creditUnionMembers.FirstOrDefault(p => p.creditUnionMemberID == value.creditUnionMemberID);
            if (forDelete != null)
            {
                le.creditUnionMembers.Remove(forDelete);
                le.SaveChanges();
            }
        }


        [HttpGet]
        // GET: crud/MomoWallet/NewLoading
        public walletLoading NewLoading()
        {
            return new walletLoading();
        }
       
        [HttpGet]
        // GET: crud/MomoWallet/WalletLookup
        public IEnumerable<Models.LookupEntry> MemberLookUp()
        {
            var wallets =  le.creditUnionMembers 
                .AsNoTracking() 
                .ToList();
            var lookups = new List<Models.LookupEntry>();

            using (var lent = new coreLogic.coreLoansEntities())
            {
                foreach (var wallet in wallets)
                {
                    var client = lent.clients.FirstOrDefault(p=> p.clientID == wallet.clientID);
                    if (client != null)
                    {
                        lookups.Add(new Models.LookupEntry
                        {
                            ID = wallet.creditUnionMemberID,
                            Description = client.surName + ", " + client.otherNames
                        });
                    }
                }
            }

            return lookups;
        }
    }
}
