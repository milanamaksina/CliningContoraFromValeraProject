﻿using Dapper;
using System.Data.SqlClient;
using CliningContoraFromValera.DAL.DTOs;

namespace CliningContoraFromValera.DAL.Managers
{
    public class EmployeeManager
    {
        public List<EmployeeDTO> GetAllEmployees()
        {
            using (var connection = new SqlConnection(ServerSettings._connectionString))
            {
                connection.Open();

                return connection.Query<EmployeeDTO>(
                    StoredProcedures.Employee_GetAll,
                    commandType: System.Data.CommandType.StoredProcedure)
                    .ToList();
            }
        }

        public EmployeeDTO GetEmployeeByID(int employeeId)
        {
            using (var connection = new SqlConnection(ServerSettings._connectionString))
            {
                connection.Open();

                return connection.QuerySingle<EmployeeDTO>(
                    StoredProcedures.Employee_GetById,
                    param: new { id = employeeId },
                    commandType: System.Data.CommandType.StoredProcedure
                    );
            }
        }

        public void AddEmployee(EmployeeDTO newEmployee)
        {
            using (var connection = new SqlConnection(ServerSettings._connectionString))
            {
                connection.Open();

                connection.QuerySingle<EmployeeDTO>(
                    StoredProcedures.Employee_Add,
                    param: new
                    {
                        newEmployee.FirstName,
                        newEmployee.LastName,
                        newEmployee.Phone
                    },
                    commandType: System.Data.CommandType.StoredProcedure
                    );
            }
        }

        public void UpdateEmployeeById(EmployeeDTO newEmployee)
        {
            using (var connection = new SqlConnection(ServerSettings._connectionString))
            {
                connection.Open();

                connection.QuerySingleOrDefault<EmployeeDTO>(
                    StoredProcedures.Employee_UpdateById,
                    param: new
                    {
                        newEmployee.Id,
                        newEmployee.FirstName,
                        newEmployee.LastName,
                        newEmployee.Phone
                    },
                    commandType: System.Data.CommandType.StoredProcedure
                    );
            }
        }

        public void DeleteEmployeeById(int employeeId)
        {
            using (var connection = new SqlConnection(ServerSettings._connectionString))
            {
                connection.Open();

                connection.QuerySingleOrDefault<EmployeeDTO>(
                    StoredProcedures.Employee_DeleteById,
                    param: new { id = employeeId },
                    commandType: System.Data.CommandType.StoredProcedure
                    );
            }
        }
        public void DeleteEmployeesOrder(int employeeId, int orderId)
        {
            using (var connection = new SqlConnection(ServerSettings._connectionString))
            {
                connection.Open();

                connection.QuerySingleOrDefault<EmployeeDTO>(
                    StoredProcedures.Employee_Order_DeleteByValue,
                    param: new { EmployeeId = employeeId, OrderId = orderId },
                    commandType: System.Data.CommandType.StoredProcedure
                    );
            }
        }

        public void DeleteEmployeesWorkArea(int employeeId, int workAreaId)
        {
            using (var connection = new SqlConnection(ServerSettings._connectionString))
            {
                connection.Open();

                connection.QuerySingleOrDefault<EmployeeDTO>(
                    StoredProcedures.Employee_WorkArea_DeleteByValue,
                    param: new { EmployeeId = employeeId, WorkAreaId = workAreaId },
                    commandType: System.Data.CommandType.StoredProcedure
                    );
            }
        }
        public void DeleteEmployeesService(int employeeId, int serviceId)
        {
            using (var connection = new SqlConnection(ServerSettings._connectionString))
            {
                connection.Open();

                connection.QuerySingleOrDefault<EmployeeDTO>(
                    StoredProcedures.Employee_Service_DeleteByValue,
                    param: new { EmployeeId = employeeId, ServiceId = serviceId },
                    commandType: System.Data.CommandType.StoredProcedure
                    );
            }
        }

