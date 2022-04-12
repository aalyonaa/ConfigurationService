CREATE procedure [dbo].[GetAllMicroservices]
	as
	select S.[Id], S.[ServiceName], S.[URL], S.[Address] from dbo.[Microservices] as S