
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Repository.Interface;

//Use IActionResult when you need a more general return type or want flexibility with different response types.
//Use ActionResult<T> when you need to return a specific type (like a model or DTO) along with HTTP status codes,
//as it makes your code more expressive and easier to work with.1


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
 
    public class EmployeesController : ControllerBase
    {
        private readonly EmplyoeeContext _context;
        private readonly IConfiguration configuration;
        private readonly  IEmployeeRepo RepoObject;
   
        public EmployeesController(EmplyoeeContext context, IConfiguration configuration, IEmployeeRepo RepoObject)
        {
            this.configuration = configuration;
            _context = context;
            this.RepoObject = RepoObject;
        }


        [HttpGet("answers/{employeeId}")]
        public async Task<IActionResult> GetAnswers(int employeeId)
        {
              var answers = await RepoObject.GetAnswersBy(employeeId);
              if (answers == null || !answers.Any())
              {
              return NotFound(new { message = "No answers found for this employee." });
              }
              return Ok(answers); 
            
        }



        [HttpPost("answers")]
        public async Task<IActionResult> PostAnswers( [FromBody] List<TblAnswer> answers)
        {
            var result = await RepoObject.AnswerPost( answers);

            return Ok (new { message = "Answer Saved Successfully" } );
        }
      


        [HttpGet("questions")]
        public async Task<IActionResult> GetQuestions()
        {
            var question=await RepoObject.GetQuestion();
            if(question==null){
            return NotFound("No question");
            }
            return Ok(question);   
        }
      

        [HttpGet("category")]
        public async Task<IActionResult> GetCategories()
        {
            var category=await RepoObject.GetCategory();
            if (!category.Any())
            {
                return NotFound("No category");
            }
            return Ok( category);
        }


        [HttpGet("subCategory")]
        public async Task<IActionResult> GetSubCategories()
        {
            var subCategory = await RepoObject.GetSubCategory();
            if (!subCategory.Any())
            {
                return NotFound("No category");
            }
            return Ok(subCategory);
        }


        //ActionResult methods that return specific types of data, and you want to make use of ActionResult<T>’s generic capability.
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<TblEmployee>>> GetTblEmployee()
        { 
            return Ok(await RepoObject.GetEmployees());          
        }





        //async means asynchronous means works in background and wont stop further code or application
        [HttpPost]
        //IActionResult is used where u want to return more variety of responses 
        public async Task<IActionResult> PostTblEmployee([FromBody] TblEmployee tblEmployee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return await RepoObject.AddEmployee(tblEmployee);      
        }

        //Select is a LINQ (Language Integrated Query) extension method that operates on collections,
        //such as arrays, lists, or any IEnumerable object.
        //tblEmployee is the object
        // Manually include hobby names in the response
        //tblEmployee.Hobbies: This is a string, likely containing hobby IDs as a comma-separated list(e.g., "1,2,3,4").
        //Split(',') : This method splits the string into an array of substrings, each representing a hobby ID(e.g., ["1", "2", "3", "4"]).
        //Select(int.Parse) : This applies the int.Parse method to each element of the array, converting the strings into integers.The result is a sequence of integers ([1, 2, 3, 4]).
        //ToList(): This converts the sequence into a List<int>.So, the result of this operation will be a list of integers representing the hobby IDs.
        // GET: api/Employees/5

       
        [HttpGet("{id}")]
        public async Task<ActionResult<TblEmployee>> GetTblEmployee(int id)
        {
            return await RepoObject.GetEmployeeById(id);
        }
    
        

        //new
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTblEmployee(int id, TblEmployee tblEmployee)
        {
            return await RepoObject.UpdateEmployee(id, tblEmployee);
        }

        //FirstOrDefaultAsync is an asynchronous LINQ method that:
        // Retrieves the first record that matches the condition(e.Email == loginModel.email).
        //Returns null if no matching record is found.
        //_context is likely an instance of your Entity Framework Core database context, used for querying the database.
        //The JSON body is deserialized into the loginModel parameter because of [FromBody].
        //Serialization: Converting an object to a format like JSON (object → JSON).
        //Deserialization: Converting JSON data back into an object (JSON → object).
        //[FromBody] tells ASP.NET Core to deserialize the JSON body into a C# object, making it easier for you to access the data.
     


      

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTblEmployee(int id)
        {
            return await RepoObject.DeleteEmployee(id);          
        }

        
    }
}