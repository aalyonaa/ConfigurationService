CREATE procedure [dbo].[DeleteOrRestoreMicroserviceById]
	@Id integer,
	@IsDeleted bit
	as
	update dbo.[Microservices]
	set IsDeleted = @IsDeleted
	where Id = @Id