using Microsoft.AspNetCore.Mvc;
using minimalAPIMongo.Domains;
using minimalAPIMongo.Services;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace minimalAPIMongo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMongoCollection<User> _user;

        public UserController(MongoDbService mongoDbService)
        {
            _user = mongoDbService.GetDatabase.GetCollection<User>("user");
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> Get()
        {
            try
            {
                var users = await _user.Find(FilterDefinition<User>.Empty).ToListAsync();
                return Ok(users);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(string id)
        {
            try
            {
                var user = await _user.Find(u => u.Id == id).FirstOrDefaultAsync();
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<User>> Create(User user)
        {
            try
            {
                await _user.InsertOneAsync(user);
                return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, User updatedUser)
        {
            try
            {
                var user = await _user.Find(u => u.Id == id).FirstOrDefaultAsync();
                if (user == null)
                {
                    return NotFound();
                }

                updatedUser.Id = user.Id;
                await _user.ReplaceOneAsync(u => u.Id == id, updatedUser);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _user.DeleteOneAsync(u => u.Id == id);
                if (result.DeletedCount == 0)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
