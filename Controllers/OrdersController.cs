using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApiPractice.Data;
using WebApiPractice.Extensions;
using WebApiPractice.Models;
using WebApiPractice.Services;

namespace WebApiPractice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        public ApplicationDbContext _context { get; set; }

        private readonly ILogger<OrdersController> _logger;

        public OrdersController(ApplicationDbContext context, ILogger<OrdersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        [Route("Order")]

        public IActionResult CreateOrder(string orderNumber, string? reference1 = null)
        {
            var order = new Order
            {
                Reference1 = reference1,
                id = Guid.NewGuid(),
                TotalPrice = 0,
                OrderNumber = orderNumber
            };

            _context.Order.Add(order);
            _context.SaveChanges();

            return Ok(new { Orderguid = order.id });


        }

        [HttpDelete]
        [Route("Order")]

        public async Task<IActionResult> DeleteOrder(Guid guid)
        {
            var order = await _context.Order.FindAsync(guid);
            if (order is null)
            {
                return NotFound($"Order with guid: {guid} does not exist");
            }

            _context.Order.Remove(order);
            _context.SaveChanges();
            return Ok($"Order with guid: {order.id} deleted");


        }

        [HttpPost("{orderGuid}/addProduct/{productGuid}")]
        public IActionResult AddProductToOrder(Guid productGuid, Guid orderGuid, [FromServices] IPriceRecalculationService pricingRecalculationService)
        {
            var maybeOrder = _context.Order.Any(o => o.id == orderGuid);
            if (!maybeOrder)
            {
                return NotFound($"order {orderGuid} does not exist");
            }

            var maybeProduct = _context.Product.Any(p => p.id == productGuid);
            if (!maybeProduct)
            {
                return NotFound($"Product does not exist");
            }

            var maybeOrderProduct = _context.OrderProduct.Any(op => op.OrderId == orderGuid && op.ProductId == productGuid);
            if (maybeOrderProduct) {
                return Ok("line in order with desired product already exists");
            }

            

            _context.OrderProduct.Add(new OrderProduct { ProductId = productGuid, OrderId = orderGuid });
            _context.SaveChanges();
            var order = _context.Order.FirstOrDefault(x => x.id == orderGuid);
            
            order.TotalPrice = pricingRecalculationService.CalculateTotalPrice(orderGuid);
            _context.SaveChanges();
            return Ok($"product {productGuid} sucessfully added to order {orderGuid}");



        }

        [HttpDelete("{orderGuid}/deleteproductfromorder/{productGuid}")]
        public IActionResult DeleteProductFromOrder(Guid productGuid, Guid orderGuid, [FromServices] IPriceRecalculationService priceRecalculationService)
        {
            var maybeOrder = _context.Order.Any(o => o.id == orderGuid);
            if (!maybeOrder)
            {
                return NotFound($"order {orderGuid} does not exist");
            }

            var maybeProduct = _context.Product.Any(p => p.id == productGuid);
            if (!maybeProduct)
            {
                return NotFound($"Product does not exist");
            }

            var maybeOrderProduct = _context.OrderProduct.Any(op => op.OrderId == orderGuid && op.ProductId == productGuid);
            if (!maybeOrderProduct)
            {
                return Ok("Order does not contain product you wanted to delete");
            }
            
            
            
            _context.OrderProduct.Where(a => a.OrderId == orderGuid && a.ProductId == productGuid).ExecuteDelete();
            _context.SaveChanges();

            var order = _context.Order.FirstOrDefault(x => x.id == orderGuid);
            order.TotalPrice = priceRecalculationService.CalculateTotalPrice(orderGuid);

            _context.SaveChanges();
            return Ok($"product {productGuid} sucessfully removed form order {orderGuid}");



        }
        [HttpGet]
        [Route("Order")]
        public IActionResult GetOrder(Guid orderGuid)
        {
            var order = _context.Order
                .Include(o => o.Products)                
                .FirstOrDefault(x => x.id == orderGuid);

            if (order == null)
            {
                return NotFound($"Order with guid {orderGuid} not found");
            }


            return Ok(order.ToDto());



        }

    }
   }

