CREATE procedure [dbo].[UpdateMicroserviceById]
	@Id integer,
	@ServiceName nvarchar(50), 
	@URL nvarchar(max),
	@Address nvarchar(20)
	as
	update dbo.[Microservices]
	set
	ServiceName = @ServiceName, [URL] = @URL, [Address] = @Address
	where Id = @Id