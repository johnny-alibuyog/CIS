USE [master]
GO

/****** Object:  Database [cisdb]    Script Date: 01/09/2013 16:49:50 ******/
IF  EXISTS (SELECT name FROM sys.databases WHERE name = N'cisdb')
DROP DATABASE [cisdb]
GO

USE [master]
GO

/****** Object:  Database [cisdb]    Script Date: 01/09/2013 16:49:50 ******/
CREATE DATABASE [cisdb] ON  PRIMARY 
( NAME = N'cisdb', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\cisdb.mdf' , SIZE = 2048KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'cisdb_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\cisdb_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

ALTER DATABASE [cisdb] SET COMPATIBILITY_LEVEL = 100
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [cisdb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [cisdb] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [cisdb] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [cisdb] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [cisdb] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [cisdb] SET ARITHABORT OFF 
GO

ALTER DATABASE [cisdb] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [cisdb] SET AUTO_CREATE_STATISTICS ON 
GO

ALTER DATABASE [cisdb] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [cisdb] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [cisdb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [cisdb] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [cisdb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [cisdb] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [cisdb] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [cisdb] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [cisdb] SET  DISABLE_BROKER 
GO

ALTER DATABASE [cisdb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [cisdb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [cisdb] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [cisdb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [cisdb] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [cisdb] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [cisdb] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [cisdb] SET  READ_WRITE 
GO

ALTER DATABASE [cisdb] SET RECOVERY FULL 
GO

ALTER DATABASE [cisdb] SET  MULTI_USER 
GO

ALTER DATABASE [cisdb] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [cisdb] SET DB_CHAINING OFF 
GO

USE [cisdb];
GO

/****** Object:  Schema [Memberships]    Script Date: 01/09/2013 16:22:26 ******/
IF  EXISTS (SELECT * FROM sys.schemas WHERE name = N'Memberships')
DROP SCHEMA [Memberships]
GO

/****** Object:  Schema [Memberships]    Script Date: 01/09/2013 16:22:26 ******/
CREATE SCHEMA [Memberships] AUTHORIZATION [dbo]
GO

/****** Object:  Schema [Polices]    Script Date: 01/09/2013 16:22:26 ******/
IF  EXISTS (SELECT * FROM sys.schemas WHERE name = N'Polices')
DROP SCHEMA [Polices]
GO

/****** Object:  Schema [Polices]    Script Date: 01/09/2013 16:22:26 ******/
CREATE SCHEMA [Polices] AUTHORIZATION [dbo]
GO

/****** Object:  Schema [Commons]    Script Date: 01/09/2013 16:22:26 ******/
IF  EXISTS (SELECT * FROM sys.schemas WHERE name = N'Commons')
DROP SCHEMA [Commons]
GO

/****** Object:  Schema [Commons]    Script Date: 01/09/2013 16:22:26 ******/
CREATE SCHEMA [Commons] AUTHORIZATION [dbo]
GO

/****** Object:  Schema [[Barangays]]    Script Date: 01/09/2013 16:22:26 ******/
IF  EXISTS (SELECT * FROM sys.schemas WHERE name = N'Barangays')
DROP SCHEMA [Barangays]
GO

/****** Object:  Schema [Barangays]    Script Date: 01/09/2013 16:22:26 ******/
CREATE SCHEMA [Barangays] AUTHORIZATION [dbo]
GO


/****** Object:  Schema [Mayors]    Script Date: 01/09/2013 16:22:26 ******/
IF  EXISTS (SELECT * FROM sys.schemas WHERE name = N'Mayors')
DROP SCHEMA Mayors
GO

/****** Object:  Schema [Mayors]    Script Date: 01/09/2013 16:22:26 ******/
CREATE SCHEMA Mayors AUTHORIZATION [dbo]
GO

/****** Object:  Schema [Firearms]    Script Date: 01/09/2013 16:22:26 ******/
IF  EXISTS (SELECT * FROM sys.schemas WHERE name = N'Firearms')
DROP SCHEMA Firearms
GO

/****** Object:  Schema [Firearms]    Script Date: 01/09/2013 16:22:26 ******/
CREATE SCHEMA Firearms AUTHORIZATION [dbo]
GO
