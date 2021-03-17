SELECT COALESCE(Max(CTLNo),0) + 1 as CTLNo 
INTO #TEMPCtlNo
FROM tblTransactionSummary 
Where TransactionCode = 5 AND TransYear = @_TransYear;



DECLARE 
@ControlNo bigint
;
SET @ControlNo = (SELECT CTLNo FROM #TEMPCtlNo)
;


SELECT
	5 as TransactionCode,
	td.TransYear,
	@ControlNo as CTLNo,
	td.TransactionDate,
	td.ClientID,
	'Reversed :' + td.Explanation as Explanation,
	GETDATE() as DateTimeAdded,
	td.PostedBy,
	0 as ORNo
INTO #TEMPTransum
FROM tblTransactionSummary td
WHERE TransactionCode = @_TransactionCode
and CTLNo = @_CTLNo
and TransYear = @_TransYear
;



SELECT 
	5 as TransactionCode,
	td.TransYear,
	@ControlNo as CTLNo,
	td.AccountCode,
	td.ClientID,
	td.BillMonth,
	td.SLC_CODE,
	td.SLT_CODE,
	td.ReferenceNo,
	td.SLE_CODE,
	td.StatusID,
	td.TransactionDate,
	td.Amt * -1 as Amt,
	td.PostedBy,
	1 as UPDTag,
	td.SequenceNo,
	td.ClientName
INTO #TEMPTranDT
FROM tblTransactionDetails td
WHERE TransactionCode = @_TransactionCode
and CTLNo = @_CTLNo
and TransYear = @_TransYear
;

INSERT INTO tblTransactionSummary
SELECT * FROM #TEMPTransum
;

INSERT INTO tblTransactionDetails
(
	TransactionCode
	,TransYear
	,CTLNo
	,AccountCode
	,ClientID
	,BillMonth
	,SLC_CODE
	,SLT_CODE
	,ReferenceNo
	,SLE_CODE
	,StatusID
	,TransactionDate
	,Amt
	,PostedBy
	,UPDTag
	,SequenceNo
	,ClientName
)
SELECT 
	TransactionCode
	,TransYear
	,CTLNo
	,AccountCode
	,ClientID
	,BillMonth
	,SLC_CODE
	,SLT_CODE
	,ReferenceNo
	,SLE_CODE
	,StatusID
	,TransactionDate
	,Amt
	,PostedBy
	,UPDTag
	,SequenceNo
	,ClientName 
FROM #TEMPTranDT
;


UPDATE tblTransactionDetails
SET UPDTag = 5
WHERE TransactionCode = @_TransactionCode
and CTLNo = @_CTLNo
and TransYear = @_TransYear
;


UPDATE tblTransactionCheck
SET UPDTag = 5
WHERE TransactionCode = @_TransactionCode
and CTLNo = @_CTLNo
and TransYear = @_TransYear;

DROP TABLE #TEMPCtlNo;
DROP TABLE #TEMPTransum;
DROP TABLE #TEMPTranDT;