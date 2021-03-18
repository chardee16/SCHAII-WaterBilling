Declare @paramBillMonth varchar(50)
;

SET @paramBillMonth = '202101'
;


Select 
	  c.BlockNo,
	  c.LotNo,
	  c.ClientID,
	  c.FirstName + ' ' + c.LastName as FullName,
	  COALESCE(bill.Consumption,0) as Consumption,
	  IIF(COALESCE(bill.Consumption,0) < 10, COALESCE(bill.Consumption,0),10) as Minimum,
	  COALESCE(bill.ExcessConsumption,0) as Excess,
	  COALESCE(bill.CurrentDue,0) as AmountDue,
	  COALESCE(
		(
			SELECT COALESCE(SUM(Amount),0) FROM tblBillDiscount discount
				WHERE discount.ClientID = c.ClientID
					and discount.ReferenceNo = bill.ReferenceNo
		)
	  ,0) as Discount,
	  COALESCE(bill.TotalDue,0) as NetDue
from tblClient c
LEFT JOIN tblBilling bill
		ON bill.ClientID = c.ClientID and BillMonth = @paramBillMonth
		and BillStatus != 3
--LEFT JOIN tblTransactionDetails BillDue
--	ON BillDue.ClientID = c.ClientID
--		and BillDue.TransactionCode = bill.TR_CODE
--		and BillDue.CTLNo = bill.CTLNo
--		and BillDue.ReferenceNo = bill.ReferenceNo
--		and BillDue.AccountCode = 402101
WHERE c.AccountStatusID = 1
order by BlockNo,LotNo