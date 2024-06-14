using Microsoft.AspNetCore.Mvc;
using OnlineStore.Data;
using OnlineStore.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IRepository<CartModel> _cartRepository;
        private readonly IRepository<ProductModel> _productRepository;

        public CartController(IRepository<CartModel> cartRepository, IRepository<ProductModel> productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<CartModel>> GetCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var carts = await _cartRepository.GetAll();
            var userCart = carts.FirstOrDefault(c => c.UserId == userId);
            if (userCart == null)
            {
                return NotFound();
            }
            return Ok(userCart);
        }

        [HttpPost("{productId}")]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var carts = await _cartRepository.GetAll();
            var userCart = carts.FirstOrDefault(c => c.UserId == userId);

            if (userCart == null)
            {
                userCart = new CartModel { UserId = userId, Items = new List<CartItemModel>() };
                await _cartRepository.Add(userCart);
            }

            var product = await _productRepository.GetById(productId);
            if (product == null)
            {
                return NotFound();
            }

            var cartItem = userCart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (cartItem == null)
            {
                userCart.Items.Add(new CartItemModel { ProductId = productId, Quantity = 1 });
            }
            else
            {
                cartItem.Quantity++;
            }

            await _cartRepository.Update(userCart);
            return Ok(userCart);
        }
    }
}