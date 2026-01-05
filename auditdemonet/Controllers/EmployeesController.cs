using Audit.WebApi;
using auditdemonet.Models;
using auditdemonet.View;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace auditdemonet.Controllers
{
    [AuditApi(EventTypeName = "EmployeesAPI")]
    [ApiController]
    [Route("api/employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly AuditDemoDbContext _context;

        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(AuditDemoDbContext context, ILogger<EmployeesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var data = _context.Employees
                .Include(e => e.Department)
                .Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    DepartmentName = e.Department.Name
                })
                .ToList();

            return Ok(data);
        }
        [HttpGet("test-log")]
        public IActionResult TestLog()
        {
            _logger.LogInformation("Log4net is working");
            return Ok("Check log file");
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, Employee input)
        {
            var emp = _context.Employees.FirstOrDefault(x => x.Id == id);
            if (emp == null) return NotFound();

            emp.Name = input.Name;
            emp.DepartmentId = input.DepartmentId;

            _context.SaveChanges(); // 🔥 AUTO AUDIT

            _logger.LogInformation("Employee {Id} updated", id);

            return Ok();
        }
        [HttpPost("mockupdate")]
        public IActionResult UpdateMock(int id )
        {
            var emp = _context.Employees.FirstOrDefault(x => x.Id == id);
            if (emp == null) return NotFound();

            emp.Name = "testdata";
            emp.DepartmentId = 2;

            _context.SaveChanges(); // 🔥 AUTO AUDIT

            _logger.LogInformation("Employee {Id} updated", id);

            return Ok();
        }

        [HttpGet("CreateMockBerlapis")]
        public IActionResult CreateMock()
        {
            var emp = _context.Employees.FirstOrDefault(x => x.Id == 2);
            if (emp == null) return NotFound();

            emp.Name = "test ayo";
            emp.DepartmentId = 1;
            emp.Salary = 111111;
            var dep = _context.Departments.FirstOrDefault(x => x.Id == 1);
            dep.Name = "Dep atep";
            var testdep = new Department
            {
                Name = "Dep baru",
                CreatedAt = DateTime.Now
            };
            _context.Departments.Add(testdep);
            _context.SaveChanges();  
              
            return Ok();
        }

        [HttpGet("testgagal")]
        public IActionResult gagal()
        {
            var emp = _context.Employees.FirstOrDefault(x => x.Id == 2);
            if (emp == null) return NotFound();

            emp.Name = "test ayo";
            emp.DepartmentId = 999;
            emp.Salary = 111111;
      
            _context.SaveChanges();

            return Ok();
        }
    }

}
