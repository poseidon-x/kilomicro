use coreDB
go


create PROCEDURE get_bal_sht_for_close
(
	@start_date datetime,
	@end_date datetime,
	@no_tx bit
)
with encryption
AS
BEGIN
declare 
	@tbl TABLE 
(
	acct_id int,
	acc_num nvarchar(50),
	acc_name nvarchar(250),
	head_name1 nvarchar(100),
	head_name2 nvarchar(100),
	head_name3 nvarchar(100),
	head_name4 nvarchar(100),
	head_name5 nvarchar(100),
	head_name6 nvarchar(100),
	head_name7 nvarchar(100),
	cat_code int,
	cat_name nvarchar(100),
	major_name nvarchar(50),
	major_symbol nvarchar(3),
	minor_name nvarchar(50),
	minor_symbol nvarchar(3),
	currency_id int,
	cur_rate float,
	loc_end_bal float,
	frgn_end_bal float,
	loc_beg_bal float,
	frgn_beg_bal float
)
	declare @fin_year_start datetime
	declare @cost_center_id int 

	select @fin_year_start = dbo.fin_year_start(@end_date)
	
	insert into @tbl
	select
		acct_id,
		acc_num,
		acc_name,
		isnull(head_name1,''),
		isnull(head_name2,''),
		isnull(head_name3,''),
		isnull(head_name4,''),
		isnull(head_nam5,''),
		isnull(head_name6,''),
		isnull(head_name7,''),
		isnull(cat_code,''),
		isnull(cat_name,''),
		isnull(major_name,''),
		isnull(major_symbol,''),
		isnull(minor_name,''),
		isnull(minor_symbol,''),
		isnull(currency_id,0),
		isnull((select current_buy_rate from currencies c where c.currency_id = a.currency_id),0),
		isnull(dbo.acc_bal(acct_id, cat_code, @start_date, @end_date, @cost_center_id),0) as loc_end_bal,
		0 as frgn_end_bal,
		isnull(dbo.acc_bal(acct_id, cat_code, dbo.fin_year_start(dateadd(ss,-1, @start_date)), dateadd(ss,-1, @start_date) , @cost_center_id ),0) as loc_beg_bal,
		0 as frgn_beg_bal
	from vw_accounts a
	where cat_code <=3 and acct_id not in (select acct_id from def_accts where code in ('CE','RE'))
	union all 
	select
		acct_id,
		acc_num,
		acc_name,
		isnull(head_name1,''),
		isnull(head_name2,''),
		isnull(head_name3,''),
		isnull(head_name4,''),
		isnull(head_nam5,''),
		isnull(head_name6,''),
		isnull(head_name7,''),
		isnull(cat_code,''),
		isnull(cat_name,''),
		isnull(major_name,''),
		isnull(major_symbol,''),
		isnull(minor_name,''),
		isnull(minor_symbol,''),
		isnull(currency_id,0),
		isnull((select current_buy_rate from currencies c where c.currency_id = a.currency_id),0),
		isnull(dbo.cur_earn(@end_date, @cost_center_id),0) + isnull(dbo.ret_earn(@end_date, @cost_center_id),0) as loc_end_bal,
		0 as frgn_end_bal,
		0 as loc_beg_bal,
		0 as frgn_beg_bal
	from vw_accounts a
	where acct_id in (select acct_id from def_accts where code='RE')
	
	select * from @tbl 
	where  @no_tx=1 or loc_end_bal<>0 or loc_beg_bal<>0
END
GO