        public EmployeeDTO GetAllEmployeesWorkAreasById(int employeeId)
        {
            using (var connection = new SqlConnection(ServerSettings._connectionString))
            {
                connection.Open();

                EmployeeDTO result = new EmployeeDTO();
                List<int>  workAreaId = new List<int>();
                connection.Query<EmployeeDTO, WorkAreaDTO, EmployeeDTO>(
                    StoredProcedures.GetEmployeesWorkAreasById,
                    (employee, workArea) => {
                        if (employee != null && result.Id != employee.Id)
                        {
                            result = employee;
                        }
                        if (result.WorkAreas == null)
                        {
                            result.WorkAreas = new List<WorkAreaDTO>();
                        }
                        if (workArea != null && !workAreaId.Contains(workArea.Id))
                        {
                            workAreaId.Add(workArea.Id);
                            result.WorkAreas!.Add(workArea);
                        }
                        return result;
                    },
                    param: new { id = employeeId },
                    commandType: System.Data.CommandType.StoredProcedure,
                    splitOn: "Id"
                );
                return result;
            }
        }

        public EmployeeDTO GetAllEmployeesServicesById(int employeeId)
        {
            using (var connection = new SqlConnection(ServerSettings._connectionString))
            {
                connection.Open();

                EmployeeDTO result = new EmployeeDTO();
                List<int> serviceId = new List<int>();
                connection.Query<EmployeeDTO, ServiceDTO, EmployeeDTO>(
                    StoredProcedures.GetEmployeesServicesById,
                    (employee, service) => {
                        if (employee != null && result.Id != employee.Id)
                        {
                            result = employee;
                        }
                        if (result.Services == null)
                        {
                            result.Services = new List<ServiceDTO>();
                        }
                        if (service != null && !serviceId.Contains(service.Id))
                        {
                            serviceId.Add(service.Id);
                            result.Services!.Add(service);
                        }
                        return result;
                    },
                    param: new { id = employeeId },
                    commandType: System.Data.CommandType.StoredProcedure,
                    splitOn: "Id"
                );
                return result;
            }
        }

        

        public List<EmployeeDTO> GetEmployyesAvailableForOrder(DateTime date, int serviceId, int workAreaId)
        {
            using (var connection = new SqlConnection(ServerSettings._connectionString))
            {
                connection.Open();
                Dictionary<int, EmployeeDTO> result = new Dictionary<int, EmployeeDTO>();
                connection.Query<EmployeeDTO, WorkTimeDTO, EmployeeDTO>(
                    StoredProcedures.GetEmployyesAvailableForOrder,
                    (employee, workTime) =>
                    {
                        if (!result.ContainsKey(employee.Id))
                        {
                            result.Add(employee.Id, employee);
                        }
                        EmployeeDTO crntEmployee = result[employee.Id];
                        if (crntEmployee.WorkTime == null)
                        {
                            crntEmployee.WorkTime = new WorkTimeDTO();
                        }
                        if (workTime != null)
                        {
                            crntEmployee.WorkTime = workTime;
                        }
                        return crntEmployee;
                    },
                    param: new { date = date, serviceId = serviceId, workAreaId = workAreaId },
                    commandType: System.Data.CommandType.StoredProcedure,
                    splitOn: "Id"
                );
                return result.Values.ToList();
            }
        }

        public void AddOrderToEmployee(int employeeId, int orderId)
        {
            using (var connection = new SqlConnection(ServerSettings._connectionString))
            {
                connection.Open();

                connection.QuerySingle<OrderDTO>(
                    StoredProcedures.Employee_Order_Add,
                    param: new
                    {
                        EmployeeId = employeeId,
                        OrderId = orderId
                    },
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public void AddWorkAreaToEmployee(int employeeId, int workAreaId)
        {
            using (var connection = new SqlConnection(ServerSettings._connectionString))
            {
                connection.Open();

                connection.QuerySingle<OrderDTO>(
                    StoredProcedures.Employee_WorkArea_Add,
                    param: new
                    {
                        EmployeeId = employeeId,
                        WorkAreaId = workAreaId
                    },
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
        public void AddServiceToEmployee(int employeeId, int serviceId)
        {
            using (var connection = new SqlConnection(ServerSettings._connectionString))
            {
                connection.Open();

                connection.QuerySingle<OrderDTO>(
                    StoredProcedures.Employee_Service_Add,
                    param: new
                    {
                        EmployeeId = employeeId,
                        ServiceId = serviceId
                    },
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
        public List<EmployeeDTO> GetEmployeesInOrderByOrderId(int orderId)
        {
            using (var connection = new SqlConnection(ServerSettings._connectionString))
            {
                connection.Open();

                return connection.Query<EmployeeDTO>(
                    StoredProcedures.GetEmployeesInOrderByOrderId,
                    param: new {OrderId = orderId},
                    commandType: System.Data.CommandType.StoredProcedure)
                    .ToList();
            }
        }

    }


}
