use coreDB
go

alter table tr.district_acct add
	constraint fk_district_acct_districts foreign key (district_id)
	references tr.district(district_id) on update cascade 
go
