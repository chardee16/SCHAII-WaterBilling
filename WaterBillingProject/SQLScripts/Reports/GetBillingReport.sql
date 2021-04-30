Declare @paramBillMonth varchar(50)
;

SET @paramBillMonth = '@_BillMonth'
;


Select 
	  c.BlockNo,
	  c.LotNo,
	  c.ClientID,
	  c.LastName + ', ' + c.FirstName as FullName,
	  COALESCE(bill.PreviousReading,0) as PreviousReading,
	  COALESCE(bill.CurrentReading,0) as PresentReading,
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
	  COALESCE(IIF(bill.TotalDue = 0, bill.CurrentDue,bill.TotalDue),0) as NetDue,
	  (
		SELECT COALESCE(SUM(bc.Amount),0) from tblBilling b2
		INNER JOIN tblBillCharges bc
			ON bc.SLC_CODE  =15
			and bc.SLT_CODE = 1
			and bc.ClientID = b2.ClientID
			and bc.ReferenceNo = b2.ReferenceNo
		where b2.ClientID = c.ClientID
		and b2.BillStatus = 1
		and TR_Date <= bill.TR_Date
	  ) as MonthlyDue,
	  (
		SELECT COALESCE(SUM(bc.Amount),0) from tblBilling b2
		INNER JOIN tblBillCharges bc
			ON bc.SLC_CODE  =15
			and bc.SLT_CODE = 2
			and bc.ClientID = b2.ClientID
			and bc.ReferenceNo = b2.ReferenceNo
		where b2.ClientID = c.ClientID
		and b2.BillStatus = 1
		and TR_Date <= bill.TR_Date
	  ) as GarbageDue,
	  (
		SELECT COALESCE(SUM(IIF(b2.TotalDue = 0,b2.CurrentDue,b2.TotalDue)),0) from tblBilling b2
		where b2.ClientID = c.ClientID
		and b2.TR_Date < bill.TR_Date
		and b2.BillStatus = 1
	  ) as PreviousBalance
INTO #Temp
from tblClient c
LEFT JOIN tblBilling bill
		ON bill.ClientID = c.ClientID and bill.BillMonth = @paramBillMonth
		and BillStatus = 1
WHERE c.AccountStatusID = 1 --and c.ClientID = 10056
Order by c.BlockNo,c.LotNo


SELECT *,
	COALESCE(NetDue + MonthlyDue + GarbageDue + PreviousBalance,0) as TotalDue
FROM #Temp
Order by BlockNo,LotNo

;

DROP TABLE #Temp

