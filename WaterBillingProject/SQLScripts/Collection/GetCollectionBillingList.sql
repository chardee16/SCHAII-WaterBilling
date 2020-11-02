SELECT  bill.SLC_CODE
	,bill.SLT_CODE
	,bill.BillMonth
	,Concat(bill.Consumption,' Cu.M') as Consumption
	,bill.ReferenceNo
	,bill.CurrentDue
	,slt.SL_Description
from tblBilling bill
INNER JOIN tblSLType slt
	ON slt.SLC_CODE = bill.SLC_CODE AND slt.SLT_CODE = bill.SLT_CODE
WHERE ClientID = @_ClientID
and BillStatus = 1
Order by BillMonth desc