SELECT distinct number
FROM master..spt_values
WHERE number BETWEEN 1 and (SELECT max(id) FROM MyTable)
AND number NOT IN (SELECT id FROM MyTable)








use cisdb;
go
--declare @p0 as bigint = 100, 
--	    @p1 as bigint = 100;
	    
--    select
--        TOP (@p0)  suspect0_.SuspectId as SuspectId28_0_,
--        warrant1_.WarrantId as WarrantId31_1_,
--        suspect0_.DataStoreId as DataStor2_28_0_,
--        suspect0_.DataStoreChildKey as DataStor3_28_0_,
--        suspect0_.ArrestStatus as ArrestSt4_28_0_,
--        suspect0_.ArrestDate as ArrestDate28_0_,
--        suspect0_.Disposition as Disposit6_28_0_,
--        suspect0_.WarrantId as WarrantId28_0_,
--        suspect0_.CreatedBy as CreatedBy28_0_,
--        suspect0_.UpdatedBy as UpdatedBy28_0_,
--        suspect0_.CreatedOn as CreatedOn28_0_,
--        suspect0_.UpdatedOn as UpdatedOn28_0_,
--        suspect0_.FirstName as FirstName28_0_,
--        suspect0_.MiddleName as MiddleName28_0_,
--        suspect0_.LastName as LastName28_0_,
--        suspect0_.Suffix as Suffix28_0_,
--        suspect0_.Gender as Gender28_0_,
--        suspect0_.BirthDate as BirthDate28_0_,
--        suspect0_.Address1 as Address18_28_0_,
--        suspect0_.Address2 as Address19_28_0_,
--        suspect0_.Barangay as Barangay28_0_,
--        suspect0_.City as City28_0_,
--        suspect0_.Province as Province28_0_,
--        suspect0_.Hair as Hair28_0_,
--        suspect0_.Eyes as Eyes28_0_,
--        suspect0_.Complexion as Complexion28_0_,
--        suspect0_.Build as Build28_0_,
--        suspect0_.ScarsAndMarks as ScarsAn27_28_0_,
--        suspect0_.Race as Race28_0_,
--        suspect0_.Nationality as Nationa29_28_0_,
--        aliases2_.SuspectId as SuspectId0__,
--        aliases2_.Name as Name0__,
--        warrant1_.DataStoreParentKey as DataStor2_31_1_,
--        warrant1_.WarrantCode as WarrantC3_31_1_,
--        warrant1_.CaseNumber as CaseNumber31_1_,
--        warrant1_.Crime as Crime31_1_,
--        warrant1_.Description as Descript6_31_1_,
--        warrant1_.Remarks as Remarks31_1_,
--        warrant1_.BailAmount as BailAmount31_1_,
--        warrant1_.IssuedOn as IssuedOn31_1_,
--        warrant1_.IssuedBy as IssuedBy31_1_,
--        warrant1_.CreatedBy as CreatedBy31_1_,
--        warrant1_.UpdatedBy as UpdatedBy31_1_,
--        warrant1_.CreatedOn as CreatedOn31_1_,
--        warrant1_.UpdatedOn as UpdatedOn31_1_,
--        warrant1_.IssuedAtAddress1 as IssuedA15_31_1_,
--        warrant1_.IssuedAtAddress2 as IssuedA16_31_1_,
--        warrant1_.IssuedAtBarangay as IssuedA17_31_1_,
--        warrant1_.IssuedAtCity as IssuedA18_31_1_,
--        warrant1_.IssuedAtProvince as IssuedA19_31_1_,
--        aliases2_.SuspectId as SuspectId0__,
--        aliases2_.Name as Name0__ 
--    from
--        Polices.Suspects suspect0_ 
--    left outer join
--        Polices.Warrants warrant1_ 
--            on suspect0_.WarrantId=warrant1_.WarrantId 
--    left outer join
--        Polices.SuspectAliases aliases2_ 
--            on suspect0_.SuspectId=aliases2_.SuspectId 
--    where
--        suspect0_.DataStoreId is null;
--    select
--        TOP (@p1)  suspect0_.SuspectId as SuspectId28_,
--        suspect0_.DataStoreId as DataStor2_28_,
--        suspect0_.DataStoreChildKey as DataStor3_28_,
--        suspect0_.ArrestStatus as ArrestSt4_28_,
--        suspect0_.ArrestDate as ArrestDate28_,
--        suspect0_.Disposition as Disposit6_28_,
--        suspect0_.WarrantId as WarrantId28_,
--        suspect0_.CreatedBy as CreatedBy28_,
--        suspect0_.UpdatedBy as UpdatedBy28_,
--        suspect0_.CreatedOn as CreatedOn28_,
--        suspect0_.UpdatedOn as UpdatedOn28_,
--        suspect0_.FirstName as FirstName28_,
--        suspect0_.MiddleName as MiddleName28_,
--        suspect0_.LastName as LastName28_,
--        suspect0_.Suffix as Suffix28_,
--        suspect0_.Gender as Gender28_,
--        suspect0_.BirthDate as BirthDate28_,
--        suspect0_.Address1 as Address18_28_,
--        suspect0_.Address2 as Address19_28_,
--        suspect0_.Barangay as Barangay28_,
--        suspect0_.City as City28_,
--        suspect0_.Province as Province28_,
--        suspect0_.Hair as Hair28_,
--        suspect0_.Eyes as Eyes28_,
--        suspect0_.Complexion as Complexion28_,
--        suspect0_.Build as Build28_,
--        suspect0_.ScarsAndMarks as ScarsAn27_28_,
--        suspect0_.Race as Race28_,
--        suspect0_.Nationality as Nationa29_28_,
--        occupation1_.SuspectId as SuspectId0__,
--        occupation1_.Value as Value0__ 
--    from
--        Polices.Suspects suspect0_ 
--    left outer join
--        Polices.SuspectOccupations occupation1_ 
--            on suspect0_.SuspectId=occupation1_.SuspectId 
--    where
--        suspect0_.DataStoreId is null;


