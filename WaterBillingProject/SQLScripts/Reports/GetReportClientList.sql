Select c.ClientID,
	  c.FirstName + ' ' + c.LastName as FullName
	  ,'Blk ' + Cast(c.BlockNo as varchar) + ' and Lot ' + Cast(c.LotNo as varchar) as FullAddress
	  ,COALESCE(
	  (
		SELECT COALESCE(SUM(b.Consumption),0) from tblBilling b
		WHERE b.ClientID = c.ClientID
	  ),0) as Consumption
	  ,COALESCE(
	  (
		SELECT COALESCE(SUM(Amt), 0) from tblTransactionDetails td
		WHERE (td.ClientID = c.ClientID and td.SLC_CODE = 14 and td.SLT_CODE = 1)
			OR (td.ClientID = c.ClientID and td.SLC_CODE = 15 and td.SLT_CODE IN (1,2) )
	  ),0) as TotalDue,
	  COALESCE(
	  (
		SELECT COUNT(b2.ClientID) from tblBilling b2
		WHERE b2.ClientID = c.ClientID
		and b2.BillStatus = 1
	  ),0) as TotalUnpaidBill
from tblClient c
order by TotalDue desc