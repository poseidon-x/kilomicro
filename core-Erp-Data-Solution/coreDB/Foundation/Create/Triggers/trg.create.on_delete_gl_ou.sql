use coreDB
go
 
CREATE Trigger on_delete_gl_ou ON gl_ou 
with encryption 

FOR DELETE AS 

	delete gl_ou
	where parent_ou_id in
	(
		select
			ou_id
		from deleted
	)

GO

