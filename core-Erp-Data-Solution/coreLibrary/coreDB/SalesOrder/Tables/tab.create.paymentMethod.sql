use coreDB
go


create table so.paymentMethod
(
	paymentMethodID int not null primary key,
	paymentMethodName nvarchar(250) not null constraint uk_paymentMethod unique
)
go

insert into so.paymentMethod (paymentMethodID, paymentMethodName)
values (1, 'Cash')
insert into so.paymentMethod (paymentMethodID, paymentMethodName)
values (2, 'Cheque')
insert into so.paymentMethod (paymentMethodID, paymentMethodName)
values (3, 'Bank Transfer')
insert into so.paymentMethod (paymentMethodID, paymentMethodName)
values (4, 'Credit/Debit Card')
insert into so.paymentMethod (paymentMethodID, paymentMethodName)
values (5, 'Mobile Money')
go

