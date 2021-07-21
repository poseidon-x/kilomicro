use coreDB
go

create view gl.vw_v
with encryption as
SELECT   '' as item,     
	'' v_type, '' type, '' batch_no, 
'' major_name, '' minor_name, '' major_symbol, 
                         '' minor_symbol, 0.000 rate, 0.000 with_rate, 0.000 vat_rate, 0.000 nhil_rate, 
						 0.000 with_amt, 0.000 vat_amt, 0.000 nhil_amt, 
                        '' check_no, '' invoice_no, isnull(cast(0 as bit),0) is_vat, isnull(cast(0 as bit),0) is_nhil, 
						isnull(cast(0 as bit),0) is_withheld, isnull(cast(0 as bit),0) posted, 
                        '' comp_name, '' bank_acc_name, 
                         '' bank_acc_num, '' with_acc_name, 
                         '' with_acc_num, '' dtl_acc_name, '' dtl_acc_num, 
                         '' ref_no, GETDATE() tx_date, '' ou_name, '' description, 0.000  tot_amount, 
                         0.000 amount 