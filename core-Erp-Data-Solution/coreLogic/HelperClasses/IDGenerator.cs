using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using coreLogic;
using coreLogic.Models;

namespace coreLogic
{
    public class IDGenerator : coreLogic.IIDGenerator
    { 
        public IDGenerator() { }

        private static object locker = new object();

         
        public string NewClientAccountNumber(int branchID, int categoryID)
        {
            string accNum = "";
            using (var ent = new coreLogic.core_dbEntities())
            {
                using (var le = new coreLoansEntities())
                {
                    var idProf = le.id_prof.FirstOrDefault();
                    var br = le.branches.FirstOrDefault(p => p.branchID == branchID);
                    if (idProf == null)
                    {
                        var prof = ent.comp_prof.FirstOrDefault();
                        if (prof.traditionalLoanNo == true)
                        {
                            accNum = coreLogic.coreExtensions.NextSystemNumber("loan.cl.accountNumber." + categoryID.ToString());
                        }
                        else
                        {
                            accNum = coreLogic.coreExtensions.NextSystemNumber("loan.cl.accountNumber." + br.branchCode);
                        }
                    }
                    else
                    {
                        if (idProf.client_account_no_gen_scheme == 1)
                        {
                            var nsn = "";
                            nsn = coreLogic.coreExtensions.NextSystemNumber("loan.cl.accountNumber." +
                                    branchID.ToString() + "." + categoryID.ToString(), idProf.id_size - br.branchCode.Length);
                            accNum = br.branchCode + nsn;
                        }
                        else if (idProf.client_account_no_gen_scheme == 2)
                        {
                            var nsn = "";
                            if (br.branchName.ToLower().Contains(AppContants.AdentaBranch))
                            {
                                nsn = coreLogic.coreExtensions.NextSystemNumber("loan.cl.accountNumber." + categoryID.ToString());
                                accNum = nsn;
                            }
                            else
                            {
                                nsn = coreLogic.coreExtensions.NextSystemNumber("loan.cl.accountNumber." +
                                    branchID.ToString() + "." + categoryID.ToString(), idProf.id_size - br.branchCode.Length);
                                accNum = br.branchCode + nsn;
                            }
                        }
                        else if (idProf.client_account_no_gen_scheme == 3)
                        {
                            accNum = br.branchCode + coreLogic.coreExtensions.NextSystemNumber("loan.cl.accountNumber." + br.branchCode,idProf.id_size-br.branchCode.Trim().Length);
                        }
                        else if (idProf.client_account_no_gen_scheme == 4)
                        {
                            accNum = br.branchCode + idProf.fragment_separator+
                                coreLogic.coreExtensions.NextSystemNumber("loan.cl.accountNumber." + br.branchCode, idProf.id_size - br.branchCode.Trim().Length);
                        }
                    }
                }
            }

            return accNum;
        }

        public string NewStaffNumber(int branchID, DateTime startDate)
        {
            string accNum = "";
            using (var ent = new core_dbEntities())
            {
                using (var le = new coreLoansEntities())
                {
                    var idProf = le.id_prof.FirstOrDefault();
                    var br = le.branches.FirstOrDefault(p => p.branchID == branchID);
                    if (idProf == null)
                    {
                        var prof = ent.comp_prof.FirstOrDefault();
                        if (prof.traditionalLoanNo == true)
                        {
                            accNum = coreLogic.coreExtensions.NextSystemNumber("loan.staff.staffNo." + br.branchCode);
                        }
                        else
                        {
                            accNum = coreLogic.coreExtensions.NextSystemNumber("loan.staff.staffNo." + br.branchCode);
                        }
                    }
                    else
                    {
                        if (idProf.client_account_no_gen_scheme == 1)
                        {
                            accNum = coreLogic.coreExtensions.NextSystemNumber("loan.staff.staffNo." + br.branchCode);
                        }
                        else if (idProf.client_account_no_gen_scheme == 2)
                        {
                            accNum = coreLogic.coreExtensions.NextSystemNumber("loan.staff.staffNo." + br.branchCode);
                        }
                        else if (idProf.client_account_no_gen_scheme == 3 )
                        {
                            accNum = br.branchCode + coreLogic.coreExtensions.NextSystemNumber("loan.staff.staffNo." + br.branchCode, 
                                idProf.id_size - br.branchCode.Trim().Length);
                        }
                        else if (idProf.client_account_no_gen_scheme == 4)
                        {
                            accNum = startDate.Year.ToString() + idProf.fragment_separator + br.branchCode + idProf.fragment_separator +
                                coreLogic.coreExtensions.NextSystemNumber("loan.staff.staffNo." + br.branchCode,
                                idProf.id_size - br.branchCode.Trim().Length - idProf.fragment_separator.Length - idProf.fragment_separator.Length-
                                startDate.Year.ToString().Length);
                        }
                    }
                }
            }

            return accNum;
        }

