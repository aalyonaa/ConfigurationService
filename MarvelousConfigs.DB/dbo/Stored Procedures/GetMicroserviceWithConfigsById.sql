	CREATE procedure [dbo].[GetMicroserviceWithConfigsById]
	@Id integer
	as
	select S.Id, S.ServiceName, S.[URL], C.Id, C.[Key], C.[Value], C.Description, C.Created, C.Updated from dbo.[Microservices] as S
	inner join dbo.[Configs] as C
	on C.ServiceId = S.Id
	where S.Id = @Id