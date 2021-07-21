use coreDb
go

alter table ln.salaryLoanConfig add constraint fk_salaryLoanConfig_employer 
foreign key (employerId) references ln.employer (employerID)
go

alter table ln.salaryLoan add constraint fk_salaryLoan_client
foreign key (clientId) references ln.client (clientId)
go

alter table ln.salaryLoan add constraint fk_salaryLoan_loan
foreign key (loanId) references ln.loan (loanId)
go

alter table ln.salaryLoan add constraint fk_salaryLoan_employer
foreign key (employerId) references ln.employer (employerId)
go

alter table ln.salaryLoan add constraint fk_salaryLoan_salaryLoanConfig
foreign key (salaryLoanConfigId) references ln.salaryLoanConfig (salaryLoanConfigId)
go

alter table ln.salaryLoan add constraint fk_salaryLoan_employerDirector
foreign key (approvingDirectorId) references ln.employerDirector (employerDirectorId)
go


