﻿use coreDB
go

CREATE View vw_ou
with encryption  AS

	SELECT     TOP (100) PERCENT cat_name, ou_name,  
                      dbo.ou_walk(o.ou_id, dbo.ou_level(o.ou_id, 0) - 1, o.ou_id, 0,'') AS ou_name1, dbo.ou_walk(o.ou_id, 
                      dbo.ou_level(o.ou_id, 0) - 2, o.ou_id, 0,'') AS ou_name2, dbo.ou_walk(o.ou_id, 
                      dbo.ou_level(o.ou_id, 0) - 3, o.ou_id, 0,'') AS ou_name3, dbo.ou_walk(o.ou_id, 
                      dbo.ou_level(o.ou_id, 0) - 4, o.ou_id, 0,'') AS ou_name4, dbo.ou_walk(o.ou_id, 
                      dbo.ou_level(o.ou_id, 0) - 5, o.ou_id, 0,'') AS head_nam5, dbo.ou_walk(o.ou_id, dbo.ou_level(o.ou_id, 
                      0) - 6, o.ou_id, 0,'') AS ou_name6, dbo.ou_walk(o.ou_id, dbo.ou_level(o.ou_id, 0) - 7, o.ou_id, 0,'') 
                      AS ou_name7  , ou_id, c.ou_cat_id,
					    isnull(o.companyId,0)as companyId ,
					  isnull(com.comp_name,'') as companyName
FROM         dbo.ou AS o INNER JOIN
                      dbo.ou_cat AS c ON o.ou_cat_id = c.ou_cat_id 
					 left outer join dbo.comp_prof AS com ON com.companyId = o.companyId

ORDER BY cat_name, parent_ou_id, ou_name 


GO
 