UPDATE tblBilling
	SET BillStatus = 2
WHERE ClientID = @_ClientID
AND ReferenceNo IN (@_ReferenceNo)