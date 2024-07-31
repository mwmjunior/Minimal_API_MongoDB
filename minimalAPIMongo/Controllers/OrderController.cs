using Microsoft.AspNetCore.Mvc;
using minimalAPIMongo.Domains;
using minimalAPIMongo.Services;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace minimalAPIMongo.Controllers
{
    // Define a rota base para este controlador como "api/order"
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        // Coleções do MongoDB para pedidos, clientes e produtos
        private readonly IMongoCollection<Order> _order;
        private readonly IMongoCollection<Client> _client;
        private readonly IMongoCollection<Product> _product;

        // Construtor que injeta o serviço de banco de dados do MongoDB
        public OrderController(MongoDbService mongoDbService)
        {
            _order = mongoDbService.GetDatabase.GetCollection<Order>("order");
            _client = mongoDbService.GetDatabase.GetCollection<Client>("client");
            _product = mongoDbService.GetDatabase.GetCollection<Product>("product");
        }

        // Endpoint GET para recuperar todos os pedidos, incluindo os dados de cliente e produtos associados
        [HttpGet]
        public async Task<ActionResult<List<object>>> Get()
        {
            try
            {
                // Lista todos os pedidos da coleção "order"
                var orders = await _order.Find(FilterDefinition<Order>.Empty).ToListAsync();
                var result = new List<object>();

                // Percorre todos os itens da lista
                foreach (var order in orders)

                {
                    var client = await _client.Find(c => c.Id == order.ClientId).FirstOrDefaultAsync();
                    var products = await _product.Find(p => order.ProductIds.Contains(p.Id)).ToListAsync();

                    // Adiciona o pedido, cliente e produtos a uma lista de resultados
                    result.Add(new
                    {
                        Order = order,
                        Client = client,
                        Products = products
                    });
                }

                // Retorna a lista de pedidos com seus dados completos
                return Ok(result);
            }
            catch (Exception e)
            {
                // Em caso de erro, retorna uma resposta de erro
                return BadRequest(e.Message);
            }
        }

        // Endpoint GET para recuperar um pedido específico pelo ID, incluindo dados de cliente e produtos
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> Get(string id)
        {
            try
            {
                // Busca o pedido pelo ID
                var order = await _order.Find(o => o.Id == id).FirstOrDefaultAsync();
                if (order == null)
                {
                    return NotFound();
                }

                // Busca o cliente e produtos relacionados ao pedido
                var client = await _client.Find(c => c.Id == order.ClientId).FirstOrDefaultAsync();
                var products = await _product.Find(p => order.ProductIds.Contains(p.Id)).ToListAsync();

                // Cria um objeto com os dados completos do pedido, cliente e produtos
                var result = new
                {
                    Order = order,
                    Client = client,
                    Products = products
                };

                // Retorna o pedido com os dados completos
                return Ok(result);
            }
            catch (Exception e)
            {
                // Em caso de erro, retorna uma resposta de erro
                return BadRequest(e.Message);
            }
        }

        // Endpoint POST para criar um novo pedido
        [HttpPost]
        public async Task<ActionResult<Order>> Create(Order order)
        {
            try
            {
                // Insere o novo pedido na coleção "order"
                await _order.InsertOneAsync(order);

                // Retorna uma resposta de criação com os dados do pedido criado
                return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
            }
            catch (Exception e)
            {
                // Em caso de erro, retorna uma resposta de erro
                return BadRequest(e.Message);
            }
        }

        // Endpoint PUT para atualizar um pedido existente pelo ID
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Order updatedOrder)
        {
            try
            {
                // Busca o pedido existente pelo ID
                var order = await _order.Find(o => o.Id == id).FirstOrDefaultAsync();
                if (order == null)
                {
                    return NotFound();
                }

                // Atualiza o ID do pedido atualizado e substitui o pedido existente
                updatedOrder.Id = order.Id;
                await _order.ReplaceOneAsync(o => o.Id == id, updatedOrder);

                // Retorna uma resposta sem conteúdo indicando que a atualização foi bem-sucedida
                return NoContent();
            }
            catch (Exception e)
            {
                // Em caso de erro, retorna uma resposta de erro
                return BadRequest(e.Message);
            }
        }

        // Endpoint DELETE para excluir um pedido pelo ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                // Exclui o pedido pelo ID e verifica se foi bem-sucedido
                var result = await _order.DeleteOneAsync(o => o.Id == id);
                if (result.DeletedCount == 0)
                {
                    return NotFound();
                }

                // Retorna uma resposta sem conteúdo indicando que a exclusão foi bem-sucedida
                return NoContent();
            }
            catch (Exception e)
            {
                // Em caso de erro, retorna uma resposta de erro
                return BadRequest(e.Message);
            }
        }
    }
}







// using Microsoft.AspNetCore.Mvc;
// using minimalAPIMongo.Domains;
// using minimalAPIMongo.Services;
// using MongoDB.Driver;
// using System.Collections.Generic;
// using System.Threading.Tasks;

// namespace minimalAPIMongo.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class OrderController : ControllerBase
//     {
//         private readonly IMongoCollection<Order> _order;

//         public OrderController(MongoDbService mongoDbService)
//         {
//             _order = mongoDbService.GetDatabase.GetCollection<Order>("order");
//         }

//         [HttpGet]
//         public async Task<ActionResult<List<Order>>> Get()
//         {
//             try
//             {
//                 var orders = await _order.Find(FilterDefinition<Order>.Empty).ToListAsync();
//                 return Ok(orders);
//             }
//             catch (Exception e)
//             {
//                 return BadRequest(e.Message);
//             }
//         }

//         [HttpGet("{id}")]
//         public async Task<ActionResult<Order>> Get(string id)
//         {
//             try
//             {
//                 var order = await _order.Find(o => o.Id == id).FirstOrDefaultAsync();
//                 if (order == null)
//                 {
//                     return NotFound();
//                 }
//                 return Ok(order);
//             }
//             catch (Exception e)
//             {
//                 return BadRequest(e.Message);
//             }
//         }

//         [HttpPost]
//         public async Task<ActionResult<Order>> Create(Order order)
//         {
//             try
//             {
//                 await _order.InsertOneAsync(order);
//                 return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
//             }
//             catch (Exception e)
//             {
//                 return BadRequest(e.Message);
//             }
//         }

//         [HttpPut("{id}")]
//         public async Task<IActionResult> Update(string id, Order updatedOrder)
//         {
//             try
//             {
//                 var order = await _order.Find(o => o.Id == id).FirstOrDefaultAsync();
//                 if (order == null)
//                 {
//                     return NotFound();
//                 }

//                 updatedOrder.Id = order.Id;
//                 await _order.ReplaceOneAsync(o => o.Id == id, updatedOrder);
//                 return NoContent();
//             }
//             catch (Exception e)
//             {
//                 return BadRequest(e.Message);
//             }
//         }

//         [HttpDelete("{id}")]
//         public async Task<IActionResult> Delete(string id)
//         {
//             try
//             {
//                 var result = await _order.DeleteOneAsync(o => o.Id == id);
//                 if (result.DeletedCount == 0)
//                 {
//                     return NotFound();
//                 }
//                 return NoContent();
//             }
//             catch (Exception e)
//             {
//                 return BadRequest(e.Message);
//             }
//         }
//     }
// }
  