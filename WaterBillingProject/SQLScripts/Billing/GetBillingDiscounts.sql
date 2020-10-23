select disc.SLC_CODE
	  ,disc.SLT_CODE
	  ,gl.SLE_CODE
	  ,gl.StatusID 
	  ,disc.Description as SL_Description
	  ,sl.Formula
	  ,disc.COAID as AccountCode
	  ,disc.Amount
from tblBillDiscount disc
INNER JOIN tblGLControl gl
	ON gl.AccountCode = disc.COAID
INNER JOIN tblSLType sl
	ON sl.SLC_CODE = disc.SLC_CODE
	and sl.SLT_CODE = disc.SLT_CODE
	and sl.SLE_CODE = gl.SLE_CODE
	and sl.StatusID = gl.StatusID
WHERE disc.ClientID = @_ClientID
	and disc.ReferenceNo = '@_ReferenceNo'
;