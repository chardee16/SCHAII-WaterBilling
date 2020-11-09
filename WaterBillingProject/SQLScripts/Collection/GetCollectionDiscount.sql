select bd.SLC_CODE
	  ,bd.SLT_CODE
	  ,gl.SLE_CODE
	  ,gl.StatusID 
	  ,bd.Description
	  ,bd.COAID
	  ,bill.ReferenceNo
	  ,bd.Amount -
	  (
		SELECT COALESCE(SUM(Amt),0) FROM tblTransactionDetails td
			where td.SLC_CODE = bd.SLC_CODE
			and td.SLT_CODE = bd.SLT_CODE
			and td.SLE_CODE = gl.SLE_CODE
			and td.ReferenceNo = bd.ReferenceNo
	  ) as Amount
	  ,bill.BillMonth
from tblBilling bill
INNER JOIN tblBillDiscount bd
	ON bd.ClientID = bill.ClientID AND bd.ReferenceNo = bill.ReferenceNo
INNER JOIN tblGLControl gl
	ON gl.AccountCode = bd.COAID
WHERE bill.ClientID = @_ClientID
and BillStatus = 1
order by ReferenceNo desc
