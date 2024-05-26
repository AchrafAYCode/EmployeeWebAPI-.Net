using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace EmployeeWebAPI.Models.Repositories
{
	public class EmployeeRepository : IEmployeeRepository
	{
		private readonly AppDbContext appDbContext;
		public EmployeeRepository(AppDbContext appDbContext)
		{
			this.appDbContext = appDbContext;
		}

		async Task<Employee> IEmployeeRepository.GetEmployee(int EmployeeId)
		{
			return await appDbContext.Employees.FirstOrDefaultAsync(e => e.EmployeeId == EmployeeId);
		}

		async Task<Employee> IEmployeeRepository.AddEmployee(Employee employee)
		{
			var result = await appDbContext.Employees.AddAsync(employee);
			await appDbContext.SaveChangesAsync();
			return result.Entity;
		}

		async Task<Employee> IEmployeeRepository.UpdateEmployee(Employee employee)
		{
			var result = await appDbContext.Employees.FirstOrDefaultAsync(e => e.EmployeeId == employee.EmployeeId);
			if (result != null)
			{
				result.FirstName = employee.FirstName;
				result.LastName = employee.LastName;
				result.Email = employee.Email;
				result.DateOfBrith = employee.DateOfBrith;
				result.Gender = employee.Gender;
				result.DepartmentId = employee.DepartmentId;
				result.PhotoPath = employee.PhotoPath;

				await appDbContext.SaveChangesAsync();
				return result;
			}
			return null;
		}

		async Task<Employee> IEmployeeRepository.DeleteEmployee(int employeeId)
		{
			var result = await appDbContext.Employees.FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
			if (result != null)
			{
				appDbContext.Employees.Remove(result);
				await appDbContext.SaveChangesAsync();
				return result;
			}
			return null;
		}

		async Task<Employee> IEmployeeRepository.GetEmployeeByEmail(string email)
		{
			return await appDbContext.Employees.FirstOrDefaultAsync(e => e.Email == email);
		}

		public async Task<IEnumerable<Employee>> GetEmployees()
		{
			return await appDbContext.Employees.ToListAsync();
		}

        public async Task<IEnumerable<Employee>> Search(string name, Gender? gender)
        {
            IQueryable<Employee> query = appDbContext.Employees;
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(e => e.FirstName.Contains(name) || e.LastName.Contains(name));
            }
            if (gender != null)
            {
                query = query.Where(e => e.Gender == gender);
            }
            return await query.ToListAsync();
        }




    }
}
