  SELECT TOP 1 COALESCE(MAX(ORNo),0) + 1 ORNo 
  FROM tblTransactionSummary