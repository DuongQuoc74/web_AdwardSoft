create procedure [dbo].[usp_VehicleType_ReadById]
@Id int
as 
select*
from [dbo].[VehicleType]
where [Id] = @Id