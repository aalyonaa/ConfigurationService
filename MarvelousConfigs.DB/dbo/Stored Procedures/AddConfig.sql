CREATE procedure [dbo].[AddConfig] 
	@Key nvarchar(max), 
	@Value nvarchar(50),  
	@ServiceId integer,
	@Description nvarchar(20)
	as
	insert into dbo.[Configs]
	values
	(@Key, @Value, @ServiceId, SYSDATETIME(), null, 0, @Description)
	select SCOPE_IDENTITY()