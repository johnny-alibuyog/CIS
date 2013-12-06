USE [cisdb]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[Polices].[FK200EBCEC8A5BC808]') AND parent_object_id = OBJECT_ID(N'[Polices].[ClearancesExpiredLicenseMatches]'))
ALTER TABLE [Polices].[ClearancesExpiredLicenseMatches] DROP CONSTRAINT [FK200EBCEC8A5BC808]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[Polices].[FK200EBCECE70424E]') AND parent_object_id = OBJECT_ID(N'[Polices].[ClearancesExpiredLicenseMatches]'))
ALTER TABLE [Polices].[ClearancesExpiredLicenseMatches] DROP CONSTRAINT [FK200EBCECE70424E]
GO

/****** Object:  Table [Polices].[ClearancesExpiredLicenseMatches]    Script Date: 11/21/2013 00:56:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Polices].[ClearancesExpiredLicenseMatches]') AND type in (N'U'))
DROP TABLE [Polices].[ClearancesExpiredLicenseMatches]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[Polices].[FK4179054CC48E921F]') AND parent_object_id = OBJECT_ID(N'[Polices].[ClearancesSuspectPartialMatches]'))
ALTER TABLE [Polices].[ClearancesSuspectPartialMatches] DROP CONSTRAINT [FK4179054CC48E921F]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[Polices].[FK4179054CE70424E]') AND parent_object_id = OBJECT_ID(N'[Polices].[ClearancesSuspectPartialMatches]'))
ALTER TABLE [Polices].[ClearancesSuspectPartialMatches] DROP CONSTRAINT [FK4179054CE70424E]
GO

/****** Object:  Table [Polices].[ClearancesSuspectPartialMatches]    Script Date: 11/21/2013 00:56:37 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Polices].[ClearancesSuspectPartialMatches]') AND type in (N'U'))
DROP TABLE [Polices].[ClearancesSuspectPartialMatches]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[Polices].[FK25B4F343C48E921F]') AND parent_object_id = OBJECT_ID(N'[Polices].[ClearancesSuspectPerfectMatches]'))
ALTER TABLE [Polices].[ClearancesSuspectPerfectMatches] DROP CONSTRAINT [FK25B4F343C48E921F]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[Polices].[FK25B4F343E70424E]') AND parent_object_id = OBJECT_ID(N'[Polices].[ClearancesSuspectPerfectMatches]'))
ALTER TABLE [Polices].[ClearancesSuspectPerfectMatches] DROP CONSTRAINT [FK25B4F343E70424E]
GO

/****** Object:  Table [Polices].[ClearancesSuspectPerfectMatches]    Script Date: 11/21/2013 00:56:53 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Polices].[ClearancesSuspectPerfectMatches]') AND type in (N'U'))
DROP TABLE [Polices].[ClearancesSuspectPerfectMatches]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[Memberships].[FK6209393F1D2C829C]') AND parent_object_id = OBJECT_ID(N'[Memberships].[UsersRoles]'))
ALTER TABLE [Memberships].[UsersRoles] DROP CONSTRAINT [FK6209393F1D2C829C]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[Memberships].[FK6209393F4946E26E]') AND parent_object_id = OBJECT_ID(N'[Memberships].[UsersRoles]'))
ALTER TABLE [Memberships].[UsersRoles] DROP CONSTRAINT [FK6209393F4946E26E]
GO

/****** Object:  Table [Memberships].[UsersRoles]    Script Date: 11/21/2013 00:58:06 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Memberships].[UsersRoles]') AND type in (N'U'))
DROP TABLE [Memberships].[UsersRoles]
GO

/****** Object:  Table [Memberships].[Roles]    Script Date: 11/21/2013 00:58:26 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Memberships].[Roles]') AND type in (N'U'))
DROP TABLE [Memberships].[Roles]
GO


IF COL_LENGTH('Polices.Clearances', 'PerfectMatchFindings') IS NOT NULL
ALTER TABLE Polices.Clearances DROP COLUMN PerfectMatchFindings;
GO

IF COL_LENGTH('Polices.Clearances', 'PartialMatchFindings') IS NOT NULL
ALTER TABLE Polices.Clearances DROP COLUMN PartialMatchFindings;
GO

IF COL_LENGTH('Polices.Clearances', 'PurposeId') IS NOT NULL 
BEGIN
	UPDATE Polices.Clearances
	SET PurposeId = (
		SELECT TOP 1 a.PurposeID 
		FROM Polices.Applicants AS a
		WHERE a.ApplicantId = Polices.Clearances.ApplicantId
	)
	WHERE PurposeId IS NULL;

	IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IDX_FK661016535A9CAD77')
	  EXEC('USE cisdb; DROP INDEX Polices.Applicants.IDX_FK661016535A9CAD77')

	ALTER TABLE Polices.Applicants DROP FK661016535A9CAD77
	ALTER TABLE Polices.Applicants DROP COLUMN PurposeId;
END
GO


IF COL_LENGTH('Polices.Applicants', 'PictureId') IS NOT NULL 
BEGIN
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Polices].[ApplicantsPictures]') AND type in (N'U'))
	BEGIN
		INSERT INTO Polices.ApplicantsPictures
		SELECT ApplicantId, PictureId FROM Polices.Applicants
		
		DROP INDEX [IDX_FK661016536F71D173] ON [Polices].[Applicants] WITH ( ONLINE = OFF )
		ALTER TABLE [Polices].[Applicants] DROP CONSTRAINT [FK661016536F71D173]
		ALTER TABLE [Polices].[Applicants] DROP COLUMN [PictureId];
	END
END
GO

IF COL_LENGTH('Polices.Applicants', 'SignatureId') IS NOT NULL 
BEGIN
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Polices].[ApplicantsSignatures]') AND type in (N'U'))
	BEGIN
		INSERT INTO Polices.ApplicantsSignatures
		SELECT ApplicantId, SignatureId FROM Polices.Applicants
		
		DROP INDEX [IDX_FK661016535A314BAE] ON [Polices].[Applicants] WITH ( ONLINE = OFF )
		ALTER TABLE [Polices].[Applicants] DROP CONSTRAINT [FK661016535A314BAE]
		ALTER TABLE [Polices].[Applicants] DROP COLUMN [SignatureId];
	END
END
GO

DELETE FROM Memberships.Users 