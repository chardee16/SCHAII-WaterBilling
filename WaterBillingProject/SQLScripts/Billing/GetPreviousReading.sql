SELECT ISNULL( (SELECT TOP 1 COALESCE(SUM(Consumption),0) as PreviousReading from tblBilling
WHERE ClientID = @_ClientID), '0') as PreviousReading;