	CREATE procedure [dbo].[UpdateConfigById]
	@Id integer,
	@Value nvarchar(50),
	@Description nvarchar(max)
	as
	update dbo.[Configs]
	set
	[Value] = @Value, Updated = SYSDATETIME(), Description = @Description
	where Id = @Id