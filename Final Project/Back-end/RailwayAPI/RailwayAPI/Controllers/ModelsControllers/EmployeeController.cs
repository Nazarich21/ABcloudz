using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RailwayAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly railwayContext _context;

        public EmployeeController(railwayContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> Get()
        {
            var emplyees = await _context.Employees.ToListAsync();

            Response.Headers.Append("Access-Control-Expose-Headers", "Content-Range");
            Response.Headers.Append("Content-Range", $"employees 1-10/{emplyees.Count()}");

            return emplyees;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> Get(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }
            else
            {
                return employee;
            }
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> Post(Employee value)
        {
            int lastId = Convert.ToInt32(await _context.Employees.OrderByDescending(p => p.Id).Select(p => p.Id).FirstOrDefaultAsync());

            Employee newEmployee = new Employee();
            newEmployee.Id = lastId + 1;
            newEmployee.FirstName = value.FirstName;
            newEmployee.LastName = value.LastName;
            newEmployee.Patronymic = value.Patronymic;
            newEmployee.Birthday = value.Birthday;
            newEmployee.HomeAdress = value.HomeAdress;
            newEmployee.Phones = value.Phones;
            newEmployee.Position = value.Position;
            newEmployee.Photo = value.Photo;

            await _context.Employees.AddAsync(newEmployee);
            await _context.SaveChangesAsync();
            return newEmployee;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Employee>> Put(Employee value, int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }
            else
            {
                employee.FirstName = value.FirstName;
                employee.LastName = value.LastName;
                employee.Patronymic = value.Patronymic;
                employee.Birthday = value.Birthday;
                employee.HomeAdress = value.HomeAdress;
                employee.Phones = value.Phones;
                employee.Position = value.Position;
                employee.Photo = value.Photo;
                await _context.SaveChangesAsync();
                return employee;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }
            else
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }
    }
}
