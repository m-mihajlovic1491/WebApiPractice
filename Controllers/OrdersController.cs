using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;
using WebApiPractice.Data;
using WebApiPractice.Extensions;
using WebApiPractice.Interfaces;
using WebApiPractice.Models;
using WebApiPractice.Requests.Command;
using WebApiPractice.Services;

namespace WebApiPractice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        public ApplicationDbContext _context { get; set; }
        public IOrderRepository _orderRepository { get; }

        private readonly ILogger<OrdersController> _logger;

        public OrdersController(ApplicationDbContext context, ILogger<OrdersController> logger,IOrderRepository orderRepository)
        {
            _context = context;
            _logger = logger;
            _orderRepository = orderRepository;
        }

        [HttpPost]
        [Route("Order")]

        public IActionResult CreateOrder([FromBody] CreateOrderRequest newOrder, [FromServices] IValidator<CreateOrderRequest> orderValidator)
        {
            var validationResult =orderValidator.Validate(newOrder);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var order = new Order
            {
                id = Guid.NewGuid(),
                Reference1 = newOrder.reference1,
                OrderNumber = newOrder.orderNumber

            };

            

            _context.Order.Add(order);
            _context.SaveChanges();

            return Ok(new { Orderguid = order.id });


        }

        [HttpDelete]
        [Route("Order/{guid}")]

        public async Task<IActionResult> DeleteOrder([FromRoute]Guid guid)
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
        public async Task<IActionResult> AddProductToOrder(Guid productGuid, Guid orderGuid, [FromServices] IPriceRecalculationService pricingRecalculationService)
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
                return BadRequest("line in order with desired product already exists");
            }

            using var transaction  = await _context.Database.BeginTransactionAsync();

            try
            {
                _context.OrderProduct.Add(new OrderProduct { ProductId = productGuid, OrderId = orderGuid });
                _context.SaveChanges();
                var order = _context.Order
                    .Include(o=>o.Products)
                    .FirstOrDefault(x => x.id == orderGuid);

                
                order.TotalPrice = pricingRecalculationService.CalculateTotalPrice(orderGuid);
                _context.SaveChanges();
                await transaction.CommitAsync();
                return Ok(order.ToDto());
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        [HttpDelete("{orderGuid}/deleteproductfromorder/{productGuid}")]
        public IActionResult DeleteProductFromOrder([FromRoute]Guid productGuid, [FromRoute]Guid orderGuid, [FromServices] IPriceRecalculationService priceRecalculationService)
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

        [HttpGet]
        [Route("Orders")]
        [ProducesResponseType(200,Type = typeof (IEnumerable<Order>))]
        public IActionResult GetAllOrders([FromQuery] int page,
                                          [FromQuery] int pageSize,
                                          [FromQuery] string search)
        {
            var orders = _orderRepository.GetOrdersWithLines();

            if (!ModelState.IsValid) return BadRequest("Invalid model");

            return Ok(orders);

        }

        [HttpGet]
        [Route("GetAllPaged")]        
        public IActionResult GetAllOrdersPaged([FromQuery] int page,
                                          [FromQuery] int pageSize)
                                          
        {
            var orders = _orderRepository.GetAllOrdersPaged(page,pageSize);

            if (!ModelState.IsValid) return BadRequest("Invalid model");

            return Ok(orders);

        }

    }
   }

