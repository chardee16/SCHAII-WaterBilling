SELECT Userid,
	   Username,
	   IsAdmin,
	   s1.StaticValue as MinimumBill,
	   s2.StaticValue as MinimumConsumption,
	   s3.StaticValue as ExcessPerCubic
FROM tblUser
INNER JOIN tblStatic s1
	ON s1.StaticID = 1
INNER JOIN tblStatic s2
	ON s2.StaticID = 2
INNER JOIN tblStatic s3
	ON s3.StaticID = 3
WHERE Username = '@_Username'
and Password = '@_Password'