use coreDB
go

alter table gl.pc_dtl add
	constraint fk_pc_dtl_accts foreign key (acct_id)
	references accts(acct_id) on update cascade 
go
	 
alter table gl.pc_dtl add
	constraint fk_pc_dtl_pc_head foreign key (pc_head_id)
	references gl.pc_head(pc_head_id) on update cascade 
go
	 