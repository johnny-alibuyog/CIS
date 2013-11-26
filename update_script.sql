use [cisdb]
go

if col_length('Polices.Clearances', 'PerfectMatchFindings') is not null
	alter table Polices.Clearances drop column PerfectMatchFindings;
go

if col_length('Polices.Clearances', 'PartialMatchFindings') is not null
	alter table Polices.Clearances drop column PartialMatchFindings;
go

if col_length('Polices.Clearances', 'PurposeId') is not null 
 begin
	update Polices.Clearances
	set PurposeId = (
		select top 1 a.PurposeID 
		from Polices.Applicants as a
		where a.ApplicantId = Polices.Clearances.ApplicantId
	)
	where PurposeId is null;
 end
go


if col_length('Polices.Applicants', 'PurposeId') is not null 
begin

Declare @TABLENAME varchar(max), @COLUMN varchar(max)
SET @TABLENAME = 'Polices.Applicants'
SET @COLUMN = 'PurposeId'
Declare @CONSTRAINT varchar(max)
set @CONSTRAINT ='ALTER TABLE '+@TABLENAME+' DROP CONSTRAINT '
set @CONSTRAINT = @CONSTRAINT + (select SYS_OBJ.name as CONSTRAINT_NAME
from sysobjects SYS_OBJ
join syscomments SYS_COM on SYS_OBJ.id = SYS_COM.id
join sysobjects SYS_OBJx on SYS_OBJ.parent_obj = SYS_OBJx.id 
join sysconstraints SYS_CON on SYS_OBJ.id = SYS_CON.constid
join syscolumns SYS_COL on SYS_OBJx.id = SYS_COL.id
and SYS_CON.colid = SYS_COL.colid
where
SYS_OBJ.uid = user_id() and SYS_OBJ.xtype = 'D'
and SYS_OBJx.name=@TABLENAME and SYS_COL.name=@COLUMN)
select @CONSTRAINT;
exec(@CONSTRAINT)

DECLARE @ConstraintName nvarchar(200)
SELECT @ConstraintName = Name FROM SYS.DEFAULT_CONSTRAINTS WHERE PARENT_OBJECT_ID = OBJECT_ID('__TableName__') AND PARENT_COLUMN_ID = (SELECT column_id FROM sys.columns WHERE NAME = N'__ColumnName__' AND object_id = OBJECT_ID(N'__TableName__'))
IF @ConstraintName IS NOT NULL
    EXEC('ALTER TABLE __TableName__ DROP CONSTRAINT ' + @ConstraintName)

--Declare @TABLENAME varchar(max), @COLUMN varchar(max)
--SET @TABLENAME = 'Polices.Applicants'
--SET @COLUMN = 'PurposeId'
--Declare @CONSTRAINT varchar(max)
--                    set @CONSTRAINT ='ALTER TABLE '+@TABLENAME+' DROP CONSTRAINT '
--                    set @CONSTRAINT = @CONSTRAINT + (select 
--	SYS_OBJ.name as CONSTRAINT_NAME
--from 
--	sysobjects SYS_OBJ
----inner join 
----	syscomments SYS_COM 
----		on SYS_OBJ.id = SYS_COM.id
--inner join 
--	sysobjects SYS_OBJx 
--		on SYS_OBJ.parent_obj = SYS_OBJx.id 
--inner join 
--	sysconstraints SYS_CON 
--		on SYS_OBJ.id = SYS_CON.constid
--inner join 
--	syscolumns SYS_COL 
--		on SYS_OBJx.id = SYS_COL.id and SYS_CON.colid = SYS_COL.colid
--                    where
--                    SYS_OBJ.uid = user_id() and SYS_OBJ.xtype = 'D'
--                    and SYS_OBJx.name=@TABLENAME and SYS_COL.name=@COLUMN)
--                    select @CONSTRAINT;
--                    exec(@CONSTRAINT)
                    
                    
	--declare @ConstraintName nvarchar(200)
	
	--select 
	--	@ConstraintName = Name 
	--from 
	--	sys.default_constraints 
	--where 
	--	parent_object_id = object_id('__Polices.Applicants__') and 
	--	parent_column_id = (select column_id from sys.columns where name = '__PurposeId__' and object_id = object_id('__Polices.Applicants__'))
	
	--select @ConstraintName
	
	--if @ConstraintName is not null
	--	exec('alter table __Polices.Applicants__ drop constraint ' + @ConstraintName)
		
	--alter table Polices.Clearances drop column PurposeId;
end