﻿CREATE procedure [dbo].[GetConfigById]
	@Id integer
	as
	select C.Id, C.[Key], C.[Value], C.[Description], C.[ServiceId], C.[Created], C.[Updated]  from dbo.[Configs] as C
	where Id = @Id