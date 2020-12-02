SELECT COALESCE(Max(UserID),0) + 1 as UserID 
INTO #TEMPUserID
FROM tblUser 


DECLARE 
@UserID bigint
;
SET @UserID = (SELECT UserID FROM #TEMPUserID)


Insert Into tblUser
(
    UserID
    ,Username
    ,Password
	,Firstname
    ,Middlename
    ,Lastname
    ,IsActive
    ,IsAdmin
    ,IsReset
)
Values
(
    @UserID,
    '@_Username',
    '@_Password',
    '@_FirstName',
   '@_MiddleName',
   '@_LastName',
   1,
   '@_IsAdmin'
   ,0
)



DROP TABLE #TEMPUserID;