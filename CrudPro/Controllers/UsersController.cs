using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CrudPro.Models;
using CrudPro.Context;

namespace CrudPro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserInfo _context;

        public UsersController(IUserInfo context)
        {
            _context = context;
        }

        [HttpGet("CreateNewDb")]
        public async Task<IActionResult> CreateDatabase()
        {
            var result = await _context.CreateDatabase("demo-project");
            return Ok(result);
        }

        [HttpGet("CreateNewCollection")]
        public async Task<IActionResult> CreateCollection()
        {
            var result = await _context.CreateCollection("demo-project", "demo-collection");
            return Ok(result);
        }

        [HttpPost("CreateNewDocument")]
        public async Task<IActionResult> CreateDocument([FromBody] User user)
        {
            var result = await _context.CreateDocument("demo-project", "demo-collection", user);
            return Ok(result);
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            var result = await _context.GetData("demo-project", "demo-collection");
            return Ok(result);
        }

        [HttpPost("save")]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            var result = await _context.UpsertUser(user);
            return Ok();
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _context.DeleteUser("test-db", "test-collection", id);
            return Ok(result);
        }
    }
}
