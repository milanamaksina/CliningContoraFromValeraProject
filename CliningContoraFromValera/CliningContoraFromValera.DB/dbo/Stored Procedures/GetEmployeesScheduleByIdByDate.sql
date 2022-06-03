﻿CREATE PROCEDURE [dbo].[GetEmployeesScheduleByIdByDate]
@EmployeeId int,
@Date date
AS
BEGIN
	  SELECT O.Id, O.[Date], O.StartTime,  O.EstimatedEndTime, O.FinishTime, O.Price, O.[Status], O.CountOfEmployees,
	  O.IsCommercial, O.ClientId, O.AddressId FROM [dbo].[Order] AS O
	  join dbo.[Employee_Order] as EO on EO.OrderId = O.Id
      WHERE EO.EmployeeId = @EmployeeId AND O.[Date] = @Date AND O.IsDeleted = 0
END
