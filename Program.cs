// OrderProcessor/Program.cs
// DEMO APP — contains 4 deliberate bugs for the Copilot debugging demo.
// Do NOT fix before presenting.
// We'll fix this together during the demo!

using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            var orders = GetSampleOrders();

            Console.WriteLine("=== Order Summary ===\n");

            // BUG B3: Should be item.Price * item.Quantity
            // As written, quantity is ignored, so high-value orders are undercounted.
            var highValueOrders = orders
                .Where(o => o.Items != null && o.Items.Sum(item => item.Price) > 100);

            Console.WriteLine($"Orders over $100: {highValueOrders.Count()}");
            foreach (var o in highValueOrders)
                Console.WriteLine($"  #{o.Id}  {o.CustomerName}");

            Console.WriteLine("\n=== Processing ===\n");

            // BUG B1 will surface here — Bob's Items is null.
            // BUG B2 target: Carol White (Id 1003) — use for conditional breakpoint demo.
            foreach (var order in orders)
            {
                ProcessOrder(order);
            }
        }

        static void ProcessOrder(Order order)
        {
            Console.WriteLine($"Order #{order.Id} — {order.CustomerName}");

            // B1 fires here for Bob (Items is null)
            decimal subtotal = order.Items.Sum(i => i.Price * i.Quantity);

            // BUG B4: threshold should be > 50, not > 500
            // Result: nobody ever gets a discount in practice
            decimal discount = 0m;
            if (subtotal > 500)
            {
                discount = subtotal * 0.10m;
            }

            decimal total = subtotal - discount;

            Console.WriteLine($"  Items:    {order.Items.Count}");
            Console.WriteLine($"  Subtotal: {subtotal:C}");
            Console.WriteLine($"  Discount: {discount:C}");
            Console.WriteLine($"  Total:    {total:C}");
            Console.WriteLine();
        }

        static List<Order> GetSampleOrders()
        {
            return new List<Order>
            {
                new Order
                {
                    Id = 1001,
                    CustomerName = "Alice Johnson",
                    Items = new List<OrderItem>
                    {
                        new OrderItem { Name = "Widget A",  Price = 29.99m, Quantity = 3 },
                        new OrderItem { Name = "Widget B",  Price = 14.99m, Quantity = 1 }
                    }
                },
                new Order
                {
                    Id = 1002,
                    CustomerName = "Bob Smith",
                    Items = null
                },
                new Order
                {
                    Id = 1003,
                    CustomerName = "Carol White",
                    Items = new List<OrderItem>
                    {
                        new OrderItem { Name = "Gadget Pro", Price = 199.99m, Quantity = 1 },
                        new OrderItem { Name = "Cable Pack", Price = 12.50m,  Quantity = 4 }
                    }
                },
                new Order
                {
                    Id = 1004,
                    CustomerName = "Dave Brown",
                    Items = new List<OrderItem>
                    {
                        new OrderItem { Name = "Gizmo X", Price = 75.00m, Quantity = 2 }
                    }
                },
                new Order
                {
                    Id = 1005,
                    CustomerName = "Eve Davis",
                    Items = new List<OrderItem>
                    {
                        new OrderItem { Name = "Doohickey", Price = 8.99m, Quantity = 10 }
                    }
                }
            };
        }
    }

    class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public List<OrderItem> Items { get; set; }
    }

    class OrderItem
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}