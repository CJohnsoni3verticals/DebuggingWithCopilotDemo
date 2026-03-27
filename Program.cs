namespace DebuggingWithCopilotDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var orders = GetOrders();
            var highValueOrders = FilterHighValueOrders(orders);

            Console.WriteLine("=== Order Summary ===\n");
            Console.WriteLine($"Orders over $100: {highValueOrders.Count()}");
            foreach (var o in highValueOrders)
            { Console.WriteLine($"  #{o.Id}  {o.CustomerName}"); }
            Console.WriteLine("\n=== Processing ===\n");
            ProcessOrders(orders);
        }

        static void ProcessOrders(IList<Order> orders)
        {
            // Carol White (Id 1003) — use for conditional breakpoint demo.
            foreach (var order in orders)
            {
                ProcessOrder(order);
            }
        }

        static IEnumerable<Order> FilterHighValueOrders(IList<Order> orders)
        {
            // Should be item.Price * item.Quantity
            return orders
                .Where(o => o.Items != null && o.Items.Sum(item => item.Price) > 100);
        }

        static void ProcessOrder(Order order)
        {
            Console.WriteLine($"Order #{order.Id} — {order.CustomerName}");

            if (order.Items == null || !order.Items.Any())
            {
                Console.WriteLine($"Order #{order.Id} has no items.");
                return;
            }

            decimal subtotal = order.Items.Sum(i => i.Price * i.Quantity);

            // threshold should be > 50
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

        static List<Order> GetOrders()
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