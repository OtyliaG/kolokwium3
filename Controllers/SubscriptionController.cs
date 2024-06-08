
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;
using System.Linq;
using WebApplication1.Data;
using WebApplication1.Models;
namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaleController : ControllerBase
    {
        private readonly SubscriptionContext _context;

        public SaleController(SubscriptionContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetSales()
        {
            var sales = _context.Sales.ToList();
            return Ok(sales);
        }

        [HttpGet("{id}")]
        public IActionResult GetSale(int id)
        {
            var sale = _context.Sales.FirstOrDefault(s => s.IdSale == id);
            if (sale == null)
            {
                return NotFound();
            }
            return Ok(sale);
        }

        [HttpPost]
        public IActionResult CreateSale(Sale sale)
        {
            object value = _context.Sales.Add(sale);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetSale), new { id = sale.IdSale }, sale);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSale(int id)
        {
            var saleToDelete = _context.Sales.FirstOrDefault(s => s.IdSale == id);
            if (saleToDelete == null)
            {
                return NotFound();
            }

            _context.Sales.Remove(saleToDelete);
            _context.SaveChanges();

            return NoContent();
        }
    }
}

