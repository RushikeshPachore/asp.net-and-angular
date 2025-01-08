using Azure.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Migrations;
using WebApplication1.Models;
using WebApplication1.Repository.Interface;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApplication1.Repository.Services
{
    public class EmployeeRepository : IEmployeeRepo
    {
        private readonly EmplyoeeContext _emplyoeeContext;

        private readonly IWebHostEnvironment environment;

        public EmployeeRepository(EmplyoeeContext emplyoeeContext, IWebHostEnvironment environment)
        {
            _emplyoeeContext = emplyoeeContext;
            this.environment = environment;
        }




        public async Task<IEnumerable<TblEmployee>> GetEmployees()
        {
            var employees = await _emplyoeeContext.TblEmployee.Include(e => e.Designation).ToListAsync();
           

            foreach (var employee in employees)
            {
                if (!string.IsNullOrEmpty(employee.Hobbies))    //this checks if hobby of current employee is neither
                                                               //null nor empty ,then execute the code
                {
                    // Split the comma-separated hobbyIDs. int.Parse=> "1" will be converted to the integer 1.
                    //Select(int.Parse) part converts each of the split strings into an integer (the ID of the hobby).
                    //This query fetches the actual hobby names from the TblHobbies table in the database,
                    //based on the hobby IDs. 
                    var hobbyIds = employee.Hobbies.Split(',').Select(int.Parse).ToList();                                                                          
                    var hobbies = await _emplyoeeContext.TblHobbies
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
            return (employees);
        }


        public async Task<IActionResult> AddEmployee(TblEmployee employee)
        {
            if (employee == null)
            {
                return new BadRequestObjectResult("Employee data is required.");
            }

            var designation = await _emplyoeeContext.TblDesignation.FindAsync(employee.DesignationID);
            if (designation == null)
            {
                return new BadRequestObjectResult("Invalid DesignationId.");
            }

            // Validate hobby IDs
            var hobbyIds = employee.Hobbies.Split(',').Select(id =>
            {
                int parsedId;
                return int.TryParse(id, out parsedId) ? parsedId : -1;
            }).ToList();

            foreach (var hobbyId in hobbyIds)
            {
                if (hobbyId == -1 || await _emplyoeeContext.TblHobbies.FindAsync(hobbyId) == null)
                {
                    return new BadRequestObjectResult($"Invalid HobbyId: {hobbyId}");
                }
            }

            // Check for duplicate email
            if (!string.IsNullOrEmpty(employee.Email))
            {
                var existingEmail = await _emplyoeeContext.TblEmployee.FirstOrDefaultAsync(e => e.Email == employee.Email && e.Id != employee.Id);

                    if (existingEmail != null)
                {
                    return new BadRequestObjectResult(new { message = "this email already exists" });
                }
            }

            // Hash password before saving
            if (!string.IsNullOrEmpty(employee.password))
            {
                employee.password = BCrypt.Net.BCrypt.HashPassword(employee.password);
            }

            // Add employee to the database
            _emplyoeeContext.TblEmployee.Add(employee);
            await _emplyoeeContext.SaveChangesAsync();

            // Add entries to TblEmployeeHobby
            var employeeHobbies = hobbyIds.Select(hobbyId => new TblEmployeeHobby
            {
                EmpId = employee.Id,
                HobId = hobbyId
            });

            await _emplyoeeContext.TblEmployeeHobbies.AddRangeAsync(employeeHobbies);
            await _emplyoeeContext.SaveChangesAsync();

            return new OkObjectResult (new { id = employee.Id });

        }
        //[FromBody] Used for binding complex data(like JSON or XML) from the body of the request.POST/PUT/PATCH
        //[FromForm] Used for binding simple form data(like strings, files, or form fields) from a multipart/form-data request.POST/PUT
        //The [FromForm] attribute is used in ASP.NET Core to indicate that the data should be bound from the form data of the HTTP request
        //so use this while uploading image and file there. it uploads easily eith this. 

        //IFormFile
        //When a form is submitted with a file (usually using <input type="file"> in an HTML form or a corresponding frontend input component),
        //the file is sent in the request body. ASP.NET Core uses IFormFile to read and manage these files.


        public async Task<IActionResult> UploadImages([FromForm] int employeeId, [FromForm] List<IFormFile> images)
        {
                if (!_emplyoeeContext.TblEmployee.Any(e => e.Id == employeeId))
                {
                    return new NotFoundObjectResult(new { message = "Employee not found." });
                }

                if (images == null || images.Count == 0)
                {
                    return new BadRequestObjectResult(new { message = "No images provided." });
                }

                //list created for uploading images in.
                var uploadedImages = new List<TblImage>();

                foreach (var image in images)
                {
                    if (image.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(environment.WebRootPath, "images");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        var uniqueFileName = $"{Guid.NewGuid()}_{image.FileName}";
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // Save the file
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }

                        // Save image data in the database
                        var tblImage = new TblImage
                        {
                            EmployeeId = employeeId,
                            MultiImage = $"/images/{uniqueFileName}"
                        };
                        uploadedImages.Add(tblImage);
                    }
                }

                await _emplyoeeContext.TblImage.AddRangeAsync(uploadedImages);
                await _emplyoeeContext.SaveChangesAsync();

                return new OkObjectResult(new { message = "Images uploaded successfully.",uploadedImages });           
        }

        //When you need to add a collection of entities (for example, multiple images in your case), AddRangeAsync() is more efficient than calling Add() in a loop for each entity.
        //This is because it minimizes the number of database round-trips.

        
        public async Task<IActionResult> UpdateEmployee(int employeeId,TblEmployee employeeData)
        {
          if(employeeId!= employeeData.Id)
          {
                return new BadRequestObjectResult("Id not found");
          }
            var existingData = await _emplyoeeContext.TblEmployee.Include(e => e.Designation).FirstOrDefaultAsync(e => e.Id == employeeId);

            if (existingData == null)
            {
                return new NotFoundObjectResult("Not found");
            }


            if (!string.IsNullOrEmpty(employeeData.Hobbies))
            {
                var hobbyIds = employeeData.Hobbies.Split(',').Select(id =>
                {
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
                    if (await _emplyoeeContext.TblHobbies.FindAsync(hobbyId) == null)
                    {
                        return new BadRequestObjectResult($"Invalid HobbyId: {hobbyId}");
                    }
                }


                //TBLHOBBYEMP
                var existingHobbies = _emplyoeeContext.TblEmployeeHobbies.Where(eh => eh.EmpId == employeeId);
                _emplyoeeContext.TblEmployeeHobbies.RemoveRange(existingHobbies);

                foreach (var hobId in hobbyIds)
                {
                    _emplyoeeContext.TblEmployeeHobbies.Add(new TblEmployeeHobby
                    {
                        EmpId = employeeId,
                        HobId = hobId
                    });

                }

                // Join valid hobby IDs back into a comma-separated string
                employeeData.Hobbies = string.Join(",", hobbyIds);
            }
            else
            {
                _emplyoeeContext.Entry(employeeData).Property(e => e.Hobbies).IsModified = false;
                //tblEmployee.Hobbies = ""; // If no hobbies, set it as an empty string
            }


            if (!string.IsNullOrEmpty(employeeData.Email))
            {
                var existingEmail=await _emplyoeeContext.TblEmployee.FirstOrDefaultAsync(e=>e.Email == employeeData.Email && e.Id !=employeeData.Id);

                if (existingEmail != null)
                {
                    return new BadRequestObjectResult ("This email already exists , try other" );
                }
            }

            if (employeeData.password != null)
            {
                existingData.password=BCrypt.Net.BCrypt.HashPassword(employeeData.password);
            }

            //update existing data
            existingData.Name = employeeData.Name;
            existingData.LastName = employeeData.LastName;
            existingData.Age = employeeData.Age;
            existingData.DesignationID = employeeData.DesignationID;
            existingData.Email = employeeData.Email;
            existingData.Hobbies = employeeData.Hobbies;
            existingData.Doj = employeeData.Doj;
            existingData.Gender = employeeData.Gender;
            
            await _emplyoeeContext.SaveChangesAsync();          
            return new NoContentResult();

        }

       public async Task<IActionResult> DeleteEmployee(int Id)
       {
            var employee = await _emplyoeeContext.TblEmployee.FindAsync(Id);
            if (employee == null)
            {
            return new NotFoundObjectResult("Employee doesnt exists");
            }
            _emplyoeeContext.TblEmployee.Remove(employee);
            await _emplyoeeContext.SaveChangesAsync();
            return new NoContentResult();
       }


        public async Task<ActionResult<TblEmployee>> GetEmployeeById(int empId)
        {
            var employeeData = await _emplyoeeContext.TblEmployee.Include(e => e.Designation).FirstOrDefaultAsync(e => e.Id==empId);

            if (employeeData == null)
            {
                return new NotFoundObjectResult("Not Found");
            }

            if (!string.IsNullOrEmpty(employeeData.Hobbies))
            {
                var hobbyIds = employeeData.Hobbies.Split(',').Select(int.Parse).ToList();
                var hobbies = await _emplyoeeContext.TblHobbies.Where(h => hobbyIds.Contains(h.HobbyId)).ToListAsync();
                employeeData.Hobbies = string.Join(",", hobbies.Select(h => h.HobbyName));  // Join hobby names as a comma-separated string
            }
            return new OkObjectResult(employeeData);
        }



       public async Task<List<Category>> GetCategory()
        {
            return await _emplyoeeContext.Category.ToListAsync();     
        }

        public async Task<List<SubCategory>> GetSubCategory()
        {
            return await _emplyoeeContext.SubCategory.ToListAsync();
        }
        

        public async Task<List<TblQuestion>> GetQuestion()
        {
            return await _emplyoeeContext.TblQuestion.ToListAsync();
        }

      
        public async Task<List<TblAnswer>> GetAnswersBy(int employeeId)
        {
                // Fetch answers for the given employee
                 return await _emplyoeeContext.TblAnswer
                .Where(a => a.EmployeeId == employeeId)
                .ToListAsync();
        }


        public async Task<IActionResult> AnswerPost(IEnumerable<TblAnswer> answers)
        {
            foreach (var ans in answers) //answer is from frontend
            {
                // Check if an answer already exists for the given EmployeeId and QuestionId
                var existingAnswer = await _emplyoeeContext.TblAnswer
                    .FirstOrDefaultAsync(a => a.EmployeeId == ans.EmployeeId && a.QuestionId == ans.QuestionId);

                if (existingAnswer != null)
                {
                    // If an answer already exists, update it
                    existingAnswer.Answer = ans.Answer;
                    // Optionally, update other properties if necessary
                }
                else
                {
                    // If no answer exists, add a new entry
                    await _emplyoeeContext.TblAnswer.AddAsync(ans);
                }
            }
            // Save changes to the database
                await _emplyoeeContext.SaveChangesAsync();
                return new OkObjectResult( "Answer Saved Successfully" );
        }
    }
}
