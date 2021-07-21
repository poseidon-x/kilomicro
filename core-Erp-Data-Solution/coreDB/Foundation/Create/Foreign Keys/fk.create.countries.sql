use coreDB
go

alter table countries add
	constraint fk_country_currency foreign key (currency_id)
	references currencies(currency_id) on delete set null on update cascade 
go
	