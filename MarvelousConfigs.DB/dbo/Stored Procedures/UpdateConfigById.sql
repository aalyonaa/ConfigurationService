	CREATE procedure [dbo].[UpdateConfigById]
	@Id integer,
	@Key nvarchar(max), 
	@Value nvarchar(50),
	@ServiceId integer,
	@Description nvarchar(max)
	as
	update dbo.[Configs]
	set
	[Key] = @Key, [Value] = @Value, ServiceId = @ServiceId, Updated = SYSDATETIME(), Description = @Description
	where Id = @Id