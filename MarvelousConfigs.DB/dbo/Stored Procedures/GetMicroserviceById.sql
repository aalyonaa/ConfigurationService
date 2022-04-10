CREATE procedure [dbo].[GetMicroserviceById]
	@Id integer
	as
	select S.[Id], S.[ServiceName], S.[URL] from dbo.[Microservices] as S
	where Id = @Id