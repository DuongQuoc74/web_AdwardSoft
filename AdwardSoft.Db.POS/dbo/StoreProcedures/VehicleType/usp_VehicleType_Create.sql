create procedure [dbo].[usp_VehicleType_Create]
@Id int,
@Name nvarchar(120),
@Status tinyint
as
begin
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED
	BEGIN TRY
        BEGIN TRAN
			INSERT INTO [dbo].[VehicleType] ([Name],[Status]) values (@Name,@Status)
			set @Id = SCOPE_IDENTITY()
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