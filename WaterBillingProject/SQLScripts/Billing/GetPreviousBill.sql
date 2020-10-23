SELECT  BillMonth
	,ReferenceNo
	,TotalDue
from tblBilling
WHERE ClientID = @_ClientID
@_Condition