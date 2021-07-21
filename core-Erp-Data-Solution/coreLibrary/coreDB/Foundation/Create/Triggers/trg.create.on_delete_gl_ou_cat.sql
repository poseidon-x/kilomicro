use coreDB
go
 
CREATE Trigger on_delete_gl_ou_cat ON gl_ou_cat 
with encryption 

FOR DELETE AS 

	delete gl_ou_cat
	where parent_ou_cat_id in
	(
		select
			ou_cat_id
		from deleted
	)

GO

