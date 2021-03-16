Select c.ClientID,
		c.FirstName,
		c.MiddleName,
		c.LastName,
		c.AccountStatusID as ClientAccountStatusID,
		c.BlockNo,
		c.LotNo,
		c.Occupants,
		c.IsSenior,
		c.SeniorCount,
		c.LastName + ', ' + c.FirstName + ' ' + c.MiddleName as FullName,
		'Blk ' + Cast(c.BlockNo as varchar) + ' and Lot ' + Cast(c.LotNo as varchar) as FullAddress,
		cs.ClientStatusDesc as ClientStatusDesc,
		c.PreviousReading
from tblClient c
Inner Join tblClientStatus cs
	ON cs.ClientStatusID = c.AccountStatusID
ORDER BY c.BlockNo,c.LotNo