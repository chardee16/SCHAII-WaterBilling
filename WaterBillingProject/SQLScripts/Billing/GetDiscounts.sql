select sl.SLC_CODE
	  ,sl.SLT_CODE
	  ,sl.SLE_CODE
	  ,sl.StatusID 
	  ,sl.SL_Description
	  ,sl.Formula
	  ,gl.AccountCode
	  ,sl.Formula as Amount
from tblSLType sl
INNER JOIN tblGLControl gl
	ON gl.SLC_CODE = sl.SLC_CODE
		and gl.SLT_CODE = sl.SLT_CODE
		and gl.SLE_CODE = sl.SLE_CODE
		and gl.StatusID = sl.StatusID
WHERE sl.SLC_CODE = 13
and IsAutoLoad = 1