use coreDB
go


CREATE FUNCTION acc_bals
(
	@start_date datetime,
	@end_date datetime
)
RETURNS 
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
	cat_code tinyint,
	cat_name nvarchar(100),
	major_name nvarchar(50),
	major_symbol nvarchar(3),
	minor_name nvarchar(50),
	minor_symbol nvarchar(3),
	currency_id int,
	loc_end_bal float,
	frgn_end_bal float
)
with encryption
AS
BEGIN
	declare @fin_year_start datetime
	
	select @fin_year_start = dbo.fin_year_start(@end_date)
	
	insert into @tbl
	select
		acct_id,
		acc_num,
		acc_name,
		head_name1,
		head_name2,
		head_name3,
		head_name4,
		head_nam5,
		head_name6,
		head_name7,
		cat_code,
		cat_name,
		major_name,
		major_symbol,
		minor_name,
		minor_symbol,
		currency_id,
		dbo.acc_bal(acct_id, cat_code, @fin_year_start, @end_date) as loc_end_bal,
		0 as frgn_end_bal
	from vw_accounts a
	
	RETURN 
END
GO