use coreDB
go

IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_user_perms_perm')
	BEGIN
		alter table user_perms DROP  constraint fk_user_perms_perm
	END
GO
  
IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_user_perms_user')
	BEGIN
		alter table user_perms DROP  constraint fk_user_perms_user
	END
GO
  
IF EXISTS (SELECT * FROM sysobjects WHERE name = 'fk_user_perms_module')
	BEGIN
		alter table user_perms DROP  constraint fk_user_perms_module
	END
GO
  