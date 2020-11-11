
SELECT IIF(ISNULL( (SELECT TOP 1 COALESCE(MAX(CurrentReading),0) as PreviousReading from tblBilling
WHERE ClientID = @_ClientID AND BillStatus != 3), '0') = 0,
	(SELECT PreviousReading FROM tblClient 
	WHERE ClientID = @_ClientID),
	ISNULL( (SELECT TOP 1 COALESCE(MAX(CurrentReading),0) as PreviousReading from tblBilling
	WHERE ClientID = @_ClientID AND BillStatus != 3), '0')) 
as PreviousReading;