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
    public class MomoProviderController : ApiController
    {
        momoModelsConnectionString le = new momoModelsConnectionString();

        public MomoProviderController()
        {
            le.Configuration.LazyLoadingEnabled = false; 
            le.Configuration.ProxyCreationEnabled = false;
        }

        // GET: api/Category
        public IEnumerable<momoProvider> Get()
        {
            return le.momoProviders
                .AsNoTracking()
                .OrderBy(p=> p.providerFullName)
                .ToList();
        }

        // GET: api/Category/5
        [HttpGet]
        public momoProvider Get(int id)
        {
            return le.momoProviders
                .AsNoTracking()
                .FirstOrDefault(p=> p.providerID == id);
        }

        [HttpPost]
        // POST: api/momoProvider
        public momoProvider Post([FromBody]momoProvider value)
        {
            le.momoProviders.Add(value);
            le.SaveChanges();

            return value;
        }

        [HttpPut]
        // PUT: api/Category/5
        public void Put([FromBody]momoProvider value)
        {
            var toBeUpdated = new momoProvider
            {
                providerID = value.providerID,
                providerFullName = value.providerFullName,
                providerShortName = value.providerShortName,
                momoProductName = value.momoProductName
            };
            le.Entry(toBeUpdated).State = System.Data.Entity.EntityState.Modified;
            le.SaveChanges();
        }

        [HttpDelete]
        // DELETE: api/Category/5
        public void Delete([FromBody]momoProvider value)
        {
            var forDelete = le.momoProviders.FirstOrDefault(p => p.providerID == value.providerID);
            if (forDelete != null)
            {
                le.momoProviders.Remove(forDelete);
                le.SaveChanges();
            }
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
            var cats = le.momoProviders
                .Include(p=> p.momoServices)
                .Include(p=> p.momoServices.Select(q=> q.momoServiceCharges))
                .OrderBy(p => p.providerFullName)
                .ToList();
            //step 2: we create a memory stream that listens to the document
            var output = new MemoryStream();
            // step 1: creation of a document-object
            var document = new Document(PageSize.A4, 20, 20, 38, 38);
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
            document.Add(new Paragraph("  ",  titleFont));
            var header = new Paragraph("Mobile Money Providers", titleFont);
           
            var numOfColumns = 3;
            var dataTable = new PdfPTable(numOfColumns);
             
            dataTable.DefaultCell.Padding = 3;
            dataTable.SetWidths(new [] {4, 4, 4});
            dataTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT; 
            dataTable.DefaultCell.Border = 0;

            var cell = new PdfPCell(header);
            cell.Border = 0;
            cell.BorderWidthBottom = 1;
            cell.Colspan = 3;
            dataTable.AddCell(cell);

            cell = new PdfPCell(new Phrase("              ", titleFont));
            cell.Border = 0; 
            cell.Colspan = 3;
            dataTable.AddCell(cell);

            // Adding headers
            cell = new PdfPCell(new Phrase("Provider Name", headerFont));
            cell.BackgroundColor = BaseColor.BLACK;
            cell.Border = 0;
            cell.BorderWidthBottom = 1;
            dataTable.AddCell(cell);
            cell = new PdfPCell(new Phrase("Company Name", headerFont));
            cell.BackgroundColor = BaseColor.BLACK;
            cell.Border = 0;
            cell.BorderWidthBottom = 1;
            dataTable.AddCell(cell);
            cell = new PdfPCell(new Phrase("Product Name", headerFont));
            cell.BackgroundColor = BaseColor.BLACK;
            cell.Border = 0;
            cell.BorderWidthBottom = 1;
            dataTable.AddCell(cell); 
            dataTable.HeaderRows = 1;

            foreach (var cat in cats)
            {
                cell = new PdfPCell(new Phrase(cat.providerShortName, bodyFont));
                cell.Border = 0;
                cell.BorderWidthBottom = 1;
                dataTable.AddCell(cell);
                cell = new PdfPCell(new Phrase(cat.providerFullName, bodyFont));
                cell.Border = 0;
                cell.BorderWidthBottom = 1;
                dataTable.AddCell(cell);
                cell = new PdfPCell(new Phrase(cat.momoProductName, bodyFont));
                cell.Border = 0;
                cell.BorderWidthBottom = 1;
                dataTable.AddCell(cell);

                if (cat.momoServices.Count > 0)
                {
                    var detailTable = new PdfPTable(2);

                    detailTable.DefaultCell.Padding = 3;
                    detailTable.SetWidths(new[] { 1, 9 });
                    detailTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    detailTable.DefaultCell.Border = 0;
                    //Inner Title

                    header = new Paragraph("Provider Services", innerTitleFont);
                    cell = new PdfPCell(header);
                    cell.Colspan = 2;
                    cell.Border = 0;
                    cell.BorderWidthBottom = 1;
                    detailTable.AddCell(cell);

                    // Adding headers
                    detailTable.AddCell("");
                    cell = new PdfPCell(new Phrase("Service Name", headerFont));
                    cell.BackgroundColor = BaseColor.BLACK;
                    cell.Border = 0;
                    cell.BorderWidthBottom = 1;
                    detailTable.AddCell(cell); 

                    detailTable.HeaderRows = 1;

                    foreach (var det in cat.momoServices)
                    {
                        detailTable.AddCell("");
                        cell = new PdfPCell(new Phrase(det.serviceName, bodyFont));
                        cell.Border = 0;                        
                        cell.BorderWidthBottom = 1;
                        detailTable.AddCell(cell);

                        if (det.momoServiceCharges.Count > 0)
                        {
                            var detailTable2 = new PdfPTable(5);

                            detailTable2.DefaultCell.Padding = 3;
                            detailTable2.SetWidths(new[] { 4, 3, 3, 3, 3 });
                            detailTable2.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            detailTable2.DefaultCell.Border = 0;
                            //Inner Title

                            header = new Paragraph("Service Charges", innerTitleFont);
                            cell = new PdfPCell(header);
                            cell.Colspan = 5;
                            cell.Border = 0;
                            cell.BorderWidthBottom = 1;
                            detailTable2.AddCell(cell);

                            // Adding headers
                            detailTable2.AddCell("");
                            cell = new PdfPCell(new Phrase("Min. Tran", headerFont));
                            cell.BackgroundColor = BaseColor.BLACK;
                            cell.Border = 0;
                            cell.BorderWidthBottom = 1;
                            detailTable2.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Max. Tran", headerFont));
                            cell.BackgroundColor = BaseColor.BLACK;
                            cell.Border = 0;
                            cell.BorderWidthBottom = 1;
                            detailTable2.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Charge Valie", headerFont));
                            cell.BackgroundColor = BaseColor.BLACK;
                            cell.Border = 0;
                            cell.BorderWidthBottom = 1;
                            detailTable2.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Percentage", headerFont));
                            cell.BackgroundColor = BaseColor.BLACK;
                            cell.Border = 0;
                            cell.BorderWidthBottom = 1;
                            detailTable2.AddCell(cell);

                            detailTable2.HeaderRows = 1;

                            foreach (var det2 in det.momoServiceCharges)
                            {
                                detailTable2.AddCell("");
                                cell = new PdfPCell(new Phrase(det2.minTranAmount.ToString("#,##0.#0"), bodyFont));
                                cell.Border = 0;
                                cell.BorderWidthBottom = 1;
                                detailTable2.AddCell(cell);
                                cell = new PdfPCell(new Phrase(det2.maxTranAmount.ToString("#,##0.#0"), bodyFont));
                                cell.Border = 0;
                                cell.BorderWidthBottom = 1;
                                detailTable2.AddCell(cell);
                                cell = new PdfPCell(new Phrase(det2.chargesValue.ToString("#,##0.#0"), bodyFont));
                                cell.Border = 0;
                                cell.BorderWidthBottom = 1;
                                detailTable2.AddCell(cell);
                                cell = new PdfPCell(new Phrase(det2.isPercent ? "✔ Yes"
                                    : "\u274C No", bodyFont));
                                cell.Border = 0;
                                cell.BorderWidthBottom = 1;
                                detailTable2.AddCell(cell);
                            }

                            PdfPCell nesthousing2 = new PdfPCell(detailTable2);
                            nesthousing2.Padding = 0f;
                            nesthousing2.Colspan = 2;
                            nesthousing2.Border = 0;

                            detailTable.AddCell(nesthousing2);
                        }
                    }

                    PdfPCell nesthousing = new PdfPCell(detailTable);
                    nesthousing.Padding = 0f;
                    nesthousing.Colspan = 3;
                    nesthousing.Border = 0;

                    dataTable.AddCell(nesthousing);
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
                FileName = "momo_providers.pdf"
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            
            return response; 
        }

    }
}
