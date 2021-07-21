use coreDB
go

set identity_insert ln.branch on
go

insert into ln.branch(branchID, branchName)
values (1, 'Accra')
insert into ln.branch(branchID, branchName)
values (2, 'Kumasi')
insert into ln.branch(branchID, branchName)
values (3, 'Takoradi')
insert into ln.branch(branchID, branchName)
values (4, 'Tamale')