        public string NewLoanNumber(int branchID, int clientID, int loanID=0, string productPrefix="01")
        {
            string accNum = "";
            using (var ent = new core_dbEntities())
            {
                using (var le = new coreLoansEntities())
                {
                    var idProf = le.id_prof.FirstOrDefault();
                    var br = le.branches.FirstOrDefault(p => p.branchID == branchID);
                    var prof = ent.comp_prof.FirstOrDefault();
                    var client = le.clients.FirstOrDefault(p => p.clientID == clientID);
                    if (idProf == null)
                    {
                        if (ent.comp_prof.FirstOrDefault().traditionalLoanNo == true && client != null)
                        {
                            var pastLoans = le.loans.Where(p => p.clientID == clientID &&
                                p.loanID != loanID).Count();
                            accNum = client.accountNumber + "/" + (pastLoans + 1).ToString();
                        }
                        else
                        {
                            accNum = productPrefix + "" +
                                coreLogic.coreExtensions.NextSystemNumber(
                                    "LOAN_" + productPrefix);
                        }
                    }
                    else
                    {
                        if (idProf.client_account_no_gen_scheme == 1)
                        {
                            var pastLoans = le.loans.Where(p => p.clientID == clientID &&
                                p.loanID != loanID).Count();
                            accNum = client.accountNumber + "/" + (pastLoans + 1).ToString();
                        }
                        else if (idProf.client_account_no_gen_scheme == 2)
                        {
                            accNum = productPrefix + "" +
                               coreLogic.coreExtensions.NextSystemNumber(
                                   "LOAN_" + productPrefix);
                        }
                        else if (idProf.client_account_no_gen_scheme == 3)
                        {
                            var pc = le.productCodes.FirstOrDefault();
                            accNum = br.branchCode + pc.loanAccountCode 
                                + coreLogic.coreExtensions.NextSystemNumber("LOAN_" + br.branchCode, idProf.id_size - br.branchCode.Trim().Length - pc.loanAccountCode.Trim().Length);
                        }
                        else if (idProf.client_account_no_gen_scheme == 4)
                        {
                            var pc = le.productCodes.FirstOrDefault();
                            accNum = br.branchCode + idProf.fragment_separator + pc.loanAccountCode + idProf.fragment_separator
                                + coreLogic.coreExtensions.NextSystemNumber("LOAN_" + br.branchCode, idProf.id_size - br.branchCode.Trim().Length - pc.loanAccountCode.Trim().Length
                                - idProf.fragment_separator.Length - idProf.fragment_separator.Length);
                        }
                    }
                }
            }

            return accNum;
        }

