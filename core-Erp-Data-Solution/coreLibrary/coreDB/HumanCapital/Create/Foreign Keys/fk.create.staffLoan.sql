use coreDB
go

alter table hc.staffLoanTypeLevel
	add constraint fk_staffLoanTypeLevel_loanType foreign key (loanTypeID)
	references hc.staffLoanType (loanTypeID)
go

alter table hc.staffLoanTypeLevel
	add constraint fk_staffLoanTypeLevel_level foreign key (levelID)
	references hc.[level] (levelID)
go

alter table hc.staffLoan
	add constraint fk_staffLoan_loanType foreign key (loanTypeID)
	references hc.staffLoanType (loanTypeID)
go

alter table hc.staffLoan
	add constraint fk_staffLoan_staff foreign key (staffID)
	references fa.staff (staffID)
go

alter table hc.staffLoanSchedule
	add constraint fk_staffLoanSchedule_staffLoan foreign key (staffLoanID)
	references hc.staffLoan (staffLoanID)
go

alter table hc.staffLoanRepayment
	add constraint fk_staffLoanRepayment_staffLoan foreign key (staffLoanID)
	references hc.staffLoan (staffLoanID)
go
