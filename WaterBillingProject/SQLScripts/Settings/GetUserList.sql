Select
	UserID,
	Username,
	Password,
	Firstname,
	Middlename,
	Lastname,
	IsAdmin,
	Firstname + ' ' + Middlename + ' ' + Lastname as FullName
from tblUser
