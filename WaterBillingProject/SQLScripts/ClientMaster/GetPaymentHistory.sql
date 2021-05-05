select ts.ORNo,
	   td.TransactionCode,
	   TransType.TransactionDescription,
	   td.TransYear,
	   td.CTLNo,
	   convert(varchar(10), td.TransactionDate, 120) as TransactionDate,
	   td.ClientName,
	   td.WaterBill,
	   td.Discount,
	   td.MonthlyDues,
	   td.GarbageCollection,
	   td.Surcharge,
	   td.CashReceived,
	   u.Username,
	   u.UserID
from (
		select
		td.TransactionCode,
		td.TransYear,
		td.CTLNo,
		td.TransactionDate,
		td.ClientName,
		COALESCE((
			SELECT COALESCE(SUM(Amt) * -1,0) FROM tblTransactionDetails tds
			WHERE tds.TransactionCode = td.TransactionCode
				AND tds.CTLNo = td.CTLNo
				and tds.TransYear = td.TransYear
				and SLC_CODE = 14 and SLT_CODE = 1
		),0) as WaterBill,
		COALESCE((
			SELECT COALESCE(SUM(Amt),0) FROM tblTransactionDetails td1
			WHERE td1.TransactionCode = td.TransactionCode
				AND td1.CTLNo = td.CTLNo
				and td1.TransYear = td.TransYear
				and td1.SLC_CODE = 13
		),0) as Discount,
		COALESCE((
			SELECT COALESCE(SUM(Amt) * -1,0) FROM tblTransactionDetails td1
			WHERE td1.TransactionCode = td.TransactionCode
				AND td1.CTLNo = td.CTLNo
				and td1.TransYear = td.TransYear
				and AccountCode = 402102
		),0) as MonthlyDues,
		COALESCE((
			SELECT COALESCE(SUM(Amt) * -1,0) FROM tblTransactionDetails td1
			WHERE td1.TransactionCode = td.TransactionCode
				AND td1.CTLNo = td.CTLNo
				and td1.TransYear = td.TransYear
				and AccountCode = 402103
		),0) as GarbageCollection,
		COALESCE((
			SELECT COALESCE(SUM(Amt) * -1,0) FROM tblTransactionDetails td1
			WHERE td1.TransactionCode = td.TransactionCode
				AND td1.CTLNo = td.CTLNo
				and td1.TransYear = td.TransYear
				and AccountCode = 402104
		),0) as Surcharge,
			COALESCE((
			SELECT COALESCE(SUM(Amt),0) FROM tblTransactionDetails td1
			WHERE td1.TransactionCode = td.TransactionCode
				AND td1.CTLNo = td.CTLNo
				and td1.TransYear = td.TransYear
				and SLC_CODE = 11
				and SLT_CODE = 1
				and SLE_CODE = 11
		),0) as CashReceived
	from tblTransactionDetails td
	where TransactionCode = 1
	and td.UPDTag = 1
	--and ClientID = 14
	Group By td.CTLNo,td.TransactionDate,td.ClientName,td.TransactionCode,td.TransYear
) td
INNER JOIN tblTransactionSummary ts
	ON ts.TransactionCode = td.TransactionCode
	and ts.CTLNo = td.CTLNo
	and ts.TransYear = td.TransYear
	and ClientID = @_ClientID
INNER JOIN tblTransactionType TransType
	ON TransType.TransactionCode = td.TransactionCode
INNER JOIN tblUser u
	ON u.UserID = ts.PostedBy
order by TransactionDate