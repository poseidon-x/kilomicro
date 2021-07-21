use coreDB
go
set identity_insert ln.industry on 

insert into ln.industry(industryID, industryName)
values (1, 'Agricultural')
insert into ln.industry(industryID, industryName)
values (2, 'Banking')
insert into ln.industry(industryID, industryName)
values (3, 'Transportation') 