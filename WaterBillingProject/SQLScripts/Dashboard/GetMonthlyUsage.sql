select 
	m.MonthDescription,
	SUM(COALESCE(Consumption,0)) as Consumption 
from tblMonths m
LEFT JOIN tblBilling b
	ON RIGHT(b.BillMonth,2) = m.MonthID AND YEAR(b.TR_Date) = @_Year AND BillStatus != 3
GROUP BY m.MonthID ,m.MonthDescription
ORDER BY m.MonthID 