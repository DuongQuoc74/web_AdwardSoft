create PROCEDURE [dbo].[usp_VehicleType_Update]
@Id int,
@Name nvarchar (120),
@Status tinyint
as
begin 
set nocount on
set transaction isolation level read committed

begin try

	begin tran 
		update [dbo].[VehicleType]
		set [Name] = @Name,
			[Status] = @Status
			where [Id] = @Id
		commit
		select VT .* 
		from [dbo].[VehicleType] as VT
		where VT.[Id] = @Id
	end try
	begin catch 
		rollback;
		select 0;
		throw;
	end catch
end