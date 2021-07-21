using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using coreData.Constants;
using coreLogic;
using coreLogic.HelperClasses;
using Excel.Core;
using log4net.Repository.Hierarchy;
using coreData.ErrorLog;
using coreERP.Models.LoanRepaymentUpload;
using coreLogic.HelperClasses;

namespace coreERP.Controllers.Loans
{
    public class FileUploadController : Controller
    {
        private readonly string errorUploadingFile = "An error ocuured, File could not save.";

        private coreLoansEntities le;

        public FileUploadController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public ActionResult SaveLoanRepaymentList(IEnumerable<HttpPostedFileBase> repaymentFile)
        {
            var file = repaymentFile.FirstOrDefault();
            var fileName = Path.GetFileName(file.FileName);
            var destinationPath = Path.Combine(Server.MapPath("~/App_Data/Upload/StaffLoanRepayment"), fileName);
            if (System.IO.File.Exists(destinationPath))
            {
                System.IO.File.Delete(destinationPath);
            }
            try
            {
                file.SaveAs(destinationPath);
            }
            catch (IOException)
            {
                throw new ApplicationException(errorUploadingFile);
            }
            return Json(fileName);
        }

        public ActionResult RemoveLoanRepaymentList(string[] fileNames)
        {
            foreach (var fullName in fileNames)
            {
                var fileName = Path.GetFileName(fullName);
                var physicalPath = Path.Combine(Server.MapPath("~/App_Data/Upload/StaffLoanRepayment"), fileName);
                if (System.IO.File.Exists(physicalPath))
                {
                    System.IO.File.Delete(physicalPath);
                }
            }
            return Content("");
        }

        [HttpPost]
        public ActionResult SaveUploadedLoanRepaymentList(FileModel input)
        {
            var targetPath = Path.Combine( // file is in starguest API
                Server.MapPath("~/App_Data/Upload/StaffLoanRepayment"), input.fileName);
            ExcelFileProcessor ex = new ExcelFileProcessor(targetPath);

            controllerFile fileToBeSaved = new controllerFile
            {
                fileName = input.fileName,
                fileMonth = Convert.ToInt32(DateTime.Today.ToString("yyyyMM")),
                uploadDate = DateTime.Now,
                controllerFileDetails = new List<controllerFileDetail>()
            };

            List<controllerFileDetail> listToSave = ex.getRepaymentResults();
            //aset toBeSaved = new aset();
            int saved = 0;
            if (listToSave.Any())
            {
                foreach (var fileDetail in listToSave)
                {
                    fileToBeSaved.controllerFileDetails.Add(fileDetail);
                }
                ControllerFileProcessor.processFile(fileToBeSaved, le, fileToBeSaved.uploadDate);
                le.controllerFiles.Add(fileToBeSaved);
            }
            

            try
            {
                le.SaveChanges();
            }
            catch (Exception x)
            {
                coreData.ErrorLog.Logger.logError(x);
                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
            }
            removeAssetFile(input.fileName); //delete file once done.
            return Json(saved);
        }

        private void removeAssetFile(string fileName)
        {
            var physicalPath = Path.Combine(Server.MapPath("~/App_Data/Uploads/StaffList"), fileName);
            if (System.IO.File.Exists(physicalPath))
            {
                System.IO.File.Delete(physicalPath);
            }
        }



    }
}