        public string NewSavingsNumber(int branchID, int clientID, int savingID = 0, string productPrefix = "05")
        {
            string accNum = "";
            using (var ent = new core_dbEntities())
            {
                using (var le = new coreLoansEntities())
                {
                    var idProf = le.id_prof.FirstOrDefault();
                    var br = le.branches.FirstOrDefault(p => p.branchID == branchID);
                    var prof = ent.comp_prof.FirstOrDefault();
                    var client = le.clients.FirstOrDefault(p => p.clientID == clientID);
                    if (client != null)
                    {
                        productPrefix = client.category.categoryName.Substring(0, 2).ToUpper();
                    }
                    if (idProf == null)
                    {
                        if (prof.traditionalLoanNo == true)
                        {
                            var cl = le.clients.First(p => p.clientID == client.clientID);
                            //cl.savings.Load();
                            var cnt = cl.savings.Count(p => p.savingID != savingID);
                            char c = (char)(((int)'A') + cnt);
                            accNum = cl.accountNumber + "/" + c;
                        }
                        else
                        {
                            if (prof.comp_name.ToLower().Trim().StartsWith("link"))
                            {
                                accNum = "IN" + "" +
                                         coreLogic.coreExtensions.NextSystemNumber(
                                             "SAVING_" + "IN");
                            }
                            else
                            {
                                accNum = productPrefix + "" +
                                         coreLogic.coreExtensions.NextSystemNumber(
                                             "SAVING_" + productPrefix);
                            }
                        }
                    }
                    else
                    {
                        if (idProf.client_account_no_gen_scheme == 1)
                        {
                            var cl = le.clients.FirstOrDefault(p => p.clientID == client.clientID);
                            //cl.savings.Load();
                            var cnt = cl.savings.Count(p => p.savingID != savingID);
                            char c = (char)(((int)'A') + cnt);
                            accNum = cl.accountNumber + "/" + c;
                        }
                        else if (idProf.client_account_no_gen_scheme == 2)
                        {
                            accNum = productPrefix + "" +
                                 coreLogic.coreExtensions.NextSystemNumber(
                                 "SAVING_" + productPrefix);
                        }
                        else if (idProf.client_account_no_gen_scheme == 3)
                        {
                            var pc = le.productCodes.First();
                            accNum = br.branchCode + pc.savingsAccountCode
                                + coreLogic.coreExtensions.NextSystemNumber("SAVINGS_" + br.branchCode, idProf.id_size - br.branchCode.Trim().Length - pc.savingsAccountCode.Trim().Length);
                        }
                        else if (idProf.client_account_no_gen_scheme == 4)
                        {
                            var pc = le.productCodes.FirstOrDefault();
                            accNum = br.branchCode + idProf.fragment_separator + pc.savingsAccountCode + idProf.fragment_separator +
                               coreLogic.coreExtensions.NextSystemNumber("SAVINGS_" + br.branchCode, idProf.id_size - br.branchCode.Trim().Length -
                               pc.savingsAccountCode.Trim().Length - idProf.fragment_separator.Length - idProf.fragment_separator.Length);
                        }
                    }
                }
            }

            return accNum;
        }

