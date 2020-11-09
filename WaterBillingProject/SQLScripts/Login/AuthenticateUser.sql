SELECT Userid,
	   Username 
FROM tblUser
	WHERE Username = '@_Username'
	and Password = '@_Password'