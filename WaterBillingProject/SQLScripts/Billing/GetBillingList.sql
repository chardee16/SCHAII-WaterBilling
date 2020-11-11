select BillMonth
	  ,ReferenceNo
	  ,ClientID
	  ,CurrentDue
	  ,BillStatus
	  ,stat.BillStatusDescription
	  ,Consumption
	  ,TotalDue as dueWithDiscount
	  ,CurrentReading
	  ,PreviousReading
	  ,TR_Date
	  ,TR_CODE
	  ,CTLNo
from tblBilling bill
INNER JOIN tblBillStatus stat
	ON stat.BillStatusID = bill.BillStatus
WHERE ClientID = @_ClientID