        public string NewDepositNumber(int branchID, int clientID, int depositID = 0, string productPrefix = "02")
        {
            string accNum = "";
            using (var ent = new core_dbEntities())
            {
                using (var le = new coreLoansEntities())
                {
                    var idProf = le.id_prof.FirstOrDefault();
                    var br = le.branches.FirstOrDefault(p => p.branchID == branchID);
                    var prof = ent.comp_prof.FirstOrDefault();
                    var client = le.clients.FirstOrDefault(p => p.clientID == clientID);
                    if (idProf == null)
                    {
                        if (prof.traditionalLoanNo == true)
                        {
                            var cl = le.clients.FirstOrDefault(p => p.clientID == client.clientID); 
                            var cnt = cl.deposits.Where(p => p.depositID != depositID).Count();
                            if (cnt >= 26)
                            {
                                accNum = cl.accountNumber + "/" + (cnt + 1).ToString();
                            }
                            else
                            {
                                char c = (char)(((int)'A') + cnt);
                                accNum = cl.accountNumber + "/" + c;
                                while (cl.deposits.Any(p => p.depositNo.ToUpper() == accNum.ToUpper()))
                                {
                                    c = (char)((int)c - 1);
                                    accNum = cl.accountNumber + "/" + c;
                                }
                                if (cl.deposits.Any(p => p.depositNo.ToUpper() == accNum.ToUpper()))
                                {
                                    accNum = cl.accountNumber + "/" + (cnt + 1).ToString();
                                }
                            }
                        }
                        else
                        {
                            accNum = productPrefix + "" +
                                coreLogic.coreExtensions.NextSystemNumber(
                                "DEPOSIT_" + productPrefix);
                        }
                    }
                    else
                    {
                        if (idProf.client_account_no_gen_scheme == 1 || idProf.client_account_no_gen_scheme == 2)
                        {
                            var cl = le.clients.FirstOrDefault(p => p.clientID == client.clientID);
                            var cnt = cl.deposits.Where(p => p.depositID != depositID).Count();
                            if (cnt >= 26)
                            {
                                accNum = cl.accountNumber + "/" + (cnt + 1).ToString();
                            }
                            else
                            {
                                char c = (char) (((int) 'A') + cnt);
                                accNum = cl.accountNumber + "/" + c;
                                while (cl.deposits.Any(p => p.depositNo.ToUpper() == accNum.ToUpper()))
                                {
                                    c = (char) ((int) c - 1);
                                    accNum = cl.accountNumber + "/" + c;
                                }
                                if (cl.deposits.Any(p => p.depositNo.ToUpper() == accNum.ToUpper()))
                                {
                                    accNum = cl.accountNumber + "/" + (cnt + 1).ToString();
                                }
                            }
                        }
                        else if (idProf.client_account_no_gen_scheme == 4)
                        {
                            var pc = le.productCodes.FirstOrDefault();
                            accNum = br.branchCode + pc.depositAccountCode
                                     +
                                     coreLogic.coreExtensions.NextSystemNumber("DEPOSIT_" + br.branchCode,
                                         idProf.id_size - br.branchCode.Trim().Length -
                                         pc.depositAccountCode.Trim().Length);
                        }
                        else if (idProf.client_account_no_gen_scheme == 4)
                        {
                            var pc = le.productCodes.FirstOrDefault();
                            accNum = br.branchCode + idProf.fragment_separator + pc.depositAccountCode +
                                     idProf.fragment_separator
                                     +
                                     coreLogic.coreExtensions.NextSystemNumber("DEPOSIT_" + br.branchCode,
                                         idProf.id_size - br.branchCode.Trim().Length -
                                         pc.depositAccountCode.Trim().Length - idProf.fragment_separator.Length -
                                         idProf.fragment_separator.Length);
                        }
                    }
                }
            }

            return accNum;
        }

        public string NewInvestmentNumber(int branchID, int clientID, int investmentID = 0, string productPrefix = "02")
        {
            string accNum = "";
            using (var ent = new core_dbEntities())
            {
                using (var le = new coreLoansEntities())
                {
                    var idProf = le.id_prof.FirstOrDefault();
                    var br = le.branches.FirstOrDefault(p => p.branchID == branchID);
                    var prof = ent.comp_prof.FirstOrDefault();
                    var client = le.clients.FirstOrDefault(p => p.clientID == clientID);
                    if (idProf == null)
                    {
                        if (prof.traditionalLoanNo == true)
                        {
                            var cl = le.clients.FirstOrDefault(p => p.clientID == client.clientID);
                            var cnt = cl.investments.Where(p => p.investmentID != investmentID).Count();
                            if (cnt >= 26)
                            {
                                accNum = cl.accountNumber + "/" + (cnt + 1).ToString();
                            }
                            else
                            {
                                char c = (char)(((int)'A') + cnt);
                                accNum = cl.accountNumber + "/" + c;
                                while (cl.investments.Any(p => p.investmentNo.ToUpper() == accNum.ToUpper()))
                                {
                                    c = (char)((int)c - 1);
                                    accNum = cl.accountNumber + "/" + c;
                                }
                                if (cl.investments.Any(p => p.investmentNo.ToUpper() == accNum.ToUpper()))
                                {
                                    accNum = cl.accountNumber + "/" + (cnt + 1).ToString();
                                }
                            }
                        }
                        else
                        {
                            accNum = productPrefix + "" +
                                coreLogic.coreExtensions.NextSystemNumber(
                                "INVESTMENT_" + productPrefix);
                        }
                    }
                    else
                    {
                        if (idProf.client_account_no_gen_scheme == 1)
                        {
                            var cl = le.clients.FirstOrDefault(p => p.clientID == client.clientID);
                            var cnt = cl.investments.Where(p => p.investmentID != investmentID).Count();
                            if (cnt >= 26)
                            {
                                accNum = cl.accountNumber + "/" + (cnt + 1).ToString();
                            }
                            else
                            {
                                char c = (char)(((int)'A') + cnt);
                                accNum = cl.accountNumber + "/" + c;
                                while (cl.investments.Any(p => p.investmentNo.ToUpper() == accNum.ToUpper()))
                                {
                                    c = (char)((int)c - 1);
                                    accNum = cl.accountNumber + "/" + c;
                                }
                                if (cl.investments.Any(p => p.investmentNo.ToUpper() == accNum.ToUpper()))
                                {
                                    accNum = cl.accountNumber + "/" + (cnt + 1).ToString();
                                }
                            }
                        }
                        else if (idProf.client_account_no_gen_scheme == 2)
                        {
                            accNum = productPrefix + "" +
                                 coreLogic.coreExtensions.NextSystemNumber(
                                 "INVESTMENT_" + productPrefix);
                        }
                        else if (idProf.client_account_no_gen_scheme == 4)
                        {
                            var pc = le.productCodes.FirstOrDefault();
                            accNum = br.branchCode + pc.investmentAccountCode
                                + coreLogic.coreExtensions.NextSystemNumber("INVESTMENT_" + br.branchCode, idProf.id_size - br.branchCode.Trim().Length - pc.investmentAccountCode.Trim().Length);
                        }
                        else if (idProf.client_account_no_gen_scheme == 4)
                        {
                            var pc = le.productCodes.FirstOrDefault();
                            accNum = br.branchCode + idProf.fragment_separator + pc.investmentAccountCode + idProf.fragment_separator
                                + coreLogic.coreExtensions.NextSystemNumber("INVESTMENT_" + br.branchCode, idProf.id_size - br.branchCode.Trim().Length -
                                pc.investmentAccountCode.Trim().Length - idProf.fragment_separator.Length - idProf.fragment_separator.Length);
                        }
                    }
                }
            } 
            return accNum;
        }

