using Microsoft.AspNetCore.Mvc;
using OnlineStore.Data;
using OnlineStore.Models;

namespace OnlineStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IRepository<ProductModel> _repository;

        public ProductsController(IRepository<ProductModel> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetProducts()
        {
            var products = await _repository.GetAll();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductModel>> GetProduct(int id)
        {
            var product = await _repository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductModel>> PostProduct(ProductModel product)
        {
            await _repository.Add(product);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductModel product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            try
            {
                await _repository.Update(product);
            }
            catch
            {
                if (await _repository.GetById(id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _repository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            await _repository.Delete(id);
            return NoContent();
        }
    }
}