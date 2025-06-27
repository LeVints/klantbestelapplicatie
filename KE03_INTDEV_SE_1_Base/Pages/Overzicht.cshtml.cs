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
        private readonly ILogger<OverzichtModel> _logger;
        private readonly MatrixIncDbContext _context;

        public List<Customer> Customers { get; set; } = new();
        public List<Product> Products { get; set; } = new();
        public List<Order> Orders { get; set; } = new();

        public OverzichtModel(ILogger<OverzichtModel> logger, MatrixIncDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task OnGetAsync()
        {
            Customers = await _context.Customers
                .Include(c => c.Orders)
                    .ThenInclude(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                .ToListAsync();

            Orders = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .ToListAsync();

            Products = await _context.Products
                .Include(p => p.Parts)
                .ToListAsync();

            _logger.LogInformation("Aantal klanten geladen: {count}", Customers.Count);
            _logger.LogInformation("Aantal orders geladen: {count}", Orders.Count);
            _logger.LogInformation("Aantal producten geladen: {count}", Products.Count);
        }
    }
}
