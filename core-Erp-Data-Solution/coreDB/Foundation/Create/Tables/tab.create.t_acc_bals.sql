use coreDB
go

CREATE table t_acc_bals
(
		acc_num nvarchar(250) ,
		acc_name nvarchar(250) ,
		head_name1 nvarchar(250) ,
		head_name2 nvarchar(250) ,
		head_name3 nvarchar(250) ,
		head_name4 nvarchar(250) ,
		cat_code tinyint,
		cat_name nvarchar(250) ,
		major_name nvarchar(250) ,
		major_symbol nvarchar(250) ,
		loc_end_bal float ,
		frgn_end_bal float,
		loc_beg_bal float,
		frgn_beg_bal float,
		minor_name nvarchar(250) ,
		minor_symbol nvarchar(250) ,
		acct_id int not null primary key,
		head_name5 nvarchar(250) ,
		head_name6 nvarchar(250) ,
		head_name7 nvarchar(250) ,
		currency_id int
)

GO
 