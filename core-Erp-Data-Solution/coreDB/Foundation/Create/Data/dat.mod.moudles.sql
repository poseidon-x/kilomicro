use coreDB
go

update modules set parent_module_id=29
where module_id=1099
go

update modules set parent_module_id=29
where module_id=74
go

update modules set parent_module_id=74, sort_value=99
where module_id=2121
go

update modules set parent_module_id=69, sort_value=99
where module_id=2129
go

update modules set module_name='Cashier Interface'
where module_id=74
go

update modules set parent_module_id=29, sort_value=11
where module_id=34
go
 
update modules set parent_module_id=13333
where module_id in (82, 93,-6,2129)
go

update modules set parent_module_id=13334
where module_id in (1108, 1107)
go

USE [coreDB]
GO

set identity_insert modules on
	INSERT INTO [dbo].[modules]
           (module_id, [module_name]
           ,[url]
           ,[parent_module_id]
           ,[creation_date]
           ,[creator] 
           ,[sort_value]
           ,[visible]
           ,[module_code])
     VALUES
           (2270,
		   'Savings &amp; Current Accounts'
           ,'/'
           ,29
           ,getdate()
           ,'KOFI' 
           ,25
           ,1
           ,''
		   )
	INSERT INTO [dbo].[modules]
           (module_id, [module_name]
           ,[url]
           ,[parent_module_id]
           ,[creation_date]
           ,[creator] 
           ,[sort_value]
           ,[visible]
           ,[module_code])
     VALUES
           (2271,
		   'Client Investments &amp; Deposits'
           ,'/'
           ,29
           ,getdate()
           ,'KOFI' 
           ,26
           ,1
           ,''
		   ) 
GO

set identity_insert modules off

update modules set parent_module_id=2271
where module_id in (97, 193)

update modules set parent_module_id=2270
where module_id in (3193, 3097)

update modules set module_name='Account Management'
where module_id in (97)

update modules set module_name='Account Reports'
where module_id in (193)

update modules set module_name='Account Reports'
where module_id in (3193)

update modules set module_name='Account Management'
where module_id in (3097)

update modules set parent_module_id=12190
where module_id in (197)

update modules set module_name='Our Investments',
	sort_value=29
where module_id in (12190)
go

update modules set module_name='Susu Schemes', parent_module_id=null, sort_value=51
where module_id=13343
go

set identity_insert modules on
	INSERT INTO [dbo].[modules]
           (module_id, [module_name]
           ,[url]
           ,[parent_module_id]
           ,[creation_date]
           ,[creator] 
           ,[sort_value]
           ,[visible]
           ,[module_code])
     VALUES
           (14360,
		   'Susu Scheme Reports'
           ,'/'
           ,13343
           ,getdate()
           ,'KOFI' 
           ,5
           ,1
           ,''
		   )
set identity_insert modules off

update dbo.modules set url=replace(url, 'analysis', 'reports')
where url like '%susuAccountStages%'