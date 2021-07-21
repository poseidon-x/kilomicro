using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreData.Constants
{
    public class ErrorMessages
    {
        public static readonly string ErrorSavingToServe = "Error saving to server <br />";
        public static readonly string ErrorSavingToServer = "Error saving to server <br />";
        public static readonly string CorrectData = "Please correct the wrong entries and save again <br />";

        public static readonly string ErrorLoggingIn = "Log in failed <br />";
        public readonly string AssemblyLineTypeError = "One or more form drop down have invalid input <br />";
        public readonly string AssemblyLineNameEmptyError = "Assembly Line Name is invalid <br />";
        public readonly string AssemblyLineNameSizeError = "Assembly Line Name is too Long <br />";
        public readonly string AssemblyLineWorkStageError = "One or more Work stage(s) has invalid Work Stage Type <br />";
        public readonly string AssemblyLineEmptyWorkStageError = "One or more Work stage(s) has invalid value <br />";
        public readonly string AssemblyLineEmptyWorkStageDataSize = "One or more Work stage(s) has record with value that exceed its character lenght <br />";

        public readonly string SHRINKAGE_DROP_DOWN_ERROR_MESSAGE = "Be sure you have selected from the provided Drop Down list in the Details Grid<br />";
        public readonly string SHRINKAGE_EMPTY_FIELDS_ERROR_MESSAGE = "Please make sure all the Grid fields are not Empty<br />";
        public readonly string SHRINKAGE_QUANTITY_SHRUNK_SIZE_ERROR_MESSAGE = "Please make sure all quantity shrunk is a real number<br />";
        public readonly string GridWithoutDataErrorMessage = "Details Grid cannot be empty<br />";

        public readonly string CreditLineInvalidClient = "Invalid client selected<br />";
        public readonly string CreditLineNumericFieldError = "Numeric fields must have a min value of 1 <br />";
        public readonly string CreditLineInvalidDate = "One or more date fields has invalid date <br />";

        public readonly string BorrowingFeesGridWithoutData = "Please make sure borrowing Fees has at least record <br />";
        public readonly string BorrowingFormError = "One or more form drop down have invalid input <br />";
        public readonly string BorrowingInvalidApplicationDate = "Application date is invalid <br />";
        public readonly string BorrowingFutureApplicationDate = "Application date cannot be a future date <br />";
        public readonly string BorrowingEmptyNumericInput = "One or more numeric input has invalid value <br />";
        public readonly string BorrowingEmptyCreditOfficer = "Credit officer notes cannot be empty <br />";
        public readonly string BorrowingAmountApprove = "Credit officer notes cannot be empty <br />";
        public readonly string BorrowingAmountApproveError = "Borrowing is not approved or approval date is null <br />";
        public readonly string BorrowingRepaymentAmountError = "Borrowing Account repayment amount cannot be less than One(1) <br />";

        public readonly string LoanDocumentInvalidDocPUT = "The selected document doesn't exist<br />";
        //public readonly string CreditLineNumericFieldError = "Numeric fields must have a min value of 1 <br />";
        //public readonly string CreditLineInvalidDate = "One or more date fields has invalid date <br />";

        //Loan Group Api Error Messages
        public readonly string LoanGroupClientsBelowMin = "Group client cannot be less than five <br />";


        public readonly string InvalidClient = "The selected client doesn't exist <br />";
        public readonly string InvalidAmount = "Deposit amount cann't be less than one(1) <br />";
        public readonly string InvalidReceiptDate = "Receipt date is invalid <br />";


        //For Loan Approval
        public readonly string LoanFullyApprovedAlreadyErrorMsg = "The loan you are trying to approve, has already been fully approved <br />";

        //For Clients
        public readonly string ClientSurnameEmpty = "Client Surname is Empty <br />";
        public readonly string ClientSurnameLength = "Client Surname is too long. (100) characters allowed <br />";
        public readonly string ClientOtherNamesEmpty = "Client Other Names is Empty <br />";
        public readonly string ClientOtherNamesLength = "Client Other Names is too long. (100) characters allowed <br />";
        public readonly string ClientBranchEmpty = "No Client Branch Selected <br />";
        public readonly string ClientCategoryEmpty = "No Client Category Selected <br />";
        public readonly string ClientTypeEmpty = "No Client Type Selected <br />";

        // AGENCY
        public readonly string SearchCriteriaEmpty = "No Search Criteria Selected <br />";
        public readonly string SurnameEmpty = "No Surname Entered <br />";
        public readonly string OtherNamesEmpty = "No Other Names Entered <br />";
        public readonly string AccountNumberEmpty = "No Account Number Entered <br />";
        public readonly string SearchDataEmpty = "No Search Data entered <br />";


        // Client Service Charge
        public readonly string InputDataEmpty = "Client service charge cannot be empty <br />";
        public readonly string ChargePostedAlready = "Charge already posted and cannot be edited <br />";

        // For RepaymentManager.cs
        public static readonly string OverPrincipalPayment = "Sorry principal payment is more than principal balance <br />";
        public static readonly string OverInterestPayment = "Sorry interest payment is more than interest balance <br />";
        public static readonly string OverPenaltyPayment = "Sorry penalty payment is more than penalty balance <br />";

        //Loan Provision
        public static readonly string ErrorInitializingProvision = "An error occured in initializing provisions <br />";


        //Cashier Till Day
        public static readonly string InvalidInput = "Input cannot be empty <br />";
        public static readonly string InvalidCashierSelected = "The selected cashier doesn't exist <br />";
        public static readonly string InvalidStartDate = "Invalid start date selected <br />";
        public static readonly string InvalidEndDate = "Invalid end date selected <br /> <br />";
        public static readonly string FutureStartDate = "Start date cannot be a future date  <br />";
        public static readonly string FutureEndDate = "End date cannot be a future date <br /> <br />";


        //Loan Group
        public static readonly string InvalidGroupLeader = "Selected group leader doesn't belong to this group <br />";
        public static readonly string EmptyGroupName = "Group name cannot be empty <br />";
        public static readonly string InvalidGroupDay = "Invalid group day selected <br />";
        public static readonly string InvalidRelationsOfficer = "Selected relations officer doesn't exist <br /> <br />";

        //System Date
        public static readonly string EmptyLoanDate = "Loan Date cannot be changed to empty <br />";
        public static readonly string EmptyInvestmentDate = "Client Investment Date cannot be changed to empty <br />";
        public static readonly string EmptySavingsDate = "Bank Account Date cannot be changed to empty <br />";
        public static readonly string EmptyCompanyInvestmentDate = "Company Investment Date cannot be changed to empty <br />";
        public static readonly string FutureLoanDate = "Loan Date cannot be a future Date  <br />";
        public static readonly string FutureInvestmentDate = "Client Investment Date cannot be a future Date <br />";
        public static readonly string FutureSavingsDate = "Bank Account Date cannot be a future Date <br />";
        public static readonly string FutureCompanyInvestmentDate = "Company Investment Date cannot be a future Date <br />";
        public static readonly string InvalidLoanDate = "Loan Date is invalid <br />";
        public static readonly string InvalidInvestmentDate = "Client Investment Date is invalid <br />";
        public static readonly string InvalidSavingsDate = "Bank Account Date is invalid <br />";
        public static readonly string InvalidCompanyInvestmentDate = "Company Investment Date is invalid <br />";




    }
}
