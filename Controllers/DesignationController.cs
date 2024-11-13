using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
//issue with my hoobies to post for that particular employe and fetch n my table..i will share my angular fronted then u analyze and give me changes so that it is posted in db  as im using ssms  and fetched in my table
namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesignationController : ControllerBase
    {
        private readonly EmplyoeeContext _context;
        public DesignationController(EmplyoeeContext context)
        {
            _context = context;
        }

        // GET: api/Designation
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblDesignation>>> GetTblDesignation()
        {
            return await _context.TblDesignation.ToListAsync();
        }

        // GET: api/Designation/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<TblDesignation>> GetTblDesignation(int id)
        //{
        //    var tblDesignation = await _context.TblDesignation.FindAsync(id);

        //    if (tblDesignation == null)
        //    {
        //        return NotFound();
        //    }

        //    return tblDesignation;
        //}


        //private bool TblDesignationExists(int id)
        //{
        //    return _context.TblDesignation.Any(e => e.Id == id);
        //}
    }
}