        public string NewGroupSusuNumber(int branchID, int clientID, int susuAccountID = 0, string productPrefix = "03")
        {
            string accNum = "";
            using (var ent = new core_dbEntities())
            {
                using (var le = new coreLoansEntities())
                {
                    var idProf = le.id_prof.FirstOrDefault();
                    var br = le.branches.FirstOrDefault(p => p.branchID == branchID);
                    var prof = ent.comp_prof.FirstOrDefault();
                    var client = le.clients.FirstOrDefault(p => p.clientID == clientID);
                    if (client != null)
                    {
                        productPrefix = client.category.categoryName.Substring(0, 2).ToUpper();
                    }
                    if (idProf == null)
                    {
                        if (prof.traditionalLoanNo == true)
                        {
                            var cnt = le.susuAccounts.Where(p => p.clientID == client.clientID).Count();
                            cnt = cnt + 1;
                            accNum = client.accountNumber + "/" + GetRomanNumeral(cnt);
                        }
                        else
                        {
                            accNum = productPrefix + "" +
                                coreLogic.coreExtensions.NextSystemNumber(
                                "GROUP_SUSU_" + productPrefix);
                        }
                    }
                    else
                    {
                        if (idProf.client_account_no_gen_scheme == 1)
                        {
                            var cnt = le.susuAccounts.Where(p => p.clientID == client.clientID).Count();
                            cnt = cnt + 1;
                            accNum = client.accountNumber + "/" + GetRomanNumeral(cnt);
                        }
                        else if (idProf.client_account_no_gen_scheme == 2)
                        {
                            accNum = productPrefix + "" +
                                 coreLogic.coreExtensions.NextSystemNumber(
                                 "GROUP_SUSU_" + productPrefix);
                        }
                        else if (idProf.client_account_no_gen_scheme == 3)
                        {
                            var pc = le.productCodes.FirstOrDefault();
                            accNum = br.branchCode + pc.groupSusuAccountCode
                                + coreLogic.coreExtensions.NextSystemNumber("GROUP_SUSU_" + br.branchCode, idProf.id_size - br.branchCode.Trim().Length - pc.groupSusuAccountCode.Trim().Length);
                        }
                        else if (idProf.client_account_no_gen_scheme == 4)
                        {
                            var pc = le.productCodes.FirstOrDefault();
                            accNum = br.branchCode + idProf.fragment_separator + pc.groupSusuAccountCode + idProf.fragment_separator
                                + coreLogic.coreExtensions.NextSystemNumber("GROUP_SUSU_" + br.branchCode, idProf.id_size - br.branchCode.Trim().Length -
                                pc.groupSusuAccountCode.Trim().Length - idProf.fragment_separator.Length - idProf.fragment_separator.Length);
                        }
                    }
                }
            }

            return accNum;
        }

