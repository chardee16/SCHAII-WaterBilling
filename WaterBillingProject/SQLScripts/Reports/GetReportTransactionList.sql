select ts.ORNo,
	   td.TransactionCode,
	   td.TransYear,
	   td.CTLNo,
	   td.TransactionDate,
	   td.ClientName,
	   td.WaterBill,
	   td.Discount,
	   td.MonthlyDues,
	   td.GarbageCollection,
	   td.Surcharge,
	   td.CashReceived
from (
		select
		td.TransactionCode,
		td.TransYear,
		td.CTLNo,
		td.TransactionDate,
		td.ClientName,
		COALESCE(SUM(td.Amt),0) * -1 as WaterBill,
		COALESCE((
			SELECT COALESCE(Amt,0) FROM tblTransactionDetails td1
			WHERE td1.TransactionCode = td.TransactionCode
				AND td1.CTLNo = td.CTLNo
				and td1.TransYear = td.TransYear
				and AccountCode = 401
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
	and SLC_CODE = 14 and SLT_CODE = 1 and Amt < 0
	Group By td.CTLNo,td.TransactionDate,td.ClientName,td.TransactionCode,td.TransYear
) td
INNER JOIN tblTransactionSummary ts
	ON ts.TransactionCode = td.TransactionCode
	and ts.CTLNo = td.CTLNo
	and ts.TransYear = td.TransYear
	and ts.TransactionDate >= '@_DateFrom' and ts.TransactionDate <= '@_DateTo'