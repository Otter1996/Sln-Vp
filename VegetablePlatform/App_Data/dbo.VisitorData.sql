CREATE TABLE [dbo].[VisitorData] (
    [Id]       INT        NOT NULL IDENTITY,
    [account]  NCHAR (10) NULL,
    [password] NCHAR (10) NULL,
    [remindme] NCHAR(10)        NULL DEFAULT Null,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

