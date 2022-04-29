SELECT ts.TransactionCode,
	   ts.TransYear,
	   ts.CTLNo,
	   convert(varchar(10), ts.TransactionDate, 120) as TransactionDate,
	   ts.ClientID,
	   td.Amt,
	   td.UPDTag,
	   td.ClientName
from tblTransactionSummary ts
INNER JOIN tblTransactionDetails td
	ON td.TransactionCode = ts.TransactionCode
		and td.CTLNo = ts.CTLNo
		and td.TransYear = ts.TransYear
		and td.SLC_CODE = 11
where ts.TransactionCode = 1
and ts.TransactionDate = '@_trDate'