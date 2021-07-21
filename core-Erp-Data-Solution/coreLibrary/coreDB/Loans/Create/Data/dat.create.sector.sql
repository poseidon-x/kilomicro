use coreDB
go
set identity_insert ln.sector on 

insert into ln.sector(sectorID, sectorName)
values (1, 'Informal')
insert into ln.sector(sectorID, sectorName)
values (2, 'Formal')
insert into ln.sector(sectorID, sectorName)
values (3, 'Governmental') 