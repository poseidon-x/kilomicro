use coreDB
go

alter table ln.regularSusuAccount add constraint fk_regularSusuAccount_client
	foreign key (clientID) references ln.client (clientID)
go

alter table ln.regularSusuAccount add constraint fk_regularSusuAccount_agent
	foreign key (agentID) references ln.agent (agentID)
go

alter table ln.regularSusuAccount add constraint fk_regularSusuAccount_loan
	foreign key (loanID) references ln.loan (loanID)
go

alter table ln.regularSusuAccount add constraint fk_regularSusuAccount_staff
	foreign key (staffID) references fa.staff (staffID)
go

alter table ln.regularSusuContributionSchedule add constraint fk_regularSusuContributionSchedule_regularSusuAccount
	foreign key (regularSusuAccountID) references ln.regularSusuAccount (regularSusuAccountID)
go

alter table ln.regularSusuContribution add constraint fk_regularSusuContribution_regularSusuAccount
	foreign key (regularSusuAccountID) references ln.regularSusuAccount (regularSusuAccountID)
go

alter table ln.regularSusuContribution add constraint fk_regularSusuContribution_modeOfPayment
	foreign key (modeOfPaymentID) references ln.modeOfPayment (modeOfPaymentID)
go


alter table ln.regularSusuWithdrawal add constraint fk_regularSusuWithdrawal_regularSusuAccount
	foreign key (regularSusuAccountID) references ln.regularSusuAccount (regularSusuAccountID)
go

