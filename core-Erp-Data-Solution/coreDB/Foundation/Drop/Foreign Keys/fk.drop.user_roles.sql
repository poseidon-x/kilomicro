use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_user_roles_user')
	BEGIN
		alter table user_roles DROP  constraint fk_user_roles_user
	END
GO
 
IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_user_roles_role')
	BEGIN
		alter table user_roles DROP  constraint fk_user_roles_role
	END
GO
