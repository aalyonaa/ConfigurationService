CREATE procedure [dbo].[GetConfigsByServiceId]
	@ServiceId integer
	as
	select C.[Id], C.[Key], C.[Description], C.[Value], C.[ServiceId], C.[Created], C.[Updated]
	from [dbo].[Configs] as C
	where C.ServiceId = @ServiceId 