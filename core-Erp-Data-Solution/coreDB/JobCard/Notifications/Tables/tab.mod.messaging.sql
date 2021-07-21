use coreDB
go

alter table msg.messagingConfig add
	maxMessageLength smallint not null default(157) check(maxMessageLength>0 and maxMessageLength<400),
	maxNarationLength tinyint not null default (25) check(maxNarationLength>0 and maxNarationLength<80)
go

alter table msg.messagingConfig add
	loanRepaymentNotificationCycle smallint not null default(30) check(loanRepaymentNotificationCycle>0 and loanRepaymentNotificationCycle<60)
go

alter table msg.messagingConfig add
	numberOfDaysBeforeLoanOverdue smallint not null default(30) check(numberOfDaysBeforeLoanOverdue>0 and numberOfDaysBeforeLoanOverdue<60)
go



