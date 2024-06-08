
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;
using System;
using System.Linq;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly SubscriptionContext _context;

        public ClientController(SubscriptionContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetClients()
        {
            var clients = _context.Clients.ToList();
            return Ok(clients);
        }

        [HttpGet("{id}")]
        public IActionResult GetClient(int id)
        {
            var client = _context.Clients.FirstOrDefault(c => c.IdClient == id);
            if (client == null)
            {
                return NotFound();
            }
            return Ok(client);
        }

        [HttpPost]
        public IActionResult CreateClient(Client client)
        {
            _context.Clients.Add(client);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetClient), new { id = client.IdClient }, client);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateClient(int id, Client updatedClient)
        {
            var existingClient = _context.Clients.FirstOrDefault(c => c.IdClient == id);
            if (existingClient == null)
            {
                return NotFound();
            }

            existingClient.FirstName = updatedClient.FirstName;
            existingClient.LastName = updatedClient.LastName;
            existingClient.Email = updatedClient.Email;
            existingClient.Phone = updatedClient.Phone;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteClient(int id)
        {
            var clientToDelete = _context.Clients.FirstOrDefault(c => c.IdClient == id);
            if (clientToDelete == null)
            {
                return NotFound();
            }

            _context.Clients.Remove(clientToDelete);
            _context.SaveChanges();

            return NoContent();
        }
    }
}

