use coreDB
go

create table hc.staffPromotion
(
	staffPromotionID int identity(1,1) not null primary key,
	staffID int not null,
	oldLevelID int,
	newLevelID int,
	oldNotchID int,
	newNotchID int,
	promotionDate datetime not null default(getdate()),
	memo ntext,
	oldJobTitleID int,
	newJobTitleID int,
	oldManagerStaffID int,
	newManagerStaffID int,
	promotedBy nvarchar(30),
	creationDate datetime default(getdate())
)
go