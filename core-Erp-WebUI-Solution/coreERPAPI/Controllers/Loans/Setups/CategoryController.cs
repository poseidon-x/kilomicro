using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using iTextSharp.text;
using iTextSharp.text.pdf;
using coreERP;
using coreLogic;
using System.IO;
using System.Net.Http.Headers;
using System.Web;
using coreERP.Providers;
using System.Data.Entity;

namespace coreERP.Controllers
{
    [AuthorizationFilter()]
    public class CategoryController : ApiController
    {
        coreLoansEntities le = new coreLoansEntities();

        public CategoryController()
        {
            le.Configuration.LazyLoadingEnabled = false; 
            le.Configuration.ProxyCreationEnabled = false;
        }

        // GET: api/Category
        public IEnumerable<category> Get()
        {
            return le.categories 
                .OrderBy(p=> p.categoryName)
                .ToList();
        }

        // GET: api/Category/5
        [HttpGet]
        public category Get(int id)
        {
            return le.categories 
                .FirstOrDefault(p=> p.categoryID == id);
        }

        [HttpPost]
        // POST: api/Category
        public void Post([FromBody]category value)
        {
            le.categories.Add(value);
            le.SaveChanges(); 
        }

        [HttpPut]
        // PUT: api/Category/5
        public void Put([FromBody]category value)
        {
            var toBeUpdated = new category
            {
                categoryID = value.categoryID,
                categoryName = value.categoryName
            };
            le.Entry(toBeUpdated).State = System.Data.Entity.EntityState.Modified;
            le.SaveChanges();
        }

        [HttpDelete]
        // DELETE: api/Category/5
        public void Delete([FromBody]category value)
        {
            var forDelete = le.categories.FirstOrDefault(p => p.categoryID == value.categoryID);
            if (forDelete != null)
            {
                le.categories.Remove(forDelete);
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
            var cats = le.categories 
                .Include("categoryCheckLists")
                .OrderBy(p => p.categoryName)
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
            var header = new Paragraph("Client Categories", titleFont);
           
            var numOfColumns = 2;
            var dataTable = new PdfPTable(numOfColumns);
             
            dataTable.DefaultCell.Padding = 3;
            dataTable.SetWidths(new [] {3, 12});
            dataTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT; 
            dataTable.DefaultCell.Border = 0;

            var cell = new PdfPCell(header);
            cell.Border = 0;
            cell.BorderWidthBottom = 1;
            cell.Colspan = 2;
            dataTable.AddCell(cell);

            cell = new PdfPCell(new Phrase("              ", titleFont));
            cell.Border = 0; 
            cell.Colspan = 2;
            dataTable.AddCell(cell);

            // Adding headers
            cell = new PdfPCell(new Phrase("Category ID", headerFont));
            cell.BackgroundColor = BaseColor.BLACK;
            cell.Border = 0;
            cell.BorderWidthBottom = 1;
            dataTable.AddCell(cell);
            cell = new PdfPCell(new Phrase("Category Name", headerFont));
            cell.BackgroundColor = BaseColor.BLACK;
            cell.Border = 0;
            cell.BorderWidthBottom = 1;
            dataTable.AddCell(cell); 
            dataTable.HeaderRows = 1;

            foreach (var cat in cats)
            {
                cell = new PdfPCell(new Phrase(cat.categoryID.ToString(), bodyFont));
                cell.Border = 0;
                cell.BorderWidthBottom = 1;
                dataTable.AddCell(cell);
                cell = new PdfPCell(new Phrase(cat.categoryName, bodyFont));
                cell.Border = 0;
                cell.BorderWidthBottom = 1;
                dataTable.AddCell(cell);

                if (cat.categoryCheckLists.Count > 0)
                {
                    var detailTable = new PdfPTable(3);

                    detailTable.DefaultCell.Padding = 3;
                    detailTable.SetWidths(new[] { 1, 6, 3 });
                    detailTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    detailTable.DefaultCell.Border = 0;
                    //Inner Title

                    header = new Paragraph("Check list items", innerTitleFont);
                    cell = new PdfPCell(header);
                    cell.Colspan = 3;
                    cell.Border = 0;
                    cell.BorderWidthBottom = 1;
                    detailTable.AddCell(cell);

                    // Adding headers
                    detailTable.AddCell("");
                    cell = new PdfPCell(new Phrase("Description", headerFont));
                    cell.BackgroundColor = BaseColor.BLACK;
                    cell.Border = 0;
                    cell.BorderWidthBottom = 1;
                    detailTable.AddCell(cell);
                    cell = new PdfPCell(new Phrase("Mandatory?", headerFont));
                    cell.BackgroundColor = BaseColor.BLACK;
                    cell.Border = 0;
                    cell.BorderWidthBottom = 1;
                    detailTable.AddCell(cell);

                    detailTable.HeaderRows = 1;

                    foreach (var det in cat.categoryCheckLists)
                    {
                        detailTable.AddCell("");
                        cell = new PdfPCell(new Phrase(det.description, bodyFont));
                        cell.Border = 0;                        
                        cell.BorderWidthBottom = 1;
                        detailTable.AddCell(cell);
                        cell = new PdfPCell(new Phrase(det.isMandatory ? "✔ Yes"
                            : "\u274C No", bodyFont));
                        cell.Border = 0;
                        cell.BorderWidthBottom = 1;
                        detailTable.AddCell(cell);
                    }

                    PdfPCell nesthousing = new PdfPCell(detailTable);
                    nesthousing.Padding = 0f;
                    nesthousing.Colspan = 2;
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
                FileName = "categories.pdf"
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            
            return response; 
        }

    }
}
