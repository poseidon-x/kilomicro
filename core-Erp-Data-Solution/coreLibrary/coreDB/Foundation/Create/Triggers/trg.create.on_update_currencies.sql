use coreDB
go
 
CREATE Trigger on_update_currencies ON currencies 
with encryption 

FOR INSERT, UPDATE AS 

	insert into past_currency_rates
	(currency_id, buy_rate, sell_rate, tran_datetime, creation_date, creator)
	select
		currency_id, current_buy_rate, current_sell_rate, getdate(), creation_date, creator
	from inserted

GO

