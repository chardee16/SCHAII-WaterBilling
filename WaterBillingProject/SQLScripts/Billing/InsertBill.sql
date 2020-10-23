SELECT Cast(COALESCE(Max(REF_Count), 0) + 1 AS BIGINT) AS REF_Count
INTO   #maxcountref 
FROM   tblBilling 
;

DECLARE @ref_count bigint
DECLARE @ReferenceNo varchar(50)
SET @ref_count = (SELECT REF_Count FROM #maxcountref);
SET @ReferenceNo = Concat('@_BillMonth',RIGHT('0000000'+CAST(@ref_count as VARCHAR(20)),7))
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
	,0
	,'@_TR_Date'
	,GETDATE()
	,'Bill Added'
	,@_Consumption
	,@_ExcessConsumption
	,@_dueWithDiscount
	,@_CurrentReading
	,@_PreviousReading
);



INSERT Into tblBillDiscount
(
	SLC_CODE
	,SLT_CODE
	,COAID
	,Description
	,ClientID
	,ReferenceNo
	,Amount
)
Values
@_discountValue
;


INSERT Into tblBillCharges
(
	SLC_CODE
	,SLT_CODE
	,COAID
	,Description
	,ClientID
	,ReferenceNo
	,Amount
)
Values
@_ChargesValue
;