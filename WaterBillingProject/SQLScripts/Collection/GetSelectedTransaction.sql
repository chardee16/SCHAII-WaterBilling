SELECT coa.COADesc as SL_Description,
	   td.AccountCode,
	   td.BillMonth,
	   IIF(Amt > 0,Convert(varchar,Amt),'') as Debit,
	   IIF(Amt < 0,Convert(varchar,Amt * -1),'') as Credit
FROM tblTransactionDetails td
INNER JOIN tblChartofaccounts coa
	ON coa.COAID = td.AccountCode
WHERE TransactionCode = @_TransactionCode
and CTLNo = @_CTLNo
and TransYear = @_TransYear