select
	s.FirstName, 
	s.MiddleName, 
	s.LastName, 
	w.Crime,
	count(s.SuspectId)
from 
	Polices.Suspects as s
inner join
	Polices.Warrants as w
		on s.WarrantId = w.WarrantId
group by
	s.FirstName, s.MiddleName, s.LastName, w.Crime
having 
	count(s.SuspectId) > 1
order by 
	count(s.SuspectId) desc
	
 
select 
	s.FirstName,
	s.MiddleName,
	s.LastName,
	w.Crime,
	w.CaseNumber,
	w.WarrantCode,
	w.WarrantId,
	s.CreatedOn
from 
	cisdb.Polices.Suspects as s
left outer join
	cisdb.Polices.Warrants as w
		on s.WarrantId = w.WarrantId
where 
	s.FirstName = 'Elda Myrah' and
	s.MiddleName = 'N' and
	s.LastName = 'Ramillano'	

	
select 
	s.FirstName,
	s.MiddleName,
	s.LastName,
	w.Crime,
	w.CaseNumber,
	w.WarrantCode,
	w.WarrantId,
	s.CreatedOn
from 
	Polices.Suspects as s
inner join
	Polices.Warrants as w
		on s.WarrantId = w.WarrantId
where 
	DataStoreId is null








	 --select distinct DataStoreId from cisdb.Polices.Suspects 
 
-- UPDATE [Polices].[Suspects]
--    SET [DataStoreId] = null,
--		[DataStoreChildKey] = null
--  WHERE [DataStoreId] between 208 and 506

--update Polices.Warrants
--   set DataStoreParentKey = null
-- where exists (select SuspectId from Polices.Suspects as s where Polices.Warrants.WarrantId = s.WarrantId and s.DataStoreId is null)
	   
--select w.DataStoreParentKey from Polices.Warrants as w
-- where exists (select SuspectId from Polices.Suspects as s where w.WarrantId = s.WarrantId and s.DataStoreId between 208 and 506)


