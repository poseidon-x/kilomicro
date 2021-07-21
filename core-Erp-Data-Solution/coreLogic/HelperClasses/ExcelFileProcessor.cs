using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel;
using coreLogic.Models.Loans;
//using starbowProcessingLib.Models;

namespace coreLogic.HelperClasses
{
    public class ExcelFileProcessor
    {
        private readonly string FILE_INACCESSIBLE = "FILE_INACCESSIBLE";
        public string filePath { get; set; }
        public ExcelFileProcessor(string filePath)
        {
            this.filePath = filePath;
        }
        public IExcelDataReader processFile() // file processing returns a reader that can be processed to get rows
        {
            var file = new FileInfo(filePath);
            FileStream stream =  null;
            IExcelDataReader reader = null;
            try
            {
                stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
                if (file.Extension == ".xls")
                {
                    reader = ExcelReaderFactory.CreateBinaryReader(stream);

                }
                else if (file.Extension == ".xlsx")
                {
                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                stream.Close();
                reader.IsFirstRowAsColumnNames = true;
                return reader; 
            }
            catch (IOException)
            {
                throw new ApplicationException(FILE_INACCESSIBLE);
            }          
                     
        }
        
        

        public List<controllerFileDetail> getRepaymentResults()
        {
            
            var repaymentList = new List<controllerFileDetail>();
            IExcelDataReader data = processFile();
            data.Read(); //put the cursor on actual data
            while (data.Read())
            {
                var row = new ControllerOutViewModel
                {
                    mgtUnit = data.GetString(0),
                    staffID = data.GetString(1),
                    name = data.GetString(2),
                    balBF = data.GetString(3),
                    monthDed = data.GetString(4),
                };

                if (isValid(row))
                {
                    var detail = new controllerFileDetail
                    {
                        balBF = double.Parse(row.balBF),
                        employeeName = row.name,
                        managementUnit = row.mgtUnit,
                        loanNo = row.loanNum,
                        monthlyDeduction = double.Parse(row.monthDed),
                        staffID = row.staffID,
                        oldID = "",
                        remarks = "",
                    };
                    repaymentList.Add(detail);
                }
            }
            return repaymentList;
        }

        private DateTime GetDate(object ob)
        {
            if (ob == null) return DateTime.Today;

            var str = ob.ToString();
            DateTime date = DateTime.Today;
            if(DateTime.TryParseExact(str, "dd-MMM-yy", System.Globalization.CultureInfo.CurrentCulture, 
                System.Globalization.DateTimeStyles.AssumeLocal, out date)) return date;
            if (DateTime.TryParseExact(str, "dd-MMM-yyyy", System.Globalization.CultureInfo.CurrentCulture,
                System.Globalization.DateTimeStyles.AssumeLocal, out date)) return date;
            if (DateTime.TryParseExact(str, "dd-MM-yy", System.Globalization.CultureInfo.CurrentCulture,
                System.Globalization.DateTimeStyles.AssumeLocal, out date)) return date;
            if (DateTime.TryParseExact(str, "dd-MM-yyyy", System.Globalization.CultureInfo.CurrentCulture,
                System.Globalization.DateTimeStyles.AssumeLocal, out date)) return date;
            if (DateTime.TryParseExact(str, "dd/MMM/yy", System.Globalization.CultureInfo.CurrentCulture,
                System.Globalization.DateTimeStyles.AssumeLocal, out date)) return date;
            if (DateTime.TryParseExact(str, "dd/MM/yy", System.Globalization.CultureInfo.CurrentCulture,
                System.Globalization.DateTimeStyles.AssumeLocal, out date)) return date;
            if (DateTime.TryParseExact(str, "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentCulture,
                System.Globalization.DateTimeStyles.AssumeLocal, out date)) return date;
            if (DateTime.TryParseExact(str, "dd/MMM/yyyy", System.Globalization.CultureInfo.CurrentCulture,
                System.Globalization.DateTimeStyles.AssumeLocal, out date)) return date;

            return DateTime.Today;
        }

        
        

        

        private double GetDouble(string fieldValue)
        {
            try
            {
                if (fieldValue != null && fieldValue.Length > 0)
                {
                    double value = 0;
                    double.TryParse(fieldValue, out value);
                    return value;
                }
            }
            catch (Exception) { }
            return 0;
        }

        private bool isValid(ControllerOutViewModel input)
        {
            if ((string.IsNullOrEmpty(input.name) || string.IsNullOrWhiteSpace(input.name))
                || (string.IsNullOrEmpty(input.staffID) || string.IsNullOrWhiteSpace(input.staffID))
                || (string.IsNullOrEmpty(input.mgtUnit) || string.IsNullOrWhiteSpace(input.mgtUnit))
                || (string.IsNullOrEmpty(input.balBF) || string.IsNullOrWhiteSpace(input.balBF))
                || (string.IsNullOrEmpty(input.monthDed) || string.IsNullOrWhiteSpace(input.monthDed)))
            {
                return false;
            }
            return true;
        }

    }
}
