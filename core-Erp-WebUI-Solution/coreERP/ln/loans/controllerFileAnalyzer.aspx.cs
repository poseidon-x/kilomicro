using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using coreLogic;
using coreLogic.Models.Loans;
using Microsoft.Office.Interop.Excel;
using Telerik.Web.UI;
using Page = System.Web.UI.Page;

namespace coreERP.ln.loans
{
    public partial class controllerFileAnalyzer : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }


        protected void btnNext1_Click(object sender, EventArgs e)
        {
            var loanNum = "";

            if (upload.UploadedFiles.Count == 1)
            {
                Stream s = upload.UploadedFiles[0].InputStream;
                StreamReader sr = new StreamReader(s);
                var line = "";
                var started = false;
                int i = 0;
                List<ControllerOutViewModel> FileDetails = new List<ControllerOutViewModel>();
                string month;
                controllerFile file = new controllerFile
                {
                    fileName = upload.UploadedFiles[0].FileName,
                    uploadDate = DateTime.Now
                };
                while (sr.EndOfStream == false)
                {
                    line = sr.ReadLine();
                    if (!started)
                    {
                        started = true;
                    }

                    var list = line.Split(',');
                    if (started && list.Length >= 5 && list[2].Trim() != "" && list[5].Trim() != "")
                    {
                        var mgtUnit = list[0];
                        var staffID = list[2];
                        var oldStaffID = list[1];
                        var name = list[3];
                        var balBF = list[4];
                        var monthDed = list[5];
                        loanNum = list.Length == 6? "":list[6];


                        if (mgtUnit != "" && staffID != "" && name != "" && balBF != "" && monthDed != "")
                        {
                            var detail = new controllerFileDetail
                            {
                                balBF = double.Parse(balBF),
                                employeeName = name,
                                managementUnit = mgtUnit,
                                monthlyDeduction = double.Parse(monthDed),
                                oldID = oldStaffID,
                                staffID = staffID,
                                remarks = ""
                            };
                            file.controllerFileDetails.Add(detail);

                            var dt = new ControllerOutViewModel
                            {
                                mgtUnit = mgtUnit,
                                oldStaffID = oldStaffID,
                                staffID = staffID,
                                loanNum = loanNum,
                                name = name,
                                balBF = balBF,
                                monthDed = monthDed
                            };
                            FileDetails.Add(dt);
                        }
                    }
                }
                if (FileDetails.Count > 0)
                {
                    List<string> distinctStaffIds = new List<string>();
                    List<string> duplicateStaffIds = new List<string>();

                    //remove duplicates which has no loan number specified from list to process.
                    foreach (var row in FileDetails)
                    {
                        if (distinctStaffIds.Contains(row.staffID) && row.loanNum == "" && !duplicateStaffIds.Contains(row.staffID))
                        {
                            duplicateStaffIds.Add(row.staffID);
                            var toRem = file.controllerFileDetails.Where(p => p.staffID == row.staffID).ToList();
                            var len = toRem.Count;
                            for (var x=0;x< len; x++)
                            {
                                var remov = file.controllerFileDetails.FirstOrDefault(p => p.staffID == row.staffID);
                                file.controllerFileDetails.Remove(remov);
                            }
                        }
                        else distinctStaffIds.Add(row.staffID);
                    }

                    //retrieve records to send back
                    var duplicatesRecords = FileDetails
                        .Where(p => duplicateStaffIds.Contains(p.staffID))
                        .ToList();

                    var distinctRecords = FileDetails
                        .Where(p => !duplicateStaffIds.Contains(p.staffID))
                        .ToList();


                    if (duplicatesRecords.Count > 0)
                    {
                        HtmlHelper.MessageBox("Duplicates found, Please check output file!");
                        CreateExcelWithConflictRecords(duplicatesRecords, "Duplicates");
                        CreateExcelWithConflictRecords(distinctRecords, "Distinct");
                    }
                    else
                    {
                        HtmlHelper.MessageBox2("No duplicates without loan numbers found, File is clean!", ResolveUrl("~/ln/loans/controllerOut.aspx"), 
                        "coreERP©: Successful", IconType.ok);
                    }
                }
            }
        }



        public static void CreateExcelWithConflictRecords(List<ControllerOutViewModel> dataToReturn, string fileDescription)
        {
            GC.Collect();
            
            string[,] tmp = new string[dataToReturn.Count,7];
            List<string> data = new List<string>();

            for (int i=0; i<dataToReturn.Count; i++)
            {
                var record = dataToReturn[i];
                var st = record.mgtUnit + "," + record.oldStaffID + "," + record.staffID + "," + record.name + "," +
                         record.balBF + "," + record.monthDed + "," + record.loanNum;
                data.Add(st);
            }


            var name = fileDescription +"_"+ DateTime.Now.GetHashCode() + ".csv";
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"C:\Users\Public\Documents\"+ name))
            {
                foreach (var record in data)
                {
                    
                        file.WriteLine(record);
                    
                }
            }


            GC.Collect();

        }

    }
}