using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class OverzichtModel : PageModel
    {
        // Logger voor het registreren van informatie en foutopsporing
        private readonly ILogger<OverzichtModel> _logger;

        // Databasecontext voor het ophalen van gegevens uit de database
        private readonly MatrixIncDbContext _context;

        // Lijsten voor klanten, producten en bestellingen die aan de view worden doorgegeven
        public List<Customer> Customers { get; set; } = new();
        public List<Product> Products { get; set; } = new();
        public List<Order> Orders { get; set; } = new();

        // Constructor met dependency injection van logger en databasecontext
        public OverzichtModel(ILogger<OverzichtModel> logger, MatrixIncDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Methode die uitgevoerd wordt bij een GET-verzoek naar de Overzicht-pagina
        public async Task OnGetAsync()
        {
            // Haalt klanten op inclusief hun bestellingen en de bijbehorende producten
            Customers = await _context.Customers
                .Include(c => c.Orders)
                    .ThenInclude(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                .ToListAsync();

            // Haalt bestellingen op inclusief de producten binnen elke bestelling
            Orders = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .ToListAsync();

            // Haalt producten op inclusief de onderdelen
            Products = await _context.Products
                .Include(p => p.Parts)
                .ToListAsync();

            // Logging van het aantal opgehaalde records
            _logger.LogInformation("Aantal klanten geladen: {count}", Customers.Count);
            _logger.LogInformation("Aantal orders geladen: {count}", Orders.Count);
            _logger.LogInformation("Aantal producten geladen: {count}", Products.Count);
        }
    }
}
