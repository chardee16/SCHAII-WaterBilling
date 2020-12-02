UPDATE tblUser SET
	FirstName = '@_FirstName',
    MiddleName = '@_MiddleName',
    LastName = '@_LastName',
    IsAdmin = '@_IsAdmin'
WHERE UserID = @_UserID
;