use cisdb;
go

--select 
--	EvaluatedCivilStatus = 
--		case Gender
--			when 'Male' then 'Widower'
--			when 'Female'then 'Widow'
--			else CivilStatus
--		end, 
--	Gender = Gender,
--	CivilStatus = CivilStatus
--from 
--	Polices.Applicants
--where 
--	CivilStatus = 'Widowed';


update 
	Polices.Applicants 
set 
	CivilStatus = 
		case Gender
			when 'Male' then 'Widower'
			when 'Female'then 'Widow'
			else CivilStatus
		end
where 
	(CivilStatus = 'Widowed') or
	(CivilStatus = 'Widower' and Gender = 'Female') or
	(CivilStatus = 'Widow' and Gender = 'Male');

select 
	Gender, 
	CivilStatus 
from 
	Polices.Applicants 
where 
	CivilStatus like 'Widow%';