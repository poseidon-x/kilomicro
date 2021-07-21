
--correct--


alter table ln.cashierTransactionReceiptCurrency add totalDB float not null default (0);
alter table ln.cashierTransactionWithdrawalCurrency add totalDB float not null default (0);
alter table ln.cashierTransactionReceiptCurrency add totalCD float not null default (0);
alter table ln.cashierTransactionWithdrawalCurrency add totalCD float not null default (0);
alter table ln.cashierTransactionReceiptCurrency add quantityDB float not null default (0);
alter table ln.cashierTransactionWithdrawalCurrency add quantityDB float not null default (0);
alter table ln.cashierTransactionReceiptCurrency add quantityCD float not null default (0);
alter table ln.cashierTransactionWithdrawalCurrency add quantityCD float not null default (0);

alter table ln.clientInvestmentReceiptDetail add paymentModeId int not null default (0) references ln.modeOfPayment(modeOfPaymentID);
alter table ln.clientActivityLog add notificationSentDate datetime ;
alter table ln.clientActivityLog add notificationSent bit not null default (0);
alter table ln.depositUpgrade add interestRate float not null default (0);
alter table ln.susuGrade add interestDeductedPerMonth float not null default (0);
alter table ln.currencyNote add currencyNoteTypeId int references ln.currencyNoteType(currencyNoteTypeId);
alter table ln.clientInvestmentReceiptDetail add created datetime not null;
alter table ln.controllerFileDetail add controllerRepaymentTypeId nvarchar(30);
alter table ln.clientInvestmentReceiptDetail add chequeNumber nvarchar(50);
alter table ln.clientInvestmentReceiptDetail add bankId int;
alter table ln.cashierTransactionReceipt add balanceCD float not null default (0);
alter table ln.cashierTransactionWithdrawal add balanceCD float not null default (0);
alter table ln.cashierTransactionReceipt add balanceBD float not null default (0);
alter table ln.cashierTransactionWithdrawal add balanceBD float not null default (0);


--correct end


alter table ln.depositType add upgradeMinimumAmount float not null default 0;

alter table ln.depositType add upgradeable bit not null default 0;

alter table ln.config add transactionalBankingEnabled bit not null default 0;



alter table ln.depositAdditional add balanceBD float not null default (0);

alter table ln.depositWithdrawal add balanceBD float not null default (0);

alter table ln.branch add territoryId int not null default 0;

alter table ln.depositConfig add staffDepositInterestRate float not null default (0);

alter table ln.loanProvision add revisionJournalDrId int not null default 0;

alter table ln.loanProvision add revisionJournalCrId int not null default 0;


alter table ln.loan add repaymentAccountId int;


alter table ln.loanProvision add provisionJournalDrId int;

alter table ln.loanProvision add provisionJournalCrId int;

alter table ln.loanProvision add provisionBatchId int references ln.provisionBatch(provisionBatchId);

alter table ln.savingWithdrawal add principalBalanceBD float not null default (0);

alter table ln.loan add penaltyDisabled bit not null default 0;

alter table ln.depositType add ordinal int not null default 0;

alter table ln.loanCheckList add modifier nvarchar(100);

alter table ln.loanCheckList add modified datetime;

alter table ln.loanPenalty add modeOfPaymentId int ;

alter table ln.savingType add minimumBalance float not null default (0);

alter table ln.deposit add maturityNotificationSent bit not null default 0;

alter table ln.config add loanPenaltyGracePeriodInDays int not null default 0;

alter table ln.depositAdditional add lastInterestDate datetime;

alter table ln.depositAdditional add isDisInvestment bit;

alter table ln.depositType add isCompoundInterest bit not null default 0;

alter table ln.depositUpgrade add interestRate float not null default (0);

alter table ln.savingWithdrawal add interestBalanceBD float not null default (0);

alter table ln.chargeType add fixed bit not null default 0;

alter table ln.depositConfig add disInvestmentRate float not null default (0);

alter table ln.depositWithdrawal add disInvestmentCharge float not null default (0);

alter table ln.deposit add depositPeriodInDays int;

alter table ln.depositType add depositInterestUpgradeable bit not null default 0;

alter table ln.cashiersTill add currentBalance float not null default (0);



alter table ln.deposit add closed bit not null default 0;

alter table ln.client add clientMandateTypeId int references ln.clientMandateType(clientMandateTypeId);

alter table ln.depositConfig add clientDepositBySav bit not null default 0;

alter table ln.chargeType add chargeDefaultAmount float not null default (0);

alter table hc.staffBenefit add bankSortCode nvarchar(10);

alter table ln.cashierTransactionReceipt add balanceCD float not null default (0);

alter table ln.cashierTransactionWithdrawal add balanceCD float not null default (0);

alter table ln.depositAdditional add balanceBD float not null default (0);

alter table ln.depositWithdrawal add balanceBD float not null default (0);

alter table ln.cashierTransactionReceipt add balanceBD float not null default (0);

alter table ln.cashierTransactionWithdrawal add balanceBD float not null default (0);

alter table ln.savingPlanFlag add appliedBy nvarchar(30);

alter table ln.deposit add annualInterestRate float not null default (0);

alter table ln.depositType add allowAdditionalDeposit bit not null default (0);

alter table ln.client add admissionFee float not null default (0);

alter table ln.address add addressTyeID int;

alter table ln.chargeType add accountsReceivableAccountID int;

alter table ln.chargeType add accountsPayableAccountID int not null references ln.

