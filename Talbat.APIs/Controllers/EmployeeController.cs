using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications.EmployeeSpec;

namespace Talabat.APIs.Controllers
{

	public class EmployeeController : BaseApiController
	{
		private readonly IGenericRepository<Employee> _employeesReop;

		public EmployeeController(IGenericRepository<Employee> employeesReop)
        {
			_employeesReop = employeesReop;
		}

		[HttpGet] // : BaseUrl/api/Employee
		public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
		{
			var spec = new EmployeeWithDepartmentSpecifications();

			var employees = await _employeesReop.GetAllWithSpecsAsync(spec);

			return Ok(employees);
		}


		[HttpGet("{id}")] //Get: BaseUrl/api/Employee/1
		public async Task<ActionResult<Employee>> GetEmployee(int id)
		{
			var spec = new EmployeeWithDepartmentSpecifications(id);
			var employee = await _employeesReop.GetWithSpec(spec);

			if (employee is null)
				return NotFound();

			return Ok(employee);
		}
    }
}
