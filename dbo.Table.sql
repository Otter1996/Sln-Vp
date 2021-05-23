CREATE TABLE [dbo].[UserData]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [account] NVARCHAR(50) NULL, 
    [password] NVARCHAR(50) NULL, 
    [email] NVARCHAR(50) NULL, 
    [emailverify] BIT NOT NULL DEFAULT 0, 
    [phone] NVARCHAR(50) NULL, 
    [phoneverify] BIT NOT NULL DEFAULT 0, 
    [name] NVARCHAR(50) NOT NULL, 
    [emailtochangepasswordnumber] NVARCHAR(50) NULL DEFAULT NULL
)
