using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly EmplyoeeContext _context;
        
        public EmployeesController(EmplyoeeContext context)
        {
            _context = context; 
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblEmployee>>> GetTblEmployee()
        {
            var employees = await _context.TblEmployee
                .Include(e => e.Designation) // Include Designation details
                .ToListAsync();

            foreach (var employee in employees)
            {
                if (!string.IsNullOrEmpty(employee.Hobbies))    //this checks if hobby of current employee is neither
                                                                //null nor empty ,then execute the code
                {
                    // Split the comma-separated hobby IDs
                    var hobbyIds = employee.Hobbies.Split(',').Select(int.Parse).ToList();  //Select(int.Parse) part
                                                                                            //converts each of the split
                                                                                            //strings into an integer (the ID of the hobby).


                    //This query fetches the actual hobby names from the TblHobbies table in the database,
                    //based on the hobby IDs.

                    var hobbies = await _context.TblHobbies
                        .Where(h => hobbyIds.Contains(h.HobbyId))
                        .ToListAsync();

                    // Join the hobby names as a comma-separated string
                    employee.Hobbies = string.Join(",", hobbies.Select(h => h.HobbyName));
                }
                else
                {
                    employee.Hobbies = "No hobbies Available";  // If no hobbies, display this message
                }
            }

            return Ok(employees);
        }


        // GET: api/Employees/5
        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TblEmployee>> GetTblEmployee(int id)
        {
            var tblEmployee = await _context.TblEmployee
                .Include(e => e.Designation) // Include Designation details
                .FirstOrDefaultAsync(e => e.Id == id);

            if (tblEmployee == null)
            {
                return NotFound();
            }

            // Manually include hobby names in the response
            if (!string.IsNullOrEmpty(tblEmployee.Hobbies))
            {
                var hobbyIds = tblEmployee.Hobbies.Split(',').Select(int.Parse).ToList();
                var hobbies = await _context.TblHobbies
                    .Where(h => hobbyIds.Contains(h.HobbyId))
                    .ToListAsync();
                tblEmployee.Hobbies = string.Join(",",hobbies.Select(h => h.HobbyName));  // Join hobby names as a comma-separated string
            }

            return Ok(tblEmployee);
        }


        //new
        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblEmployee(int id, TblEmployee tblEmployee)
        {
            if (id != tblEmployee.Id)
            {
                return BadRequest();
            }

            var existingEmployee = await _context.TblEmployee
                .Include(e => e.Designation)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (existingEmployee == null)
            {
                return NotFound();
            }

            // Validate hobbies
            if (!string.IsNullOrEmpty(tblEmployee.Hobbies))
            {
                var hobbyIds = tblEmployee.Hobbies.Split(',').Select(id => {
                    int parsedId;
                    if (int.TryParse(id, out parsedId) && parsedId > 0)
                    {
                        return parsedId;
                    }
                    return -1; // Invalid ID marker
                }).Where(id => id != -1).ToList(); // Filter out invalid hobby IDs

                // Ensure all hobby IDs are valid
                foreach (var hobbyId in hobbyIds)
                {
                    if (await _context.TblHobbies.FindAsync(hobbyId) == null)
                    {
                        return BadRequest($"Invalid HobbyId: {hobbyId}");
                    }
                }

                // Join valid hobby IDs back into a comma-separated string
                tblEmployee.Hobbies = string.Join(",", hobbyIds);
            }
            else
            {
                tblEmployee.Hobbies = ""; // If no hobbies, set it as an empty string
            }

            //update password
            if (!string.IsNullOrEmpty(tblEmployee.password))
            {
                existingEmployee.password = BCrypt.Net.BCrypt.HashPassword(tblEmployee.password);
            }

            // Update employee details
            existingEmployee.Name = tblEmployee.Name;
            existingEmployee.LastName = tblEmployee.LastName;
            existingEmployee.DesignationID = tblEmployee.DesignationID;
            existingEmployee.Hobbies = tblEmployee.Hobbies;  // Update the Hobbies string
            existingEmployee.Email = tblEmployee.Email;
            existingEmployee.Age = tblEmployee.Age;
            existingEmployee.Doj = tblEmployee.Doj;
            existingEmployee.Gender = tblEmployee.Gender;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblEmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }




        // PUT: api/Employees/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutTblEmployee(int id, TblEmployee tblEmployee)
        //{
        //    if (id != tblEmployee.Id)
        //    {
        //        return BadRequest();
        //    }

        //    var existingEmployee = await _context.TblEmployee
        //        .Include(e => e.Designation)
        //        .FirstOrDefaultAsync(e => e.Id == id);

        //    if (existingEmployee == null)
        //    {
        //        return NotFound();
        //    }

        //    // Validate hobbies
        //    var hobbyIds = tblEmployee.Hobbies.Split(',').Select(id => {
        //        int parsedId;
        //        if (int.TryParse(id, out parsedId))
        //        {
        //            return parsedId;
        //        }
        //        return -1; // Invalid ID marker
        //    }).ToList();

        //    // Ensure all hobby IDs are valid
        //    foreach (var hobbyId in hobbyIds)
        //    {
        //        if (hobbyId == -1 || await _context.TblHobbies.FindAsync(hobbyId) == null)
        //        {
        //            return BadRequest($"Invalid HobbyId: {hobbyId}");
        //        }
        //    }

        //    // Update employee details
        //    existingEmployee.Name = tblEmployee.Name;
        //    existingEmployee.DesignationID = tblEmployee.DesignationID;
        //    existingEmployee.Hobbies = tblEmployee.Hobbies;  // Update the Hobbies string
        //    existingEmployee.Email = tblEmployee.Email;
        //    existingEmployee.Age = tblEmployee.Age;
        //    existingEmployee.Doj = tblEmployee.Doj;
        //    existingEmployee.Gender = tblEmployee.Gender;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!TblEmployeeExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}



        [HttpPost]
        public async Task<ActionResult<TblEmployee>> PostTblEmployee(TblEmployee tblEmployee)
        {
            if (tblEmployee == null)
            {
                return BadRequest("Employee data is required.");
            }

            if (tblEmployee.DesignationID == 0 || string.IsNullOrEmpty(tblEmployee.Hobbies))
            {
                return BadRequest("Designation and Hobbies are required.");
            }

            var designation = await _context.TblDesignation.FindAsync(tblEmployee.DesignationID);
            if (designation == null)
            {
                return BadRequest("Invalid DesignationId.");
            }

            // Validate each hobby ID
            var hobbyIds = tblEmployee.Hobbies.Split(',').Select(id => {
                int parsedId;
                if (int.TryParse(id, out parsedId))
                {
                    return parsedId;
                }
                return -1; // Invalid ID marker
            }).ToList();

            // Ensure all hobby IDs are valid
            foreach (var hobbyId in hobbyIds)
            {
                if (hobbyId == -1 || await _context.TblHobbies.FindAsync(hobbyId) == null)
                {
                    return BadRequest($"Invalid HobbyId: {hobbyId}");
                }
            }

            //hashPassword Before saving
            if (!string.IsNullOrEmpty(tblEmployee.password))
            {
                tblEmployee.password=BCrypt.Net.BCrypt.HashPassword(tblEmployee.password);
            }

            // Add the employee to the database
            _context.TblEmployee.Add(tblEmployee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTblEmployee", new { id = tblEmployee.Id }, tblEmployee);
        }


        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginModel login)
        {




        }


        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTblEmployee(int id)
        {
            var tblEmployee = await _context.TblEmployee.FindAsync(id);
            if (tblEmployee == null)
            {
                return NotFound();
            }

            _context.TblEmployee.Remove(tblEmployee);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool TblEmployeeExists(int id)
        {
            return _context.TblEmployee.Any(e => e.Id == id);
        }
    }
}
