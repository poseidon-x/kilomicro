use coreDB
go

alter table bank_branches add
	constraint fk_bank_branches foreign key (bank_id)
	references banks(bank_id) on delete cascade on update cascade 
go
	
alter table bank_branches add
	constraint fk_bank_branch_loc foreign key (location_id)
	references locations(location_id) on delete set null on update cascade 
go
	