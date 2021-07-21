use coreDB
go

alter table ln.loanAdditionalInfo add constraint
	fk_additionalInfo_loan foreign key(loanId)
	references ln.loan(loanID)

alter table ln.loanMetaData add constraint
	fk_loanMetaData_additionalInfo foreign key(loanAdditionalInfoId)
	references ln.loanAdditionalInfo(loanAdditionalInfoId)

alter table ln.loanMetaData add constraint
	fk_loanMetaData_metaDataType foreign key(metaDataTypeId)
	references ln.metaDataType(metaDataTypeId)
