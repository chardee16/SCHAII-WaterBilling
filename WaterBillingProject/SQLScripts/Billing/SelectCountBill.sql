SELECT COUNT(BillMonth) as counting FROM tblBilling
where ClientID = @_ClientID
and BillMonth = '@_BillMonth'
and BillStatus != 3