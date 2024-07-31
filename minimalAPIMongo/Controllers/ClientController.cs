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
    public class ClientController : ControllerBase
    {
        private readonly IMongoCollection<Client>? _client;
        private readonly IMongoCollection<User>? _user;

        public ClientController(MongoDbService mongoDbService)
        {
            _client = mongoDbService.GetDatabase.GetCollection<Client>("client");
            _user = mongoDbService.GetDatabase.GetCollection<User>("user");
        }

        [HttpGet]
        public async Task<ActionResult<List<Client>>> Get()
        {
            try
            {
                var clients = await _client.Find(FilterDefinition<Client>.Empty).ToListAsync();
                return Ok(clients);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> Get(string id)
        {
            try
            {
                var client = await _client.Find(c => c.Id == id).FirstOrDefaultAsync();
                if (client == null)
                {
                    return NotFound();
                }
                return Ok(client);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Client>> Create(Client client)
        {
            try
            {
                await _client.InsertOneAsync(client);
                return CreatedAtAction(nameof(Get), new { id = client.Id }, client);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Client updatedClient)
        {
            try
            {
                var client = await _client.Find(c => c.Id == id).FirstOrDefaultAsync();
                if (client == null)
                {
                    return NotFound();
                }

                updatedClient.Id = client.Id;
                await _client.ReplaceOneAsync(c => c.Id == id, updatedClient);
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
                var result = await _client.DeleteOneAsync(c => c.Id == id);
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
