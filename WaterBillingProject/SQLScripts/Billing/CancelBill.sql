DECLARE @TransYear bigint
SET @TransYear = (
					SELECT TOP 1 TransYear FROM tblTransactionDetails
					WHERE ClientID = @_ClientID
					AND TransactionCode = @_TR_CODE
					AND CTLNo = @_CTLNo
					AND BillMonth = @_BillMonth
				  )


UPDATE tblBilling
SET BillStatus = 3
WHERE ClientID = @_ClientID
AND TR_CODE = @_TR_CODE
AND CTLNo = @_CTLNo
AND BillMonth = @_BillMonth
;


DELETE FROM tblTransactionDetails
WHERE ClientID = @_ClientID
AND TransactionCode = @_TR_CODE
AND CTLNo = @_CTLNo
AND BillMonth = @_BillMonth


DELETE FROM tblTransactionSummary
WHERE  TransactionCode = @_TR_CODE
AND CTLNo = @_CTLNo
AND TransYear = @TransYear