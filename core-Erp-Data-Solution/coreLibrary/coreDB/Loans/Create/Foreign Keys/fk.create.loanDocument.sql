﻿use coreDB
go

alter table ln.loanDocumentTemplatePage
	add constraint fk_loanDocumentTemplatePage_loanDocumentTemplate foreign key(loanDocumentTemplateId)
	references ln.loanDocumentTemplate(loanDocumentTemplateId)

alter table ln.loanDocumentTemplatePagePlaceHolder
	add constraint fk_loanDocumentTemplatePagePlaceHolder_loanDocumentTemplatePage foreign key(loanDocumentTemplatePageId)
	references ln.loanDocumentTemplatePage(loanDocumentTemplatePageId)

alter table ln.loanDocumentTemplatePagePlaceHolder
	add constraint fk_loanDocumentTemplatePagePlaceHolder_loanDocumentPlaceHolderType foreign key(placeHolderTypeId)
	references ln.loanDocumentPlaceHolderType(loanDocumentPlaceHolderTypeId)