    using Microsoft.AspNetCore.Mvc;
    using WebApplication1.Models;

    namespace WebApplication1.Repository.Interface
    {//Creating methods and adding methods..
     //their implementation is in other class which inherits this Repository, created service class for that to implement this Interface

        public interface IEmployeeRepo
        {

        Task<List<TblAnswer>> GetAnswersBy(int employeeId);

        Task<IActionResult> AnswerPost(IEnumerable<TblAnswer> answers);
            Task<IEnumerable<TblEmployee>> GetEmployees();

            Task<IActionResult> AddEmployee(TblEmployee employee);

            Task<IActionResult> UploadImages([FromForm] int employeeId, [FromForm] List<IFormFile> images);

            Task<IActionResult> UpdateEmployee(int employeeId,TblEmployee employeeData);
            
            Task<IActionResult> DeleteEmployee(int Id);

            Task<ActionResult<TblEmployee>> GetEmployeeById(int Id);

            Task<List<Category>> GetCategory();

            Task<List<SubCategory>> GetSubCategory();

            Task<List<TblQuestion>> GetQuestion();
           
    }
}