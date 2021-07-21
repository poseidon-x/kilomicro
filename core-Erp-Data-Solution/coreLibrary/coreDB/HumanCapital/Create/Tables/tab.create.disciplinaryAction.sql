use coreDB
go

create table hc.misconductType
(
	misconductTypeID int identity (1,1) not null primary key,
	misconductTypeName nvarchar(200) not null constraint uk_misconductType_name unique
)
go

create table hc.misconductSeverity
(
	misconductSeverityID int not null primary key,
	misconductSeverityName nvarchar(200) not null constraint uk_misconductSeverity_name unique
)
go

insert into hc.misconductSeverity (misconductSeverityID, misconductSeverityName)
values (1, 'Mild - Not Serious')
insert into hc.misconductSeverity (misconductSeverityID, misconductSeverityName)
values (2, 'Moderate - Minimal Impact')
insert into hc.misconductSeverity (misconductSeverityID, misconductSeverityName)
values (3, 'Severe - Significant Impact')
insert into hc.misconductSeverity (misconductSeverityID, misconductSeverityName)
values (4, 'Extreme - Fatal or Huge Financial Impact')

create table hc.disciplinaryAction
(
	disciplinaryActionID int identity (1,1) not null primary key,
	disciplinaryActionName nvarchar(200) not null constraint uk_disciplinaryAction_name unique
)
go

create table hc.staffMisconduct
(
	staffMisconductID int identity (1,1) not null primary key,
	staffID int not null,
	misconductTypeID int not null,
	incidentDate datetime not null,
	incidentDescription ntext,
	enteredBy nvarchar(30) not null,
	dateEntered datetime not null,
	disciplinaryActionID int,
	misconductSeverityID int not null,
	financialImpact float not null default(0)
)
go