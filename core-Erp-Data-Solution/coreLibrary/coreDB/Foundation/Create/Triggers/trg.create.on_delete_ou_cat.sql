use coreDB
go
 
CREATE Trigger on_delete_ou_cat ON ou_cat 
with encryption 

FOR DELETE AS 

	delete ou_cat
	where parent_ou_cat_id in
	(
		select
			ou_cat_id
		from deleted
	)

GO

