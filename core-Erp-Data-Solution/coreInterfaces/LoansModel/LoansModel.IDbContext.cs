﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace coreLogic
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    
    public interface IcoreLoansEntities : IDisposable
    {
    	int SaveChanges(); 
    	System.Data.Entity.Infrastructure.DbContextConfiguration Configuration {get;}
    	System.Data.Entity.Database Database {get;}
        IDbSet<asset> assets { get; set; }
        IDbSet<assetCategory> assetCategories { get; set; }
        IDbSet<assetDepreciation> assetDepreciations { get; set; }
        IDbSet<assetDocument> assetDocuments { get; set; }
        IDbSet<assetImage> assetImages { get; set; }
        IDbSet<assetSubCategory> assetSubCategories { get; set; }
        IDbSet<depreciationSchedule> depreciationSchedules { get; set; }
        IDbSet<jobTitle> jobTitles { get; set; }
        IDbSet<staffAddress> staffAddresses { get; set; }
        IDbSet<staffDocument> staffDocuments { get; set; }
        IDbSet<staffEmail> staffEmails { get; set; }
        IDbSet<staffImage> staffImages { get; set; }
        IDbSet<staffPhone> staffPhones { get; set; }
        IDbSet<allowanceType> allowanceTypes { get; set; }
        IDbSet<benefitsInKind> benefitsInKinds { get; set; }
        IDbSet<bonusCalculationType> bonusCalculationTypes { get; set; }
        IDbSet<bonusType> bonusTypes { get; set; }
        IDbSet<deductionType> deductionTypes { get; set; }
        IDbSet<employmentStatu> employmentStatus { get; set; }
        IDbSet<holidayType> holidayTypes { get; set; }
        IDbSet<leaveType> leaveTypes { get; set; }
        IDbSet<level> levels { get; set; }
        IDbSet<levelAllowance> levelAllowances { get; set; }
        IDbSet<levelBenefitsInKind> levelBenefitsInKinds { get; set; }
        IDbSet<levelDeduction> levelDeductions { get; set; }
        IDbSet<levelLeave> levelLeaves { get; set; }
        IDbSet<levelNotch> levelNotches { get; set; }
        IDbSet<month> months { get; set; }
        IDbSet<oneTimeDeductionType> oneTimeDeductionTypes { get; set; }
        IDbSet<overTime> overTimes { get; set; }
        IDbSet<payCalendar> payCalendars { get; set; }
        IDbSet<payMaster> payMasters { get; set; }
        IDbSet<payMasterAllowance> payMasterAllowances { get; set; }
        IDbSet<payMasterBenefitsInKind> payMasterBenefitsInKinds { get; set; }
        IDbSet<payMasterDeduction> payMasterDeductions { get; set; }
        IDbSet<payMasterLoan> payMasterLoans { get; set; }
        IDbSet<payMasterOneTimeDeduction> payMasterOneTimeDeductions { get; set; }
        IDbSet<payMasterOverTime> payMasterOverTimes { get; set; }
        IDbSet<payMasterPension> payMasterPensions { get; set; }
        IDbSet<payMasterTax> payMasterTaxes { get; set; }
        IDbSet<payMasterTaxRelief> payMasterTaxReliefs { get; set; }
        IDbSet<payrollPostingAccount> payrollPostingAccounts { get; set; }
        IDbSet<pensionType> pensionTypes { get; set; }
        IDbSet<performanceAppraisal> performanceAppraisals { get; set; }
        IDbSet<performanceAppraisalScore> performanceAppraisalScores { get; set; }
        IDbSet<performanceAppraisalType> performanceAppraisalTypes { get; set; }
        IDbSet<performanceArea> performanceAreas { get; set; }
        IDbSet<performanceContract> performanceContracts { get; set; }
        IDbSet<performanceContractItem> performanceContractItems { get; set; }
        IDbSet<performanceContractStatu> performanceContractStatus { get; set; }
        IDbSet<performanceContractTarget> performanceContractTargets { get; set; }
        IDbSet<performanceScore> performanceScores { get; set; }
        IDbSet<publicHoliday> publicHolidays { get; set; }
        IDbSet<qualificationSubject> qualificationSubjects { get; set; }
        IDbSet<qualificationType> qualificationTypes { get; set; }
        IDbSet<relationType> relationTypes { get; set; }
        IDbSet<shift> shifts { get; set; }
        IDbSet<shiftAllowance> shiftAllowances { get; set; }
        IDbSet<staffAllowance> staffAllowances { get; set; }
        IDbSet<staffAttendance> staffAttendances { get; set; }
        IDbSet<staffBenefitsInKind> staffBenefitsInKinds { get; set; }
        IDbSet<staffCalendar> staffCalendars { get; set; }
        IDbSet<staffDaysWorked> staffDaysWorkeds { get; set; }
        IDbSet<staffDeduction> staffDeductions { get; set; }
        IDbSet<staffLeave> staffLeaves { get; set; }
        IDbSet<staffLeaveBalance> staffLeaveBalances { get; set; }
        IDbSet<staffLoan> staffLoans { get; set; }
        IDbSet<staffLoanRepayment> staffLoanRepayments { get; set; }
        IDbSet<staffLoanSchedule> staffLoanSchedules { get; set; }
        IDbSet<staffLoanType> staffLoanTypes { get; set; }
        IDbSet<staffLoanTypeLevel> staffLoanTypeLevels { get; set; }
        IDbSet<staffManager> staffManagers { get; set; }
        IDbSet<staffOneTimeDeduction> staffOneTimeDeductions { get; set; }
        IDbSet<staffPension> staffPensions { get; set; }
        IDbSet<staffPromotion> staffPromotions { get; set; }
        IDbSet<staffQualification> staffQualifications { get; set; }
        IDbSet<staffRelation> staffRelations { get; set; }
        IDbSet<staffShift> staffShifts { get; set; }
        IDbSet<staffTaxRelief> staffTaxReliefs { get; set; }
        IDbSet<taxReliefType> taxReliefTypes { get; set; }
        IDbSet<taxTable> taxTables { get; set; }
        IDbSet<year> years { get; set; }
        IDbSet<address> addresses { get; set; }
        IDbSet<addressImage> addressImages { get; set; }
        IDbSet<addressType> addressTypes { get; set; }
        IDbSet<agent> agents { get; set; }
        IDbSet<agentAddress> agentAddresses { get; set; }
        IDbSet<agentDocument> agentDocuments { get; set; }
        IDbSet<agentEmail> agentEmails { get; set; }
        IDbSet<agentImage> agentImages { get; set; }
        IDbSet<agentNextOfKin> agentNextOfKins { get; set; }
        IDbSet<agentPhone> agentPhones { get; set; }
        IDbSet<bogReportsConfig> bogReportsConfigs { get; set; }
        IDbSet<branch> branches { get; set; }
        IDbSet<businessType> businessTypes { get; set; }
        IDbSet<cashierCashupCoin> cashierCashupCoins { get; set; }
        IDbSet<cashierCashupNote> cashierCashupNotes { get; set; }
        IDbSet<cashierDisbursement> cashierDisbursements { get; set; }
        IDbSet<cashierFundCoin> cashierFundCoins { get; set; }
        IDbSet<cashierFundNote> cashierFundNotes { get; set; }
        IDbSet<cashierFundsTransfer> cashierFundsTransfers { get; set; }
        IDbSet<cashierReceipt> cashierReceipts { get; set; }
        IDbSet<cashierRemainingCoin> cashierRemainingCoins { get; set; }
        IDbSet<cashierRemainingNote> cashierRemainingNotes { get; set; }
        IDbSet<cashiersTill> cashiersTills { get; set; }
        IDbSet<cashierTransferType> cashierTransferTypes { get; set; }
        IDbSet<category> categories { get; set; }
        IDbSet<categoryCheckList> categoryCheckLists { get; set; }
        IDbSet<checkType> checkTypes { get; set; }
        IDbSet<client> clients { get; set; }
        IDbSet<clientActivityType> clientActivityTypes { get; set; }
        IDbSet<clientAddress> clientAddresses { get; set; }
        IDbSet<clientBankAccount> clientBankAccounts { get; set; }
        IDbSet<clientBusinessActivity> clientBusinessActivities { get; set; }
        IDbSet<clientCompany> clientCompanies { get; set; }
        IDbSet<clientConfig> clientConfigs { get; set; }
        IDbSet<clientDocument> clientDocuments { get; set; }
        IDbSet<clientEmail> clientEmails { get; set; }
        IDbSet<clientImage> clientImages { get; set; }
        IDbSet<clientLiability> clientLiabilities { get; set; }
        IDbSet<clientMandateType> clientMandateTypes { get; set; }
        IDbSet<clientPhone> clientPhones { get; set; }
        IDbSet<clientServiceCharge> clientServiceCharges { get; set; }
        IDbSet<clientType> clientTypes { get; set; }
        IDbSet<collateralImage> collateralImages { get; set; }
        IDbSet<collateralType> collateralTypes { get; set; }
        IDbSet<collection> collections { get; set; }
        IDbSet<config> configs { get; set; }
        IDbSet<controllerFile> controllerFiles { get; set; }
        IDbSet<def_ln_accts> def_ln_accts { get; set; }
        IDbSet<deposit> deposits { get; set; }
        IDbSet<depositAdditional> depositAdditionals { get; set; }
        IDbSet<depositCertificateConfig> depositCertificateConfigs { get; set; }
        IDbSet<depositCharge> depositCharges { get; set; }
        IDbSet<depositConfig> depositConfigs { get; set; }
        IDbSet<depositDisInvestmentConfig> depositDisInvestmentConfigs { get; set; }
        IDbSet<depositInterest> depositInterests { get; set; }
        IDbSet<depositPeriodInDay> depositPeriodInDays { get; set; }
        IDbSet<depositRepaymentMode> depositRepaymentModes { get; set; }
        IDbSet<depositSchedule> depositSchedules { get; set; }
        IDbSet<depositSignatory> depositSignatories { get; set; }
        IDbSet<depositTemplate> depositTemplates { get; set; }
        IDbSet<depositType> depositTypes { get; set; }
        IDbSet<depositTypeAllowedTenure> depositTypeAllowedTenures { get; set; }
        IDbSet<depositTypePlanRate> depositTypePlanRates { get; set; }
        IDbSet<depositWithdrawal> depositWithdrawals { get; set; }
        IDbSet<dur_type> dur_type { get; set; }
        IDbSet<email> emails { get; set; }
        IDbSet<emailType> emailTypes { get; set; }
        IDbSet<employeeCategory> employeeCategories { get; set; }
        IDbSet<employeeContractType> employeeContractTypes { get; set; }
        IDbSet<employer> employers { get; set; }
        IDbSet<employerDepartment> employerDepartments { get; set; }
        IDbSet<employerDirector> employerDirectors { get; set; }
        IDbSet<employmentType> employmentTypes { get; set; }
        IDbSet<financialType> financialTypes { get; set; }
        IDbSet<genericCheckList> genericCheckLists { get; set; }
        IDbSet<group> groups { get; set; }
        IDbSet<groupCategory> groupCategories { get; set; }
        IDbSet<groupExec> groupExecs { get; set; }
        IDbSet<idNo> idNoes { get; set; }
        IDbSet<idNoType> idNoTypes { get; set; }
        IDbSet<incentiveStructure> incentiveStructures { get; set; }
        IDbSet<industry> industries { get; set; }
        IDbSet<institution> institutions { get; set; }
        IDbSet<insuranceSetup> insuranceSetups { get; set; }
        IDbSet<interestType> interestTypes { get; set; }
        IDbSet<investment> investments { get; set; }
        IDbSet<investmentAdditional> investmentAdditionals { get; set; }
        IDbSet<investmentCharge> investmentCharges { get; set; }
        IDbSet<investmentInterest> investmentInterests { get; set; }
        IDbSet<investmentSchedule> investmentSchedules { get; set; }
        IDbSet<investmentSignatory> investmentSignatories { get; set; }
        IDbSet<investmentType> investmentTypes { get; set; }
        IDbSet<investmentWithdrawal> investmentWithdrawals { get; set; }
        IDbSet<invoiceLoan> invoiceLoans { get; set; }
        IDbSet<invoiceLoanConfig> invoiceLoanConfigs { get; set; }
        IDbSet<invoiceLoanMaster> invoiceLoanMasters { get; set; }
        IDbSet<lienReason> lienReasons { get; set; }
        IDbSet<lienReleaseReason> lienReleaseReasons { get; set; }
        IDbSet<lineOfBusiness> lineOfBusinesses { get; set; }
        IDbSet<ln_type> ln_type { get; set; }
        IDbSet<loanCheck> loanChecks { get; set; }
        IDbSet<loanCheckList> loanCheckLists { get; set; }
        IDbSet<loanClosure> loanClosures { get; set; }
        IDbSet<loanClosureReason> loanClosureReasons { get; set; }
        IDbSet<loanCollateral> loanCollaterals { get; set; }
        IDbSet<loanConfig> loanConfigs { get; set; }
        IDbSet<loanDocument> loanDocuments { get; set; }
        IDbSet<loanFee> loanFees { get; set; }
        IDbSet<loanFeeType> loanFeeTypes { get; set; }
        IDbSet<loanFinancial> loanFinancials { get; set; }
        IDbSet<loanGroupClient> loanGroupClients { get; set; }
        IDbSet<loanGurantor> loanGurantors { get; set; }
        IDbSet<loanIncentive> loanIncentives { get; set; }
        IDbSet<loanInsurance> loanInsurances { get; set; }
        IDbSet<loanIterestWriteOff> loanIterestWriteOffs { get; set; }
        IDbSet<loanProduct> loanProducts { get; set; }
        IDbSet<loanProductHistory> loanProductHistories { get; set; }
        IDbSet<loanProvision> loanProvisions { get; set; }
        IDbSet<loanPurpose> loanPurposes { get; set; }
        IDbSet<loanPurposeDetail> loanPurposeDetails { get; set; }
        IDbSet<loanRepayment> loanRepayments { get; set; }
        IDbSet<loanScheme> loanSchemes { get; set; }
        IDbSet<loanStatu> loanStatus { get; set; }
        IDbSet<loanTemplate> loanTemplates { get; set; }
        IDbSet<loanTranch> loanTranches { get; set; }
        IDbSet<loanType> loanTypes { get; set; }
        IDbSet<maritalStatu> maritalStatus { get; set; }
        IDbSet<microBusinessCategory> microBusinessCategories { get; set; }
        IDbSet<modeOfEntry> modeOfEntries { get; set; }
        IDbSet<modeOfPayment> modeOfPayments { get; set; }
        IDbSet<multiPayment> multiPayments { get; set; }
        IDbSet<multiPaymentClient> multiPaymentClients { get; set; }
        IDbSet<nextOfKin> nextOfKins { get; set; }
        IDbSet<notification> notifications { get; set; }
        IDbSet<notificationRecipient> notificationRecipients { get; set; }
        IDbSet<notificationSchedule> notificationSchedules { get; set; }
        IDbSet<ownerShipType> ownerShipTypes { get; set; }
        IDbSet<penaltyType> penaltyTypes { get; set; }
        IDbSet<phone> phones { get; set; }
        IDbSet<phoneType> phoneTypes { get; set; }
        IDbSet<prAllowance> prAllowances { get; set; }
        IDbSet<prAllowanceType> prAllowanceTypes { get; set; }
        IDbSet<prLoanDetail> prLoanDetails { get; set; }
        IDbSet<productCode> productCodes { get; set; }
        IDbSet<provisionBatch> provisionBatches { get; set; }
        IDbSet<provisionClass> provisionClasses { get; set; }
        IDbSet<region> regions { get; set; }
        IDbSet<regularSusuAccount> regularSusuAccounts { get; set; }
        IDbSet<regularSusuContribution> regularSusuContributions { get; set; }
        IDbSet<regularSusuContributionSchedule> regularSusuContributionSchedules { get; set; }
        IDbSet<regularSusuWithdrawal> regularSusuWithdrawals { get; set; }
        IDbSet<repaymentMode> repaymentModes { get; set; }
        IDbSet<repaymentSchedule> repaymentSchedules { get; set; }
        IDbSet<repaymentType> repaymentTypes { get; set; }
        IDbSet<reservationType> reservationTypes { get; set; }
        IDbSet<savingAdditional> savingAdditionals { get; set; }
        IDbSet<savingBalance> savingBalances { get; set; }
        IDbSet<savingCharge> savingCharges { get; set; }
        IDbSet<savingConfig> savingConfigs { get; set; }
        IDbSet<savingDailyInterest> savingDailyInterests { get; set; }
        IDbSet<savingInterest> savingInterests { get; set; }
        IDbSet<savingLienRelease> savingLienReleases { get; set; }
        IDbSet<savingNextOfKin> savingNextOfKins { get; set; }
        IDbSet<savingPlan> savingPlans { get; set; }
        IDbSet<savingPlanFlag> savingPlanFlags { get; set; }
        IDbSet<savingPlanInterval> savingPlanIntervals { get; set; }
        IDbSet<savingReservationTransc> savingReservationTranscs { get; set; }
        IDbSet<savingRollOver> savingRollOvers { get; set; }
        IDbSet<savingSchedule> savingSchedules { get; set; }
        IDbSet<savingSignatory> savingSignatories { get; set; }
        IDbSet<savingType> savingTypes { get; set; }
        IDbSet<savingWithdrawal> savingWithdrawals { get; set; }
        IDbSet<sector> sectors { get; set; }
        IDbSet<smeCategory> smeCategories { get; set; }
        IDbSet<smeDirector> smeDirectors { get; set; }
        IDbSet<SpecialDay> SpecialDays { get; set; }
        IDbSet<SpecialDayType> SpecialDayTypes { get; set; }
        IDbSet<staffCategoryDirector> staffCategoryDirectors { get; set; }
        IDbSet<staffSaving> staffSavings { get; set; }
        IDbSet<supplier> suppliers { get; set; }
        IDbSet<supplierContact> supplierContacts { get; set; }
        IDbSet<susuAccount> susuAccounts { get; set; }
        IDbSet<susuConfig> susuConfigs { get; set; }
        IDbSet<susuContribution> susuContributions { get; set; }
        IDbSet<susuContributionSchedule> susuContributionSchedules { get; set; }
        IDbSet<susuGradePosition> susuGradePositions { get; set; }
        IDbSet<susuGroup> susuGroups { get; set; }
        IDbSet<susuGroupHistory> susuGroupHistories { get; set; }
        IDbSet<susuPosition> susuPositions { get; set; }
        IDbSet<susuScheme> susuSchemes { get; set; }
        IDbSet<sv_type> sv_type { get; set; }
        IDbSet<systemDate> systemDates { get; set; }
        IDbSet<tenor> tenors { get; set; }
        IDbSet<tenureType> tenureTypes { get; set; }
        IDbSet<transactionType> transactionTypes { get; set; }
        IDbSet<loanPenaltyDisable> loanPenaltyDisables { get; set; }
        IDbSet<loanPenaltyEnable> loanPenaltyEnables { get; set; }
        IDbSet<staffCategory> staffCategories { get; set; }
        IDbSet<bankAccountType> bankAccountTypes { get; set; }
        IDbSet<borrowing> borrowings { get; set; }
        IDbSet<borrowingDisbursement> borrowingDisbursements { get; set; }
        IDbSet<borrowingDocument> borrowingDocuments { get; set; }
        IDbSet<borrowingFee> borrowingFees { get; set; }
        IDbSet<borrowingFeeType> borrowingFeeTypes { get; set; }
        IDbSet<borrowingPenalty> borrowingPenalties { get; set; }
        IDbSet<borrowingRepayment> borrowingRepayments { get; set; }
        IDbSet<borrowingRepaymentSchedule> borrowingRepaymentSchedules { get; set; }
        IDbSet<borrowingType> borrowingTypes { get; set; }
        IDbSet<loanAdditionalInfo> loanAdditionalInfoes { get; set; }
        IDbSet<loanApproval> loanApprovals { get; set; }
        IDbSet<loanApprovalStage> loanApprovalStages { get; set; }
        IDbSet<loanApprovalStageOfficer> loanApprovalStageOfficers { get; set; }
        IDbSet<loanDocumentPlaceHolderType> loanDocumentPlaceHolderTypes { get; set; }
        IDbSet<loanDocumentTemplate> loanDocumentTemplates { get; set; }
        IDbSet<loanDocumentTemplatePage> loanDocumentTemplatePages { get; set; }
        IDbSet<loanDocumentTemplatePagePlaceHolder> loanDocumentTemplatePagePlaceHolders { get; set; }
        IDbSet<metaDataType> metaDataTypes { get; set; }
        IDbSet<accountsSystemDateChange> accountsSystemDateChanges { get; set; }
        IDbSet<clientInvestmentReceipt> clientInvestmentReceipts { get; set; }
        IDbSet<currencyNoteType> currencyNoteTypes { get; set; }
        IDbSet<defaultAction> defaultActions { get; set; }
        IDbSet<depositAuthorization> depositAuthorizations { get; set; }
        IDbSet<depositNextOfKin> depositNextOfKins { get; set; }
        IDbSet<depositSystemDateChange> depositSystemDateChanges { get; set; }
        IDbSet<employeeDepartment> employeeDepartments { get; set; }
        IDbSet<loanGroupDay> loanGroupDays { get; set; }
        IDbSet<loanMetaData> loanMetaDatas { get; set; }
        IDbSet<privateCompanyStaffAddress> privateCompanyStaffAddresses { get; set; }
        IDbSet<privateCompanyStaffVerification> privateCompanyStaffVerifications { get; set; }
        IDbSet<relationshipType> relationshipTypes { get; set; }
        IDbSet<savingSystemDateChange> savingSystemDateChanges { get; set; }
        IDbSet<susuSystemDateChange> susuSystemDateChanges { get; set; }
        IDbSet<treasuryBillRate> treasuryBillRates { get; set; }
        IDbSet<creditUnionSystemDateChange> creditUnionSystemDateChanges { get; set; }
        IDbSet<investmentSystemDateChange> investmentSystemDateChanges { get; set; }
        IDbSet<loan> loans { get; set; }
        IDbSet<loanSystemDateChange> loanSystemDateChanges { get; set; }
        IDbSet<privateCompanyStaff> privateCompanyStaffs { get; set; }
        IDbSet<cashiersTillDay> cashiersTillDays { get; set; }
        IDbSet<staffCategory1> staffCategory1 { get; set; }
        IDbSet<loanGroup> loanGroups { get; set; }
        IDbSet<depositRateUpgrade> depositRateUpgrades { get; set; }
        IDbSet<document> documents { get; set; }
        IDbSet<image> images { get; set; }
        IDbSet<savingLien> savingLiens { get; set; }
        IDbSet<cashierCashup> cashierCashups { get; set; }
        IDbSet<cashierFund> cashierFunds { get; set; }
        IDbSet<id_prof> id_prof { get; set; }
        IDbSet<chargeTypeTier> chargeTypeTiers { get; set; }
        IDbSet<overTimeConfig> overTimeConfigs { get; set; }
        IDbSet<staffBenefit> staffBenefits { get; set; }
        IDbSet<staff> staffs { get; set; }
        IDbSet<vwOutstandingLoan> vwOutstandingLoans { get; set; }
        IDbSet<staffOvertime> staffOvertimes { get; set; }
        IDbSet<controllerRemark> controllerRemarks { get; set; }
        IDbSet<controllerRepaymentType> controllerRepaymentTypes { get; set; }
        IDbSet<loanNo_by_staffID> loanNo_by_staffID { get; set; }
        IDbSet<vw_controllerFile_outstandingLoan> vw_controllerFile_outstandingLoan { get; set; }
        IDbSet<chargeType> chargeTypes { get; set; }
        IDbSet<loanPenalty> loanPenalties { get; set; }
        IDbSet<saving> savings { get; set; }
        IDbSet<cashierTillConfig> cashierTillConfigs { get; set; }
        IDbSet<cashierTillConfigDetail> cashierTillConfigDetails { get; set; }
        IDbSet<vwActiveClient> vwActiveClients { get; set; }
        IDbSet<vwFlaggedClient> vwFlaggedClients { get; set; }
        IDbSet<cashierTransactionReceipt> cashierTransactionReceipts { get; set; }
        IDbSet<cashierTransactionReceiptCurrency> cashierTransactionReceiptCurrencies { get; set; }
        IDbSet<cashierTransactionWithdrawal> cashierTransactionWithdrawals { get; set; }
        IDbSet<cashierTransactionWithdrawalCurrency> cashierTransactionWithdrawalCurrencies { get; set; }
        IDbSet<clientActivityLog> clientActivityLogs { get; set; }
        IDbSet<clientInvestmentReceiptDetail> clientInvestmentReceiptDetails { get; set; }
        IDbSet<controllerFileDetail> controllerFileDetails { get; set; }
        IDbSet<depositUpgrade> depositUpgrades { get; set; }
        IDbSet<susuGrade> susuGrades { get; set; }
        IDbSet<currencyNote> currencyNotes { get; set; }
    
         int initAccounts();
         
    
         int initClient();
         
    
         int initJournal();
         
    
         int initLoan();
         
    
         int initAsset();
         
    
         int initStaff();
         
    
         ObjectResult<getCollection_Result> getCollection(Nullable<int> month);
         
    
         int getDailyTransactionReportByBranch(Nullable<int> branchId, Nullable<System.DateTime> transactiondate);
         
    
         int getInvestmentReport(Nullable<System.DateTime> startDate, Nullable<System.DateTime> endDate);
         
    
         ObjectResult<getLoanBalanceByProduct_Result> getLoanBalanceByProduct(Nullable<System.DateTime> startDate, Nullable<System.DateTime> endDate);
         
    
         ObjectResult<string> sp_attempt_deposit(Nullable<int> savingId, Nullable<double> savingAmount, string reservedBy, Nullable<System.DateTime> savingDate, Nullable<int> bankId, string checkNo, Nullable<int> modeOfPaymentId, string naration, string transactionId, ObjectParameter savingAdditionalId);
         
    
         ObjectResult<string> sp_attempt_reservation(Nullable<int> savingId, Nullable<double> amount, Nullable<int> reservationTypeId, string reservedBy, string naration, ObjectParameter transactionId);
         
    
         ObjectResult<string> sp_withdraw_fund(Nullable<int> savingId, Nullable<double> interestWithdrawal, Nullable<double> principalWithdrawal, string reservedBy, Nullable<System.DateTime> withdrawalDate, Nullable<int> bankId, string checkNo, Nullable<int> modeOfPaymentId, string naration, string transactionId);
         
    }
}