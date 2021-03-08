SELECT COALESCE(Max(CTLNo),0) + 1 as CTLNo 
INTO #TEMPCtlNo
FROM tblTransactionSummary 
Where TransactionCode = @_TR_CODE AND TransYear = @_TransYear;


SELECT Cast(COALESCE(Max(REF_Count), 0) + 1 AS BIGINT) AS REF_Count
INTO   #maxcountref 
FROM   tblBilling 
;

DECLARE @ref_count bigint
DECLARE @ReferenceNo varchar(50)
DECLARE @ControlNo bigint
SET @ref_count = (SELECT REF_Count FROM #maxcountref);
SET @ReferenceNo = Concat('@_BillMonth',RIGHT('0000000'+CAST(@ref_count as VARCHAR(20)),7))
SET @ControlNo = (SELECT CTLNo FROM #TEMPCtlNo)
;



Insert Into tblBilling
(
	SLC_CODE
	,SLT_CODE
	,BillMonth
	,REF_Count
	,ReferenceNo
	,ClientID
	,CurrentDue
	,BillStatus
	,TR_CODE
	,CTLNo
	,TR_Date
	,DateTimeAdded
	,Remarks
	,Consumption
	,ExcessConsumption
	,TotalDue
	,CurrentReading
	,PreviousReading
)
Values
(
	@_SLC_CODE
	,@_SLT_CODE
	,'@_BillMonth'
	,@ref_count
	,@ReferenceNo
	,@_ClientID
	,@_CurrentDue
	,@_BillStatus
	,@_TR_CODE
	,@ControlNo
	,'@_TR_Date'
	,GETDATE()
	,'Bill Added'
	,@_Consumption
	,@_ExcessConsumption
	,@_dueWithDiscount
	,@_CurrentReading
	,@_PreviousReading
);




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
)
VALUES
(
    @_TR_CODE
    ,@_TransYear
    ,@ControlNo
    ,'@_TransactionDate'
    ,@_ClientID
    ,'@_Explanation'
    ,GETDATE()
    ,@_PostedBy
)
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
VALUES
@_TransactionDetailValue
;




DROP TABLE #TEMPCtlNo;
DROP TABLE #maxcountref;