Select 
	  c.BlockNo,
	  c.LotNo,
	  c.ClientID,
	  c.FirstName + ' ' + c.LastName as FullName,
	  COALESCE(bill.Consumption,0) as Consumption,
	  IIF(COALESCE(bill.Consumption,0) < 10, COALESCE(bill.Consumption,0),10) as Minimum,
	  COALESCE(bill.ExcessConsumption,0) as Excess
from tblClient c
LEFT JOIN tblBilling bill
		ON bill.ClientID = c.ClientID and BillMonth = '202101'
		and BillStatus != 3
LEFT JOIN tblTransactionDetails td
	ON td.ClientID = c.ClientID
		and td.TransactionCode = bill.TR_CODE
		and td.CTLNo = bill.CTLNo
WHERE c.AccountStatusID = 1
order by BlockNo,LotNo