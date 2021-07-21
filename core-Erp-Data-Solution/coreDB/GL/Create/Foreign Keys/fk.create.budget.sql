use coreDB
go

alter table gl.budget add
	constraint fk_budget_account foreign key (acct_id)
	references dbo.accts (acct_id)
go

alter table gl.budget add
	constraint fk_budget_cost_center foreign key (cost_center_id)
	references dbo.gl_ou (ou_id)
go
