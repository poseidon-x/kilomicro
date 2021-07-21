use coreDB
go

create index idx_momoWallet_cashier 
on momo.momoWallet
(
	cashiersTillID asc
)
go

create index idx_momoWallet_provider 
on momo.momoWallet
(
	providerID asc
)
go

create index idx_walletLoading_wallet 
on momo.walletLoading
(
	walletID asc
)
go

create index idx_walletLoading_date 
on momo.walletLoading
(
	loadingDate asc
)
go

create index idx_walletLoading_posted 
on momo.walletLoading
(
	posted asc
)
go

create index idx_walletCashup_posted 
on momo.walletCashup
(
	posted asc
)
go

create index idx_walletCashup_date 
on momo.walletCashup
(
	cashupDate asc
)
go

create index idx_walletCashup_wallet 
on momo.walletCashup
(
	walletID asc
)
go

create index idx_momoService_provider 
on momo.momoService
(
	providerID asc
)
go

create index idx_momoServiceCharge_service 
on momo.momoServiceCharge
(
	serviceID asc
)
go

create index idx_momoTransaction_wallet
on momo.momoTransaction
(
	walletID asc
)
go

create index idx_momoTransaction_posted
on momo.momoTransaction
(
	posted asc
)
go

create index idx_momoTransaction_service
on momo.momoTransaction
(
	serviceID asc
)
go

create index idx_momoTransaction_client
on momo.momoTransaction
(
	momoClientID asc
)
go
