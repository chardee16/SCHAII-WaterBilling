SELECT COUNT(*) as counting FROM tblClient
where FirstName LIKE '%@_FirstName%'
and LastName LIKE '%@_LastName%'
and BlockNo  = @_BlockNo
and LotNo = @_LotNo