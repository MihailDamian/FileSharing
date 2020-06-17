using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileSharing.Core.Entities;
using FileSharing.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FileSharing.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: api/User
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/User/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/User
        [HttpPost]
        public ActionResult<int> Post([FromBody] User user)
        {
            var userRepository = new UserRepository(_configuration.GetConnectionString("FileSharingDatabase"));
            try
            {
                int id = userRepository.Add(user);
                return Ok(id);
            }
            catch(Exception ex)
            {
                return BadRequest();
            }

        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
