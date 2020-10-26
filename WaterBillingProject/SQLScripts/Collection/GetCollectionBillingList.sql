SELECT  BillMonth
	,Concat(Consumption,' Cu.M') as Consumption
	,ReferenceNo
	,CurrentDue
from tblBilling
WHERE ClientID = @_ClientID
and BillStatus = 1
Order by BillMonth desc