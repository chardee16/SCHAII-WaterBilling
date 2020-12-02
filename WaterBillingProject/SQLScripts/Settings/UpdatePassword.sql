UPDATE tblUser SET
	Password = '@_Password'
WHERE UserID = @_UserID
;