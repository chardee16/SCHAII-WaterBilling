UPDATE tblClient SET
	FirstName = '@_FirstName',
    MiddleName = '@_MiddleName',
    LastName = '@_LastName',
    AccountStatusID = @_ClientAccountStatusID,
    BlockNo = @_BlockNo,
    LotNo = @_LotNo,
    Occupants = @_Occupants,
    IsSenior = '@_IsSenior',
    SeniorCount = @_SeniorCount,
    PreviousReading = @_PreviousReading
WHERE ClientID = @_ClientID
;