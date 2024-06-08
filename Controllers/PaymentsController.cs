
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly SubscriptionContext _context;

        public PaymentsController(SubscriptionContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddPayment([FromBody] PaymentRequest request)
        {
            // 1. Sprawdzamy czy klient istnieje
            var client = await _context.Clients.FindAsync(request.IdClient);
            if (client == null)
            {
                return NotFound("Client not found");
            }

            // 2. Sprawdzamy czy subskrypcja istnieje
            var subscription = await _context.Subscriptions.FindAsync(request.IdSubscription);
            if (subscription == null)
            {
                return NotFound("Subscription not found");
            }

            // 3. Sprawdzamy czy subskrypcja jest aktywna
            if (subscription.EndTime < DateTime.Now)
            {
                return BadRequest("Subscription is not active");
            }

            // 4. Sprawdzamy czy przypadkiem klient nie opłacił już subskrypcji za ten okres
            var sale = await _context.Sales
                .FirstOrDefaultAsync(s => s.IdClient == request.IdClient && s.IdSubscription == request.IdSubscription);

            if (sale == null)
            {
                return BadRequest("Sale not found for this subscription and client");
            }

            var nextPeriodStart = sale.CreatedAt.AddMonths(subscription.RenewalPeriod);
            var existingPayment = await _context.Payments
                .FirstOrDefaultAsync(p => p.IdClient == request.IdClient && p.IdSubscription == request.IdSubscription && p.Date >= nextPeriodStart);

            if (existingPayment != null)
            {
                return BadRequest("Subscription for this period is already paid");
            }

            // 5. Sprawdzamy czy wpłacana kwota jest zgodna z kwotą subskrypcji
            var amount = subscription.Price;

            // 6. Jeśli istnieje aktywna zniżka, przeliczamy wartość zapłaty
            var discount = await _context.Discounts
                .Where(d => d.IdSubscription == request.IdSubscription && d.DateFrom <= DateTime.Now && d.DateTo >= DateTime.Now)
                .OrderByDescending(d => d.Value)
                .FirstOrDefaultAsync();

            if (discount != null)
            {
                amount -= amount * discount.Value / 100;
            }

            if (request.Payment != amount)
            {
                return BadRequest("Payment amount does not match the subscription price");
            }

            // 7. Wstawiamy rekord do tabeli Payment
            var payment = new Payment
            {
                Date = DateTime.Now,
                IdClient = request.IdClient,
                IdSubscription = request.IdSubscription
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            // 8. Zwracamy wygenerowaną wartość Id
            return Ok(payment.IdPayment);
        }
    }
}