        public string NewNormalSusuNumber(int branchID, int clientID, int susuAccountID = 0, string productPrefix = "04")
        {
            string accNum = "";
            using (var ent = new core_dbEntities())
            {
                using (var le = new coreLoansEntities())
                {
                    var idProf = le.id_prof.FirstOrDefault();
                    var br = le.branches.FirstOrDefault(p => p.branchID == branchID);
                    var prof = ent.comp_prof.FirstOrDefault();
                    var client = le.clients.FirstOrDefault(p => p.clientID == clientID);
                    if (client != null)
                    {
                        productPrefix = client.category.categoryName.Substring(0, 2).ToUpper();
                    }
                    if (idProf == null)
                    {
                        if (prof.traditionalLoanNo == true)
                        {
                            var cnt = le.susuAccounts.Where(p => p.clientID == client.clientID).Count();
                            cnt = cnt + 1;
                            accNum = client.accountNumber + "/" + GetRomanNumeral(cnt);
                        }
                        else
                        {
                            accNum = productPrefix + "" +
                                coreLogic.coreExtensions.NextSystemNumber(
                                "NORMAL_SUSU_" + productPrefix);
                        }
                    }
                    else
                    {
                        if (idProf.client_account_no_gen_scheme == 1)
                        {
                            var cnt = le.susuAccounts.Where(p => p.clientID == client.clientID).Count();
                            cnt = cnt + 1;
                            accNum = client.accountNumber + "/" + GetRomanNumeral(cnt);
                        }
                        else if (idProf.client_account_no_gen_scheme == 2)
                        {
                            accNum = productPrefix + "" +
                                 coreLogic.coreExtensions.NextSystemNumber(
                                 "NORMAL_SUSU_" + productPrefix);
                        }
                        else if (idProf.client_account_no_gen_scheme == 3 || idProf.client_account_no_gen_scheme == 4)
                        {
                            var pc = le.productCodes.FirstOrDefault();
                            accNum = br.branchCode + pc.normalSusuAccountCode
                                + coreLogic.coreExtensions.NextSystemNumber("NORMAL_SUSU_" + br.branchCode, idProf.id_size - br.branchCode.Trim().Length - pc.normalSusuAccountCode.Trim().Length);
                        }
                        else if (idProf.client_account_no_gen_scheme == 4)
                        {
                            var pc = le.productCodes.FirstOrDefault();
                            accNum = br.branchCode + idProf.fragment_separator + pc.normalSusuAccountCode + idProf.fragment_separator
                                + coreLogic.coreExtensions.NextSystemNumber("NORMAL_SUSU_" + br.branchCode, idProf.id_size - br.branchCode.Trim().Length -
                                pc.normalSusuAccountCode.Trim().Length - idProf.fragment_separator.Length - idProf.fragment_separator.Length);
                        }
                    }
                }
            }

            return accNum;
        }

        public string NewInventoryItemNumber(long inventoryItemId, ICommerceEntities ce,
            Icore_dbEntities ent, string productPrefix)
        { 
            string itemNum = "";

            var invtItm = ce.inventoryItems.FirstOrDefault(p => p.inventoryItemId == inventoryItemId);
            var prof = ent.comp_prof.FirstOrDefault();
            if (invtItm != null)
            {
                productPrefix = invtItm.product.productSubCategory.productCategory.productCategoryName.Substring(0, 1)
                    .ToUpper()
                                + invtItm.product.productSubCategory.productCategoryId.ToString();
            }
            itemNum = productPrefix + "" +
                     coreLogic.coreExtensions.NextSystemNumber(
                         "INVENTORY_ITEM_" + productPrefix);

            return itemNum;
        }

