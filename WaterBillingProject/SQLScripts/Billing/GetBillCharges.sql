select sl.SLC_CODE
	  ,sl.SLT_CODE
	  ,sl.SLE_CODE
	  ,sl.StatusID 
	  ,sl.SL_Description
	  ,sl.Formula
	  ,gl.AccountCode
	  ,	
		 sl.Formula + COALESCE((	
		SELECT (
					SELECT SUM(Amt) FROM tblTransactionDetails td
					WHERE td.SLC_CODE = c.SLC_CODE and td.SLT_CODE = c.SLT_CODE
						and td.AccountCode = c.COAID and td.ReferenceNo = c.ReferenceNo
				) as Amount
			from tblBillCharges c 
			INNER JOIN tblBilling bill
			ON bill.ClientID = @_ClientID @_Condition
			WHERE c.SLC_CODE = sl.SLC_CODE 
				AND c.SLT_CODE = sl.SLT_CODE 
				AND c.ReferenceNo = bill.ReferenceNo
		),0) as Amount
from tblSLType sl
INNER JOIN tblGLControl gl
	ON gl.SLC_CODE = sl.SLC_CODE
		and gl.SLT_CODE = sl.SLT_CODE
		and gl.SLE_CODE = sl.SLE_CODE
		and gl.StatusID = sl.StatusID
WHERE sl.SLC_CODE = 15
and IsAutoLoad = 1