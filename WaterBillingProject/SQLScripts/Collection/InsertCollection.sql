SELECT COALESCE(Max(CTLNo),0) + 1 as CTLNo 
INTO #TEMPCtlNo
FROM tblTransactionSummary 
Where TransactionCode = @_TransactionCode AND TransYear = @_TransYear;



DECLARE 
@ControlNo bigint
;
SET @ControlNo = (SELECT CTLNo FROM #TEMPCtlNo)
;


INSERT INTO tblTransactionSummary
(
    TransactionCode
    ,TransYear
    ,CTLNo
    ,TransactionDate
    ,ClientID
    ,Explanation
    ,DateTimeAdded
    ,PostedBy
    ,ORNo
)
VALUES
(
    @_TransactionCode
    ,@_TransYear
    ,@ControlNo
    ,'@_TransactionDate'
    ,@_ClientID
    ,'@_Explanation'
    ,GETDATE()
    ,@_PostedBy
    ,'@_ORNumber'
)






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
VALUES
@_TransactionDetailValue
;


INSERT INTO tblTransactionCheck
(
    TransactionCode
    ,CTLNo
    ,TransYear
    ,COCIType
    ,Amt
    ,UPDTag
)
VALUES
@_TransactionCheckValue
;





DROP TABLE #TEMPCtlNo;