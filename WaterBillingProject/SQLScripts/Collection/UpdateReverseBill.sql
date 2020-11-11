UPDATE tblBilling
	SET BillStatus = 1
WHERE ClientID = @_ClientID
AND ReferenceNo IN (
						select ReferenceNo 
						from tblBilling bill
						where ClientID = @_ClientID
						and (
								SELECT SUM(Amt) from tblTransactionDetails td
									WHERE td.SLC_CODE = bill.SLC_CODE
										and td.SLT_CODE = bill.SLT_CODE
										and td.ReferenceNo = bill.ReferenceNo
										and td.ClientID = bill.ClientID
							) != 0
					)