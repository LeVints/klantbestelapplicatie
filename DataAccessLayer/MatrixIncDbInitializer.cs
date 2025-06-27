using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer
{
    public static class MatrixIncDbInitializer
    {
        public static void Initialize(MatrixIncDbContext context)
        {
            // Check of de database al data bevat
            if (context.Customers.Any())
            {
                return;   // DB is al gevuld
            }

            // Klanten
            var customers = new Customer[]
            {
                new Customer { Name = "Neo", Address = "123 Elm St" , Active=true},
                new Customer { Name = "Morpheus", Address = "456 Oak St", Active = true },
                new Customer { Name = "Trinity", Address = "789 Pine St", Active = true }
            };
            context.Customers.AddRange(customers);

            // Orders
            var orders = new Order[]
            {
                new Order { Customer = customers[0], OrderDate = DateTime.Parse("2021-01-01") },
                new Order { Customer = customers[0], OrderDate = DateTime.Parse("2021-02-01") },
                new Order { Customer = customers[1], OrderDate = DateTime.Parse("2021-02-01") },
                new Order { Customer = customers[2], OrderDate = DateTime.Parse("2021-03-01") }
            };
            context.Orders.AddRange(orders);

            // Producten
            var products = new Product[]
            {
                new Product { Name = "Nebuchadnezzar", Description = "Het schip waarop Neo voor het eerst de echte wereld leert kennen", Price = 10000.00m },
                new Product { Name = "Jack-in Chair", Description = "Stoel met een rugsteun en metalen armen waarin mensen zitten om ingeplugd te worden in de Matrix via een kabel in de nekpoort", Price = 500.50m },
                new Product { Name = "EMP (Electro-Magnetic Pulse) Device", Description = "Wapentuig op de schepen van Zion", Price = 129.99m }
            };
            context.Products.AddRange(products);

            // Onderdelen
            var parts = new Part[]
            {
                new Part { Name = "Tandwiel", Description = "Overdracht van rotatie in bijvoorbeeld de motor of luikmechanismen"},
                new Part { Name = "M5 Boutje", Description = "Bevestiging van panelen, buizen of interne modules"},
                new Part { Name = "Hydraulische cilinder", Description = "Openen/sluiten van zware luchtsluizen of bewegende onderdelen"},
                new Part { Name = "Koelvloeistofpomp", Description = "Koeling van de motor of elektronische systemen."}
            };
            context.Parts.AddRange(parts);

            context.SaveChanges(); // Sla eerst basisgegevens op zodat IDs beschikbaar zijn

            // Koppel producten aan orders via OrderItems
            var orderItems = new OrderItem[]
            {
                new OrderItem { Order = orders[0], Product = products[0], Quantity = 1, UnitPrice = products[0].Price },
                new OrderItem { Order = orders[0], Product = products[1], Quantity = 2, UnitPrice = products[1].Price },
                new OrderItem { Order = orders[1], Product = products[2], Quantity = 1, UnitPrice = products[2].Price },
                new OrderItem { Order = orders[2], Product = products[1], Quantity = 1, UnitPrice = products[1].Price },
                new OrderItem { Order = orders[3], Product = products[0], Quantity = 1, UnitPrice = products[0].Price },
                new OrderItem { Order = orders[3], Product = products[2], Quantity = 3, UnitPrice = products[2].Price }
            };
            context.OrderItems.AddRange(orderItems);

            // Koppel onderdelen aan producten
            products[0].Parts.Add(parts[0]);
            products[0].Parts.Add(parts[1]);

            products[1].Parts.Add(parts[2]);

            products[2].Parts.Add(parts[3]);

            context.SaveChanges(); // Sla alle relaties op
        }
    }
}