        private string GetRomanNumeral(int no)
        {
            var rn = "";

            var nums = new string[] { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII", "XIII", "XIV", "XV", "XVI", "XVII", "XVIII", "XIX", "XX" };

            if (no > nums.Length) rn = no.ToString();
            else rn = nums[no - 1];

            return rn;
        }

        //public static string newCreditLineNumber(IcoreLoansEntities le, creditLine credLin, string Prefix, int length)
        //{
        //    lock (locker)
        //    {
        //        string nextCreditLineNumber = Prefix;
        //        StringBuilder number = new StringBuilder(nextCreditLineNumber);

        //        for (int x = 0; x < length; x++)
        //        {
        //            number = number.Append("0");
        //        }

        //        nextCreditLineNumber = number.ToString();

        //        if (!le.creditLines.Any())
        //        {
        //            nextCreditLineNumber = nextCreditLineNumber.Remove(nextCreditLineNumber.Length - 1, 1)
        //                .Insert(nextCreditLineNumber.Length - 1, "1");
        //        }
        //        else
        //        {
        //            nextCreditLineNumber = (int.Parse(
        //            le.creditLines.OrderByDescending(i => i.creditLineNumber) // order by code descending
        //                .First() // get first one (last code)
        //                .creditLineNumber.Split(Prefix[Prefix.Length - 1])[1]) // get only the number part
        //         + 1).ToString(number.ToString()); // add 1 and format with parameter length's digits
        //        }

        //        return nextCreditLineNumber;
        //    }
        //}

        public static string newBorrowingNumber(IcoreLoansEntities le, borrowing borw, string Prefix, int length)
        {
            lock (locker)
            {
                string nextBorrowingNumber = Prefix;
                StringBuilder number = new StringBuilder(nextBorrowingNumber);

                for (int x = 0; x < length; x++)
                {
                    number = number.Append("0");
                }

                nextBorrowingNumber = number.ToString();

                if (!le.borrowings.Any())
                {
                    nextBorrowingNumber = nextBorrowingNumber.Remove(nextBorrowingNumber.Length - 1, 1)
                        .Insert(nextBorrowingNumber.Length - 1, "1");
                }
                else
                {
                    nextBorrowingNumber = (int.Parse(
                    le.borrowings.OrderByDescending(i => i.borrowingNo) // order by code descending
                        .First() // get first one (last code)
                        .borrowingNo.Split(Prefix[Prefix.Length - 1])[1]) // get only the number part
                 + 1).ToString(number.ToString()); // add 1 and format with parameter length's digits
                }

                return nextBorrowingNumber;
            }
        }

        public static string newGroupLoanNumber(IcoreLoansEntities le)
        {
            lock (locker)
            {
                string Prefix = "LG";
                int length = 8;

                string nextGroupNumber = Prefix;
                StringBuilder number = new StringBuilder(nextGroupNumber);

                for (int x = 0; x < length; x++)
                {
                    number = number.Append("0");
                }

                nextGroupNumber = number.ToString();

                if (!le.loanGroups.Any())
                {
                    nextGroupNumber = nextGroupNumber.Remove(nextGroupNumber.Length - 1, 1)
                        .Insert(nextGroupNumber.Length - 1, "1");
                }
                else
                {
                    nextGroupNumber = (int.Parse(
                    le.loanGroups.OrderByDescending(i => i.loanGroupNumber) // order by code descending
                        .First() // get first one (last code)
                        .loanGroupNumber.Split(Prefix[Prefix.Length - 1])[1]) // get only the number part
                 + 1).ToString(number.ToString()); // add 1 and format with parameter length's digits
                }

                return nextGroupNumber;
            }


        }

        

        public static string nextClientDepositNumber(IcoreLoansEntities le, int clientId)
        {
            lock (locker)
            {
                string nextClientDPNumber;
                string accountNum = le.clients.FirstOrDefault(p => p.clientID == clientId).accountNumber;
                var Prefix = "DP"+ accountNum + "/";
               
                int clientDeposits = le.deposits.Where(p => p.clientID == clientId).Count();
                nextClientDPNumber = Prefix + (clientDeposits +1);                    

                return nextClientDPNumber;
            }
        }

    }
}
