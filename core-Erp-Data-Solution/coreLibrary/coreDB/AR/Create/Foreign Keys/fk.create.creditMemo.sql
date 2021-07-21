use coreDB
go


alter table ar.creditMemo add constraint fk_creditMemo_arInvoice
	foreign key (arInvoiceID) references ar.arInvoice (arInvoiceID)
go

alter table ar.creditMemo add constraint fk_creditMemo_arPayment
	foreign key (arPaymentID) references ar.arPayment (arPaymentID)
go
 
alter table ar.creditMemo add constraint fk_creditMemo_creditMemoReason
	foreign key (creditMemoReasonID) references ar.creditMemoReason (creditMemoReasonID)
go
 
alter table ar.creditMemoLine add constraint fk_creditMemoLine_creditMemo
	foreign key (creditMemoID) references ar.creditMemo (creditMemoID)
go

alter table ar.creditMemoLine add constraint fk_creditMemoLine_arInvoiceLine
	foreign key (arInvoiceLineID) references ar.arInvoiceLine (arInvoiceLineID)
go



