use coreDB
go

alter table momo.momoWallet add 
	constraint fk_momoWallet_serviceProvider foreign key (providerID)
	references momo.momoProvider (providerID)
go


alter table momo.walletLoading add 
	constraint fk_walletLoading_momoWallet foreign key (walletID)
	references momo.momoWallet (walletID)
go
	
alter table momo.walletCashup add 
	constraint fk_walletCashup_momoWallet foreign key (walletID)
	references momo.momoWallet (walletID)
go

alter table momo.momoTransaction add 
	constraint fk_momoTransaction_momoWallet foreign key (walletID)
	references momo.momoWallet (walletID)
go


alter table momo.momoTransaction add 
	constraint fk_momoTransaction_momoClient foreign key (momoClientID)
	references momo.momoClient (momoClientID)
go

alter table momo.momoTransaction add 
	constraint fk_momoTransaction_momoService foreign key (serviceID)
	references momo.momoService (serviceID)
go

alter table momo.momoService add 
	constraint fk_momoService_provider foreign key (providerID)
	references momo.momoProvider (providerID)
go

alter table momo.momoServiceCharge add 
	constraint fk_momoServiceCharge_momoService foreign key (serviceID)
	references momo.momoService (serviceID)
go
