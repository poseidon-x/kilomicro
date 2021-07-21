use coreDB
go

update dbo.modules set module_name = 'Client Investment Statement of Account' where module_id=-2124;
update dbo.modules set module_name = 'Client Investment Balances Report' where module_id=-2120;
update dbo.modules set module_name = 'Client Investment Products' where module_id=98;
update dbo.modules set module_name = 'Investments Accounts Management' where module_id=97;
update dbo.modules set module_name = 'Client Investments Management' where module_id=100;
update dbo.modules set module_name = 'Client Investments Reports' where module_id=193;
update dbo.modules set module_name = 'New Client Investment' where module_id=99;
update dbo.modules set module_name = 'Roll over Client Investment' where module_id=2545;
update dbo.modules set module_name = 'Regular Deposit Accounts' where module_id=2270;
update dbo.modules set module_name = 'Manage Regular Deposit Products' where module_id=3098;
update dbo.modules set module_name = 'New Regular Deposit Account' where module_id=3099;
update dbo.modules set module_name = 'Manage Regular Deposit Account' where module_id=3100;
update dbo.modules set module_name = 'Regular Deposit Account Balances' where module_id=3120;
update dbo.modules set module_name = 'Regular Deposit Statement of Account' where module_id=3124; 
update dbo.modules set module_name = 'Company Investment Accounts' where module_id=12190; 
update dbo.modules set module_name = 'Approve Write-off on Client Investment' where module_id=13311; 
update dbo.modules set module_name = 'Approve Charges on Client Investment' where module_id=13304;  
update dbo.modules set module_name = 'Approve Charges on Regular Deposit Account' where module_id=13302; 
update dbo.modules set module_name = 'Detailed Regular Deposit Report' where module_id=13272; 
update dbo.modules set module_name = 'Detailed Client Investment Balances Report' where module_id=13274; 
update dbo.modules set module_name = 'Client Investment Accounts' where module_id=2271;

update dbo.modules set module_name = 'Reprint Withdrawal Receipt' where module_id=13273;
update dbo.modules set module_name = 'Reprint Deposit Receipt' where module_id=13325;

update dbo.modules set visible=0 where module_id in (3545, 12226, 13310, 3145);

update dbo.modules set module_name = 'Reprint Withdrawal Receipt' where module_id=13275;
update dbo.modules set module_name = 'Reprint Deposit Receipt' where module_id=13326;

update dbo.modules set module_name=replace(module_name, 'Regular Susu', 'Normal Susu') where module_name like '%Regular Susu%'

go
