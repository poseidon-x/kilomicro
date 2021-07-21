use coreDB
go

alter table ar.arPayment add constraint fk_arPayment_currency 
	foreign key (currencyId) references dbo.currencies (currency_id)
go

alter table ar.arPayment add constraint fk_arPayment_paymentMethod
	foreign key (paymentMethodID) references so.paymentMethod (paymentMethodID)
go

alter table ar.arPaymentLine add constraint fk_arPaymentLine_arPayment
	foreign key (arPaymentId) references ar.arPayment (arPaymentId)
go

alter table ar.arPaymentLine add constraint fk_arPaymentLine_arInvoice
	foreign key (arinvoiceId) references ar.arInvoice (arInvoiceId)
go

alter table ar.arPaymentLine add constraint fk_arPaymentLine_accts
	foreign key (accountId) references dbo.accts (acct_id)
go

alter table ar.arPayment add constraint fk_arPayment_customer
	foreign key (customerId) references crm.customer(customerId)
go


