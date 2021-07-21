use coreDB
GO

DELETE from dbo.acct_cats
GO

set identity_insert dbo.acct_cats on

insert dbo.acct_cats(acct_cat_id,cat_name,cat_code,max_acct_num,min_acct_num,creation_date,creator,modification_date,last_modifier) values(1,'Assets',1,'1999','1000','Mar 16 2013 12:00AM','coreAdmin',null,null)
insert dbo.acct_cats(acct_cat_id,cat_name,cat_code,max_acct_num,min_acct_num,creation_date,creator,modification_date,last_modifier) values(2,'Liabilities',2,'2999','2000','Mar 16 2013 12:00AM','coreAdmin',null,null)
insert dbo.acct_cats(acct_cat_id,cat_name,cat_code,max_acct_num,min_acct_num,creation_date,creator,modification_date,last_modifier) values(3,'Equity',3,'3999','3000','Mar 16 2013 12:00AM','coreAdmin',null,null)
insert dbo.acct_cats(acct_cat_id,cat_name,cat_code,max_acct_num,min_acct_num,creation_date,creator,modification_date,last_modifier) values(4,'Income',4,'4999','4000','Mar 16 2013 12:00AM','coreAdmin',null,null)
insert dbo.acct_cats(acct_cat_id,cat_name,cat_code,max_acct_num,min_acct_num,creation_date,creator,modification_date,last_modifier) values(5,'Cost of Goods Sold',5,'5999','5000','Mar 16 2013 12:00AM','coreAdmin',null,null)
insert dbo.acct_cats(acct_cat_id,cat_name,cat_code,max_acct_num,min_acct_num,creation_date,creator,modification_date,last_modifier) values(6,'Expense',6,'6999','6000','Mar 16 2013 12:00AM','coreAdmin',null,null)
insert dbo.acct_cats(acct_cat_id,cat_name,cat_code,max_acct_num,min_acct_num,creation_date,creator,modification_date,last_modifier) values(7,'Other Income',7,'7999','7000','Mar 16 2013 12:00AM','coreAdmin',null,null)
insert dbo.acct_cats(acct_cat_id,cat_name,cat_code,max_acct_num,min_acct_num,creation_date,creator,modification_date,last_modifier) values(8,'Other Expense',8,'8999','8000','Mar 16 2013 12:00AM','coreAdmin',null,null)

set identity_insert dbo.acct_cats off
GO

update statistics dbo.acct_cats
GO