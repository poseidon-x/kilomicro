use coreDB
go

create index ix_modules_parent on dbo.modules
(
	parent_module_id asc
)
go
