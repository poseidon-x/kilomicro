use coreDB
go
 
CREATE Trigger on_delete_ou ON ou 
with encryption 

FOR DELETE AS 

	delete ou
	where parent_ou_id in
	(
		select
			ou_id
		from deleted
	)

GO

