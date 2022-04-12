CREATE TABLE [dbo].[Configs] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Key]         NVARCHAR (MAX) NULL,
    [Value]       NVARCHAR (255)  NULL,
    [ServiceId]   INT            NULL,
    [Created]     DATETIME       NULL,
    [Updated]     DATETIME       NULL,
    [Description] VARCHAR (255)  NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([ServiceId]) REFERENCES [dbo].[Microservices] ([Id])
);

