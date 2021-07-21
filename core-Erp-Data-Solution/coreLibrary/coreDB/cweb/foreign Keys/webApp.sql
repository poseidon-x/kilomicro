use coreDB
go

alter table webAppID add
	constraint fk_webAppID_webApp foreign key (webAppID)
	references webApp (webAppID)
go

alter table webAppPhoto add
	constraint fk_webAppPhoto_webApp foreign key (webAppID)
	references webApp (webAppID)
go

alter table webAppPhyAddr add
	constraint fk_webAppPhyAddr_webApp foreign key (webAppID)
	references webApp (webAppID)
go

alter table webAppMailAddr add
	constraint fk_webAppMailAddr_webApp foreign key (webAppID)
	references webApp (webAppID)
go

alter table webAppContact add
	constraint fk_webAppContact_webApp foreign key (webAppID)
	references webApp (webAppID)
go

alter table webAppEmp add
	constraint fk_webAppEmp_webApp foreign key (webAppID)
	references webApp (webAppID)
go

alter table webAppEmp2 add
	constraint fk_webAppEmp2_webApp foreign key (webAppID)
	references webApp (webAppID)
go

alter table webAppBank add
	constraint fk_webAppBank_webApp foreign key (webAppID)
	references webApp (webAppID)
go

alter table webAppSalary add
	constraint fk_webAppSalary_webApp foreign key (webAppID)
	references webApp (webAppID)
go