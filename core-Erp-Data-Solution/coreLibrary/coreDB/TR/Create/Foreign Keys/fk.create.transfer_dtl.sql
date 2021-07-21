use coreDB
go

alter table tr.transfer_dtl add
	constraint fk_transfer_dtl_districts foreign key (district_id)
	references tr.district(district_id) on update cascade 
go

alter table tr.transfer_dtl add
	constraint fk_transfer_dtl_tranfers foreign key (transfer_id)
	references tr.transfer(transfer_id) on update cascade 
go