select bc.SLC_CODE
	  ,bc.SLT_CODE
	  ,gl.SLE_CODE
	  ,gl.StatusID 
	  ,bc.Description
	  ,bc.COAID
	  ,bc.ReferenceNo
	  ,bc.Amount
	  ,bill.BillMonth
from tblBilling bill
INNER JOIN tblBillCharges bc
	ON bc.ClientID = bill.ClientID
		AND bc.ReferenceNo = bill.ReferenceNo
INNER JOIN tblGLControl gl
	ON gl.AccountCode = bc.COAID
WHERE bill.ClientID = @_ClientID
and BillStatus = 1
order by ReferenceNo desc