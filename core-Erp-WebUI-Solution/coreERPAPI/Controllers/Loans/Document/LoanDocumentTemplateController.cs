using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web.Http;
using coreERP;
using coreLogic;
using coreERP.Providers;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using coreData.Constants;
using coreData.ErrorLog;

namespace coreErpApi.Controllers.Controllers.Loans.Document
{
    [AuthorizationFilter()]
    public class LoanDocumentTemplateController : ApiController
    {
        IcoreLoansEntities le;
        ErrorMessages error = new ErrorMessages();

        private string ErrorToReturn = "";


        public LoanDocumentTemplateController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public LoanDocumentTemplateController(IcoreLoansEntities lent)
        {
            le = lent;
        }

        // GET: api/
        public async Task<IEnumerable<loanDocumentTemplate>> Get()
        {
            return await le.loanDocumentTemplates
                .Include(p => p.loanDocumentTemplatePages.Select(q => q.loanDocumentTemplatePagePlaceHolders))
                .OrderBy(p => p.loanDocumentTemplateId)
                .ToListAsync();

        }

        // GET: api/
        [HttpGet]
        public loanDocumentTemplate Get(int id)
        {
            loanDocumentTemplate value = le.loanDocumentTemplates
                .Include(p => p.loanDocumentTemplatePages.Select(q => q.loanDocumentTemplatePagePlaceHolders))
                .FirstOrDefault(p => p.loanDocumentTemplateId == id);

            if (value == null)
            {
                value = new loanDocumentTemplate();
            }

            return value;
        }

        [HttpPost]
        public KendoResponse Get([FromBody]KendoRequest req)
        {
            string order = "templateName";

            KendoHelper.getSortOrder(req, ref order);
            var parameters = new List<object>();
            var whereClause = KendoHelper.getWhereClause<brand>(req, parameters);

            var query = le.loanDocumentTemplates.AsQueryable();
            if (whereClause != null && whereClause.Trim().Length > 0)
            {
                query = query.Where(whereClause, parameters.ToArray());
            }

            var data = query
                .OrderBy(order.ToString())
                .Skip(req.skip)
                .Take(req.take)
                .ToArray();

            return new KendoResponse(data, query.Count());
        }

        [HttpPost]
        public loanDocumentTemplate Post(loanDocumentTemplate input)
        {
            loanDocumentTemplate inputToBeSaved = new loanDocumentTemplate();

            inputToBeSaved.templateName = input.templateName;

            foreach (var page in input.loanDocumentTemplatePages)
            {
                loanDocumentTemplatePage pageToBeSaved = new loanDocumentTemplatePage
                {
                    pageNumber = page.pageNumber,
                    content = page.content
                };
                inputToBeSaved.loanDocumentTemplatePages.Add(pageToBeSaved);

                foreach (var placeHolder in page.loanDocumentTemplatePagePlaceHolders)
                {
                    loanDocumentTemplatePagePlaceHolder placeHolderToBeSaved = new loanDocumentTemplatePagePlaceHolder
                    {
                        placeHolderTypeId = placeHolder.placeHolderTypeId
                    };

                    pageToBeSaved.loanDocumentTemplatePagePlaceHolders.Add(placeHolderToBeSaved);
                }
            }
            input.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
            input.creationDate = DateTime.Now;
            input.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities());
            input.modified = DateTime.Now;



            le.loanDocumentTemplates.Add(input);
            try
            {
                le.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.logError(ex);
                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
            }

            return input;
        }

        [HttpPut]
        // PUT: api/
        public loanDocumentTemplate Put(loanDocumentTemplate input)
        {
            var toBeUpdated = le.loanDocumentTemplates
                .Include(p => p.loanDocumentTemplatePages.Select(q => q.loanDocumentTemplatePagePlaceHolders))
                .First(p => p.loanDocumentTemplateId == input.loanDocumentTemplateId);

            //If Template could not be found Throw Exception
            if (toBeUpdated == null)
            {
                ErrorToReturn = error.LoanDocumentInvalidDocPUT;
                Logger.logError(ErrorToReturn);
                throw new ApplicationException(ErrorToReturn);
            }

            toBeUpdated.templateName = input.templateName;
            toBeUpdated.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities());
            toBeUpdated.modified = DateTime.Now;

