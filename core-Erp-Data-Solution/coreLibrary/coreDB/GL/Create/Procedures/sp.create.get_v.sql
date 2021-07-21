
use coreDB
go

alter procedure get_v
@batchNo nvarchar(50)
with encryption
as
declare @rtr table(
 item nvarchar(200),     
	v_type nvarchar(200), type nvarchar(200), batch_no nvarchar(200), 
major_name nvarchar(200), minor_name nvarchar(200), major_symbol nvarchar(200), 
                         minor_symbol nvarchar(200), rate decimal, with_rate decimal, vat_rate decimal, nhil_rate decimal, 
						 with_amt decimal, vat_amt decimal, nhil_amt decimal, 
                        check_no nvarchar(200), invoice_no nvarchar(200), is_vat bit, is_nhil bit, is_withheld bit, posted bit, 
                        comp_name nvarchar(200), bank_acc_name nvarchar(200), 
                         bank_acc_num nvarchar(200), with_acc_name nvarchar(200), 
                         with_acc_num nvarchar(200), dtl_acc_name nvarchar(200), dtl_acc_num nvarchar(200), 
                         ref_no nvarchar(200),  tx_date datetime, ou_name nvarchar(200), description nvarchar(200),  tot_amount decimal, 
                         amount decimal
						 )
insert into @rtr
SELECT   'D' as item,     h.v_type, CASE WHEN v_type = 'C' THEN 'Receipt Voucher' ELSE 'Payment Voucher' END AS type, h.batch_no, 
isnull(c.major_name,''), isnull(c.minor_name,''), isnull(c.major_symbol,''), 
                         isnull(c.minor_symbol,''), isnull(h.rate,0.00), isnull(h.with_rate,0.00), isnull(h.vat_rate,0.00), isnull(h.nhil_rate,0.00), 
						 isnull(h.with_amt,0.00), isnull(h.vat_amt,0.00), isnull(h.nhil_amt,0.00), 
                         isnull(h.check_no,''), isnull(h.invoice_no,''), isnull(h.is_vat,0), isnull(h.is_nhil,0), isnull(h.is_withheld,0), 
						 isnull(h.posted,0), 
                         isnull(CASE WHEN v_type = 'C' THEN
                             (SELECT        MAX(cust_name)
                               FROM            custs
                               WHERE        cust_id = h.cust_id) ELSE
                             (SELECT        MAX(sup_name)
                               FROM            sups
                               WHERE        sup_id = h.sup_id) END,''), 
							   isnull(a.acc_name,''), 
                               isnull(a.acc_num,'') AS bank_acc_num, isnull(a3.acc_name,'') AS with_acc_name, 
                         isnull(a3.acc_num,'') AS with_acc_num, isnull(a2.acc_name,'') AS dtl_acc_name, isnull(a2.acc_num,'') AS dtl_acc_num, 
                         isnull(d.ref_no,''), isnull(d.tx_date,GETDATE()), isnull(u.ou_name,''),isnull(d.description,''), isnull(d.amount,0.00) as tot_amount, 
                         isnull(d.amount,0.00)
FROM            gl.v_dtl AS d INNER JOIN
                         gl.v_head AS h ON d.v_head_id = h.v_head_id INNER JOIN
                         dbo.accts AS a ON h.bank_acct_id = a.acct_id INNER JOIN
                         dbo.accts AS a2 ON d.acct_id = a2.acct_id INNER JOIN
                         dbo.accts AS a3 ON h.with_acct_id = a3.acct_id INNER JOIN
                         dbo.currencies AS c ON h.currency_id = c.currency_id LEFT OUTER JOIN
                         dbo.vw_gl_ou AS u ON d.gl_ou_id = u.ou_id
where @batchNo is null or h.batch_no = @batchNo
union all
SELECT   'F' as item,        h.v_type, CASE WHEN v_type = 'C' THEN 'Receipt Voucher' ELSE 'Payment Voucher' END AS type, 
h.batch_no, isnull(c.major_name,''), isnull(c.minor_name,''), isnull(c.major_symbol,''), 
                         isnull(c.minor_symbol,''), isnull(h.rate,0.00), isnull(h.with_rate,0.00), isnull(h.vat_rate,0.00), isnull(h.nhil_rate,0.00), 
						 isnull(h.with_amt,0.00), isnull(h.vat_amt,0.00), isnull(h.nhil_amt,0.00), 
                         isnull(h.check_no,''), isnull(h.invoice_no,''), isnull(h.is_vat,0), isnull(h.is_nhil,0), 
						 isnull(h.is_withheld,0), isnull(h.posted,0), 
                         isnull(CASE WHEN v_type = 'C' THEN
                             (SELECT        MAX(cust_name)
                               FROM            custs
                               WHERE        cust_id = h.cust_id) ELSE
                             (SELECT        MAX(sup_name)
                               FROM            sups
                               WHERE        sup_id = h.sup_id) END,'') AS comp_name, isnull(a.acc_name,'') AS bank_acc_name, 
                               isnull(a.acc_num,'') AS bank_acc_num, isnull(a2.acc_name,'') AS with_acc_name, 
                         isnull(a2.acc_num,'') AS with_acc_num, isnull(a3.acc_name,'') AS dtl_acc_name, isnull(a3.acc_num,'') AS dtl_acc_num, 
                         isnull(d.ref_no,''), isnull(d.tx_date,GETDATE()), isnull(u.ou_name,''), 
						 isnull(f.description,''), isnull(f.tot_amount,0.00), 
                         isnull(f.amount,0.00)
FROM            gl.v_head AS h INNER JOIN
                         gl.v_ftr AS f ON h.v_head_id = f.v_head_id INNER JOIN
                             (SELECT        MAX(gl_ou_id) AS gl_ou_id, MAX(ref_no) AS ref_no, MAX(tx_date) AS tx_date, v_head_id
                               FROM            gl.v_dtl AS i
                               GROUP BY v_head_id) AS d ON d.v_head_id = h.v_head_id INNER JOIN
                         dbo.accts AS a ON h.bank_acct_id = a.acct_id INNER JOIN
                         dbo.accts AS a3 ON f.acct_id = a3.acct_id INNER JOIN
                         dbo.accts AS a2 ON h.with_acct_id = a2.acct_id INNER JOIN
                         dbo.currencies AS c ON h.currency_id = c.currency_id LEFT OUTER JOIN
                         dbo.vw_gl_ou AS u ON d.gl_ou_id = u.ou_id
where @batchNo is null or h.batch_no = @batchNo

select * from @rtr