SELECT  bill.BillMonth
	,bill.ReferenceNo
	,
	(
		SELECT SUM(Amt) from tblTransactionDetails td
		WHERE td.SLC_CODE = bill.SLC_CODE AND  td.SLT_CODE = bill.SLT_CODE
		AND td. ReferenceNo = bill.ReferenceNo and  td.ClientID = bill.ClientID
	) as TotalDue
from tblBilling bill
WHERE ClientID = @_ClientID
@_Condition