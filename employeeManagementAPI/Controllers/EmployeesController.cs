using EmployeeManagementAppService;
using EmployeeManagementModels;
using Microsoft.AspNetCore.Mvc;

namespace employeeManagementAPI.Controllers
{
    [ApiController]
    [Route("api/employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeAppService _appService;

        public EmployeesController()
        {
            _appService = new EmployeeAppService();
        }

        // GET: api/employees
        [HttpGet]
        public ActionResult<IEnumerable<Employee>> GetEmployees()
        {
            var employees = _appService.GetEmployees();
            return Ok(employees);
        }

        // GET: api/employees/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<Employee> GetEmployee(Guid id)
        {
            var employee = _appService.GetEmployee(id);

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        // POST: api/employees
        [HttpPost]
        public IActionResult HireEmployee([FromBody] Employee employee)
        {
            if (employee == null)
                return BadRequest("Employee data is required.");

            var result = _appService.HireEmployee(employee);

            if (!result)
                return Conflict("Employee already exists.");

            return Ok("Employee hired successfully.");
        }

        // PATCH: api/employees/{id}
        [HttpPatch("{id:guid}")]
        public IActionResult UpdateEmployee(Guid id, [FromBody] Employee employee)
        {
            if (employee == null)
                return BadRequest("Employee data is required.");

            var existing = _appService.GetEmployee(id);

            if (existing == null)
                return NotFound();

            // You can choose what PATCH means here:
            // OPTION 1: Promote employee (position + salary change)
            _appService.PromoteEmployee(id, employee.Position, employee.Salary - existing.Salary);

            return NoContent();
        }

        // DELETE: api/employees/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult RemoveEmployee(Guid id)
        {
            var existing = _appService.GetEmployee(id);

            if (existing == null)
                return NotFound();

            _appService.RemoveEmployee(id);

            return NoContent();
        }
    }
}