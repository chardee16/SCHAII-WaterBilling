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
	  COALESCE(bill.TotalDue,0) as NetDue,
	  COALESCE(SUM(monthDue.Amount),0) as MonthlyDue
from tblClient c
LEFT JOIN tblBilling bill
		ON bill.ClientID = c.ClientID and bill.BillMonth = @paramBillMonth
		and BillStatus != 3
LEFT JOIN (
	SELECT bc.ClientID,bc.Amount from
	tblBillCharges bc
	INNER JOIN tblBilling billing
		ON billing.ClientID = bc.ClientID
			and billing.ReferenceNo = bc.ReferenceNo
			and billing.TR_Date <= '2021-01-30'
	WHERE bc.SLC_CODE = 15
		and bc.SLT_CODE = 1
	) monthDue ON monthDue.ClientID = c.ClientID
WHERE c.AccountStatusID = 1 
GROUP BY c.BlockNo,
	  c.LotNo,
	  c.ClientID,
	  c.FirstName ,
	  c.LastName,
	  Consumption,
	  ExcessConsumption,
	  CurrentDue,
	  bill.ReferenceNo,
	  bill.TotalDue
order by BlockNo,LotNo
