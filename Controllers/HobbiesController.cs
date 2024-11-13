using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HobbiesController : ControllerBase
    {

        private readonly EmplyoeeContext _context;

        public HobbiesController(EmplyoeeContext context)
        {
            _context = context;
        }

        // GET: api/Hobbies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblHobbies>>> GetTblHobbies()
        {
            return await _context.TblHobbies.ToListAsync();
             
        }
    }
}

//i wan hobbie view in form , it sholid show hobbies which are included in my table hobbie, it should be shown in the form of checkbox in form then it should be included accordingly in my table,,check my all codes and give only that code which has error or needs ti be edited..give only that...please give in simple and easy way step by step
