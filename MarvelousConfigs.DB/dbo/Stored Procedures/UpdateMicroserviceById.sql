CREATE procedure [dbo].[UpdateMicroserviceById]
	@Id integer,
	@URL nvarchar(max),
	@Address nvarchar(20)
	as
	update dbo.[Microservices]
	set
	[URL] = @URL, [Address] = @Address
	where Id = @Id