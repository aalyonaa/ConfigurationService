CREATE procedure [dbo].[AddMicroservice] 
	@ServiceName nvarchar(50), 
	@URL nvarchar(max),
	@Address nvarchar(20)
	as
	insert into dbo.[Microservices]
	values
	(@ServiceName, @URL, 0, @Address)
	select @@IDENTITY