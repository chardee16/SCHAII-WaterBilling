SELECT  bill.SLC_CODE
	,bill.SLT_CODE
	,bill.BillMonth
	,Concat(bill.Consumption,' Cu.M') as Consumption
	,bill.ReferenceNo
	,
	(
		SELECT SUM(Amt) from tblTransactionDetails
		WHERE SLC_CODE = bill.SLC_CODE AND SLT_CODE = bill.SLT_CODE
		AND ReferenceNo = bill.ReferenceNo and ClientID = bill.ClientID
	) as CurrentDue
	,slt.SL_Description
from tblBilling bill
INNER JOIN tblSLType slt
	ON slt.SLC_CODE = bill.SLC_CODE AND slt.SLT_CODE = bill.SLT_CODE and slt.SLE_CODE = 11
WHERE ClientID = @_ClientID
and BillStatus = 1
Order by BillMonth