select BillMonth,
	   ReferenceNo,
	   billing.BillStatus,
	   bs.BillStatusDescription,
	   billing.TR_CODE,
	   TransType.TransactionDescription,
	   convert(varchar(10), TR_Date, 120) as TR_Date,
	   Consumption,
	   CurrentReading,
	   CurrentDue 
from tblBilling billing
INNER JOIN tblBillStatus bs
	ON bs.BillStatusID = billing.BillStatus
INNER JOIN tblTransactionType TransType
	ON TransType.TransactionCode = billing.TR_CODE
where ClientID = @_ClientID
order by BillMonth
