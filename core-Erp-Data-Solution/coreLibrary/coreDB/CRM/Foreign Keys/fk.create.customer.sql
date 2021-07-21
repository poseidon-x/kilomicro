use coreDB
go

alter table crm.customerShippingAddress add constraint fk_customerShippingAddress_customer
foreign key (customerId) references crm.customer(customerId)
go


alter table crm.customerBusinessAddress add constraint fk_customerBusinessAddress_customer
foreign key (customerId) references crm.customer(customerId)
go

alter table crm.customerPhone add constraint fk_customerPhone_customer
foreign key (customerId) references crm.customer(customerId)
go


alter table crm.customerEmail add constraint fk_customerEmail_customer
foreign key (customerId) references crm.customer(customerId)
go


alter table crm.customerContactPerson add constraint fk_customerContactPerson_customer
foreign key (customerId) references crm.customer(customerId)
go


