CREATE TABLE [dbo].[Microservices] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [ServiceName] NVARCHAR (50)  NULL,
    [URL]         NVARCHAR (MAX) NULL,
    [IsDeleted]   BIT            NULL,
    [Address]     NVARCHAR (20)  NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

