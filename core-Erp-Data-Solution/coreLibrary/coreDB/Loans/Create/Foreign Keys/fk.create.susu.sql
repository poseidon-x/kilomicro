use coreDB
go

alter table ln.susuConfig add constraint fk_susuConfig_susuScheme
	foreign key (susuSchemeID) references ln.susuScheme (susuSchemeID)
go

alter table ln.susuAccount add constraint fk_susuAccount_client
	foreign key (clientID) references ln.client (clientID)
go

alter table ln.susuAccount add constraint fk_susuAccount_susuGrade
	foreign key (susuGradeID) references ln.susuGrade (susuGradeID)
go

alter table ln.susuAccount add constraint fk_susuAccount_susuGroup
	foreign key (susuGroupID) references ln.susuGroup (susuGroupID)
go

alter table ln.susuAccount add constraint fk_susuAccount_susuPosition
	foreign key (susuPositionID) references ln.susuPosition (susuPositionID)
go

alter table ln.susuAccount add constraint fk_susuAccount_agent
	foreign key (agentID) references ln.agent (agentID)
go

alter table ln.susuAccount add constraint fk_susuAccount_loan
	foreign key (loanID) references ln.loan (loanID)
go

alter table ln.susuAccount add constraint fk_susuAccount_staff
	foreign key (staffID) references fa.staff (staffID)
go

alter table ln.susuContributionSchedule add constraint fk_susuContributionSchedule_susuAccount
	foreign key (susuAccountID) references ln.susuAccount (susuAccountID)
go

alter table ln.susuContribution add constraint fk_susuContribution_susuAccount
	foreign key (susuAccountID) references ln.susuAccount (susuAccountID)
go

alter table ln.susuContribution add constraint fk_susuContribution_modeOfPayment
	foreign key (modeOfPaymentID) references ln.modeOfPayment (modeOfPaymentID)
go

alter table ln.susuGroupHistory add constraint fk_susuGroupHistory_susuGroup
	foreign key (susuGroupID) references ln.susuGroup (susuGroupID)
go

