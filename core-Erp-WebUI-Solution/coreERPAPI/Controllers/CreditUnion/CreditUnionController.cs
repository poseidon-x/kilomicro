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
    public class CreditUnionController : ApiController
    {
        momoModelsConnectionString le = new momoModelsConnectionString();

        public CreditUnionController()
        {
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        // GET: api/Category
        public IEnumerable<momoWallet> Get()
        {
            return le.momoWallets
                .Include(p=> p.momoProvider) 
                .AsNoTracking()
                .OrderBy(p => p.accountNumber)
                .ToList();
        }

        // GET: api/Category/5
        [HttpGet]
        public momoWallet Get(int id)
        {
            return le.momoWallets
                .AsNoTracking()
                .FirstOrDefault(p => p.walletID == id);
        }

        [HttpPost]
        // POST: api/momoProvider
        public momoWallet Post([FromBody]momoWallet value)
        {
            value.entryDate = DateTime.Now;
            value.enteredBy = "TO_BE_SET";
            le.momoWallets.Add(value);
            le.SaveChanges();

            return value;
        }

        [HttpPut]
        // PUT: api/Category/5
        public void Put([FromBody]momoWallet value)
        {
            var toBeUpdated = new momoWallet
            {
                providerID = value.providerID,
                walletAccountID = value.walletAccountID,
                accountNumber = value.accountNumber,
                chargesExpenseAccountID = value.chargesExpenseAccountID,
                chargesIncomeAccountID = value.chargesIncomeAccountID,
                cashiersTillID = value.cashiersTillID,
                walletID = value.walletID,
                enteredBy = value.enteredBy,
                entryDate = value.entryDate,
                balance = value.balance
            };
            le.Entry(toBeUpdated).State = System.Data.Entity.EntityState.Modified;
            le.SaveChanges();
        }

        [HttpDelete]
        // DELETE: api/Category/5
        public void Delete([FromBody]momoWallet value)
        {
            var forDelete = le.momoWallets.FirstOrDefault(p => p.walletID == value.walletID);
            if (forDelete != null)
            {
                le.momoWallets.Remove(forDelete);
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
        public IEnumerable<Models.LookupEntry> WalletLookup()
        {
            var wallets =  le.momoWallets
                .Include(p => p.momoProvider)
                .AsNoTracking()
                .OrderBy(p => p.accountNumber)
                .ToList();
            var lookups = new List<Models.LookupEntry>();

            using (var sec = new coreLogic.coreSecurityEntities())
            {
                using (var lent = new coreLogic.coreLoansEntities())
                {
                    foreach (var wallet in wallets)
                    {
                        lookups.Add(new Models.LookupEntry
                        {
                            ID = wallet.walletID,
                            Description = wallet.momoProvider.momoProductName + " - " 
                                + wallet.accountNumber + " - "
                                + getUserName(sec, lent, wallet.cashiersTillID)
                        });
                    }
                }
            }

            return lookups;
        }

        [HttpGet]
        // GET: api/Category
        public HttpResponseMessage Pdf(string token)
        {
            return Pdf();
        }

        [HttpGet]
        // GET: api/Category
        public HttpResponseMessage Pdf()
        {
            var cats = le.momoWallets
                .Include(p => p.momoProvider) 
                .ToList();
            //step 2: we create a memory stream that listens to the document
            var output = new MemoryStream();
            // step 1: creation of a document-object
            var document = new Document(PageSize.A4.Rotate(), 20, 20, 38, 38);
            PdfWriter.GetInstance(document, output);

            //step 3: we open the document
            document.Open();

            var titleFont = FontFactory.GetFont("Arial", 12, Font.BOLD | Font.UNDERLINE,
                BaseColor.BLACK);
            var innerTitleFont = FontFactory.GetFont("Arial", 12, Font.BOLD,
                BaseColor.BLACK);
            var headerFont = FontFactory.GetFont("Arial", 10, Font.BOLD,
                BaseColor.WHITE);
            var bodyFont = FontFactory.GetFont("Arial", 10, Font.NORMAL,
                BaseColor.BLACK);

            //step 4: we add content to the document
            var logo = iTextSharp.text.Image.GetInstance(
                HttpContext.Current.Server.MapPath("~/Content/Images/logo.png"));
            logo.ScaleAbsoluteHeight(30);
            logo.Right = document.Right - document.RightMargin;
            document.Add(logo);
            document.Add(new Paragraph("  ", titleFont));
            var header = new Paragraph("Mobile Money Wallets", titleFont);

            var numOfColumns = 7;
            var dataTable = new PdfPTable(numOfColumns);

            dataTable.DefaultCell.Padding = 3;
            dataTable.SetWidths(new[] { 1, 2, 2, 2, 2, 2, 2 });
            dataTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
            dataTable.DefaultCell.Border = 0;

            var cell = new PdfPCell(header);
            cell.Border = 0;
            cell.BorderWidthBottom = 1;
            cell.Colspan = numOfColumns;
            dataTable.AddCell(cell);

            cell = new PdfPCell(new Phrase("              ", titleFont));
            cell.Border = numOfColumns;
            cell.Colspan = 3;
            dataTable.AddCell(cell);

            // Adding headers
            cell = new PdfPCell(new Phrase("Account Number", headerFont));
            cell.BackgroundColor = BaseColor.BLACK;
            cell.Border = 0;
            cell.BorderWidthBottom = 1;
            dataTable.AddCell(cell);
            cell = new PdfPCell(new Phrase("Cashier's Name", headerFont));
            cell.BackgroundColor = BaseColor.BLACK;
            cell.Border = 0;
            cell.BorderWidthBottom = 1;
            dataTable.AddCell(cell);
            cell = new PdfPCell(new Phrase("Provider", headerFont));
            cell.BackgroundColor = BaseColor.BLACK;
            cell.Border = 0;
            cell.BorderWidthBottom = 1;
            dataTable.AddCell(cell);
            cell = new PdfPCell(new Phrase("Wallet Account", headerFont));
            cell.BackgroundColor = BaseColor.BLACK;
            cell.Border = 0;
            cell.BorderWidthBottom = 1;
            dataTable.AddCell(cell);
            cell = new PdfPCell(new Phrase("Income Account", headerFont));
            cell.BackgroundColor = BaseColor.BLACK;
            cell.Border = 0;
            cell.BorderWidthBottom = 1;
            dataTable.AddCell(cell);
            cell = new PdfPCell(new Phrase("Expense Account", headerFont));
            cell.BackgroundColor = BaseColor.BLACK;
            cell.Border = 0;
            cell.BorderWidthBottom = 1;
            dataTable.AddCell(cell);
            cell = new PdfPCell(new Phrase("Balance", headerFont));
            cell.BackgroundColor = BaseColor.BLACK;
            cell.Border = 0;
            cell.BorderWidthBottom = 1;
            dataTable.AddCell(cell);
            dataTable.HeaderRows = 1;

            using (var ent = new coreSecurityEntities())
            {
                using (var lent = new coreLoansEntities())
                {
                    using (var cent = new core_dbEntities())
                    {
                        foreach (var cat in cats)
                        {
                            cell = new PdfPCell(new Phrase(cat.accountNumber, bodyFont));
                            cell.Border = 0;
                            cell.BorderWidthBottom = 1;
                            dataTable.AddCell(cell);
                            var userName = getUserName(ent, lent, cat.cashiersTillID);
                            cell = new PdfPCell(new Phrase(userName, bodyFont));
                            cell.Border = 0;
                            cell.BorderWidthBottom = 1;
                            dataTable.AddCell(cell);
                            cell = new PdfPCell(new Phrase(cat.momoProvider.momoProductName, bodyFont));
                            cell.Border = 0;
                            cell.BorderWidthBottom = 1;
                            dataTable.AddCell(cell);
                            var accountName = getAccountName(cent, cat.walletAccountID);
                            cell = new PdfPCell(new Phrase(accountName, bodyFont));
                            cell.Border = 0;
                            cell.BorderWidthBottom = 1;
                            dataTable.AddCell(cell);
                            accountName = getAccountName(cent, cat.chargesIncomeAccountID);
                            cell = new PdfPCell(new Phrase(accountName, bodyFont));
                            cell.Border = 0;
                            cell.BorderWidthBottom = 1;
                            dataTable.AddCell(cell);
                            accountName = getAccountName(cent, cat.chargesExpenseAccountID);
                            cell = new PdfPCell(new Phrase(accountName, bodyFont));
                            cell.Border = 0;
                            cell.BorderWidthBottom = 1;
                            dataTable.AddCell(cell);
                            cell = new PdfPCell(new Phrase(cat.balance.ToString("#,##0.#0"), bodyFont));
                            cell.Border = 0;
                            cell.BorderWidthBottom = 1;
                            dataTable.AddCell(cell);
                        }
                    }
                }
            }
            // Add table to the document
            document.Add(dataTable);
            //This is important don't forget to close the document
            document.Close();

            // send the memory stream as File
            HttpResponseMessage response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.OK;
            response.Content = new ByteArrayContent(output.ToArray());
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline")
            {
                FileName = "momo_wallets.pdf"
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

            return response;
        }

        private string getUserName(coreSecurityEntities ent, coreLoansEntities lent, int cashierTillID)
        {
            var userName = "";
            var till = lent.cashiersTills.FirstOrDefault(p => p.cashiersTillID == cashierTillID);
            if (till != null)
            {
                userName = till.userName;
                var user = ent.users.FirstOrDefault(p => p.user_name == till.userName);
                if (user != null)
                {
                    userName = user.full_name;
                }
            }

            return userName;
        }

        private string getAccountName(core_dbEntities ent, int accountID)
        {
            var accountName = "";
            var acc = ent.accts.FirstOrDefault(p => p.acct_id == accountID);
            if (acc != null)
            {
                accountName = acc.acc_name; 
            }

            return accountName;
        }

    }
}