            //Save Changes to pages
            foreach (var page in input.loanDocumentTemplatePages)
            {
                loanDocumentTemplatePage pageToBeSaved = null;

                //If its an existing page, Update
                if (page.loanDocumentTemplatePageId > 0)
                {
                    pageToBeSaved = toBeUpdated.loanDocumentTemplatePages
                        .First(p => p.loanDocumentTemplatePageId == page.loanDocumentTemplatePageId);

                    pageToBeSaved.pageNumber = page.pageNumber;
                    pageToBeSaved.content = page.content;

                    foreach (var placeHolder in pageToBeSaved.loanDocumentTemplatePagePlaceHolders)
                    {
                        if (placeHolder.loanDocumentTemplatePageId > 0)
                        {
                            loanDocumentTemplatePagePlaceHolder placeHolderToBeSaved = null;

                            placeHolderToBeSaved = pageToBeSaved.loanDocumentTemplatePagePlaceHolders
                                .First(p => p.loanDocumentTemplatePagePlaceHolderId
                                            == placeHolder.loanDocumentTemplatePagePlaceHolderId);

                            placeHolderToBeSaved.placeHolderTypeId = placeHolder.placeHolderTypeId;
                        }
                    }
                }
                //Else its a newly added page, so Add
                else
                {
                    pageToBeSaved = new loanDocumentTemplatePage();

                    pageToBeSaved.pageNumber = page.pageNumber;
                    pageToBeSaved.content = page.content;

                    toBeUpdated.loanDocumentTemplatePages.Add(pageToBeSaved);


                    foreach (var placeHolder in page.loanDocumentTemplatePagePlaceHolders)
                    {
                        loanDocumentTemplatePagePlaceHolder placeHolderToBeSaved = new loanDocumentTemplatePagePlaceHolder();
                        placeHolderToBeSaved.placeHolderTypeId = placeHolder.placeHolderTypeId;

                        pageToBeSaved.loanDocumentTemplatePagePlaceHolders.Add(placeHolderToBeSaved);
                    }
                }
            }

            //remove all pages that have been deleted
            foreach (var page in toBeUpdated.loanDocumentTemplatePages)
            {
                var pageToDel = input.loanDocumentTemplatePages
                    .FirstOrDefault(p => p.loanDocumentTemplatePageId == page.loanDocumentTemplatePageId);

                if (pageToDel == null)
                {
                    //var pageWithPlaceHolder = page.loanDocumentTemplatePagePlaceHolders.ToList();
                    toBeUpdated.loanDocumentTemplatePages.Remove(page);
                }
            }

            try
            {
                le.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.logError(ex);
                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
            }

            return toBeUpdated;
        }


        [HttpDelete]
        // DELETE: api/
        public void DeleteDocument(int id)
        {
            var forDelete = le.loanDocumentTemplates
                .Include(p => p.loanDocumentTemplatePages)
                .Include(p => p.loanDocumentTemplatePages.Select(q => q.loanDocumentTemplatePagePlaceHolders))
                .FirstOrDefault(p => p.loanDocumentTemplateId == id);



            if (forDelete != null)
            {
                var pages = forDelete.loanDocumentTemplatePages.ToList();
                foreach (var page in pages)
                {
                    var placeHolders = page.loanDocumentTemplatePagePlaceHolders.ToList();
                    foreach (var placeholder in placeHolders)
                    {
                        le.loanDocumentTemplatePagePlaceHolders.Remove(placeholder);
                        le.SaveChanges();
                    }

                    le.loanDocumentTemplatePages.Remove(page);
                    le.SaveChanges();
                }

                le.loanDocumentTemplates.Remove(forDelete);
                le.SaveChanges();

            }
        }

        

    }
}
