use coreDB
go

insert into ln.idNoType(idNoTypeID, idNoTypeName)
values (1, 'Passport')
insert into ln.idNoType(idNoTypeID, idNoTypeName)
values (2, 'Voters ID')
insert into ln.idNoType(idNoTypeID, idNoTypeName)
values (3, 'National ID')
insert into ln.idNoType(idNoTypeID, idNoTypeName)
values (4, 'Driver''s License')
update ln.idNoType set isNational=0 where idNoTypeID=5

insert into ln.idNoType(idNoTypeID, idNoTypeName, isNational)
values (6, 'Professional ID', 0)