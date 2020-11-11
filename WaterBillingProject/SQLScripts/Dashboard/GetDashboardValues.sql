SELECT 
	COALESCE(SUM(Consumption),0) as Consumption,
	(
		SELECT COALESCE(SUM(td.Amt * -1),0) FROM tblTransactionDetails td
		WHERE td.TransactionCode = 1
		and SLC_CODE = 14
		and SLT_CODE = 1
		and SLE_CODE = 11
		AND UPDTag = 1
	) as PaymentReceived,
	(
		SELECT COALESCE(SUM(td.Amt * -1),0) FROM tblTransactionDetails td
		WHERE td.TransactionCode = 1
		and SLC_CODE = 15
		AND UPDTag = 1
	) as ChargesReceived
FROM tblBilling
WHERE BillStatus != 3
	and YEAR(TR_Date) = @_Year