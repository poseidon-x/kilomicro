use coreDB
go

alter table past_currency_rates add
	constraint fk_past_currency_rates foreign key (currency_id)
	references currencies(currency_id) on delete cascade on update cascade 
go
	