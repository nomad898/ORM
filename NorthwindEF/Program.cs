using NorthwindEF.ado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindEF
{
    class Program
    {
        static void Main(string[] args)
        {
            FirstTask();
            Console.ReadKey();
        }

        static void FirstTask()
        {
            using (var db = new Northwind())
            {
                var result = db.Orders
                    .Where(x => x.Order_Details
                    .Where(d => d.Product.CategoryID == 3).Count() > 0)
                    .Select(x => new
                    {
                        x.Customer.ContactName,
                        d = x.Order_Details
                    });

                foreach (var item in result)
                {
                    Console.WriteLine($"Name: {item.ContactName}");
                    foreach (var order in item.d)
                    {
                        Console.WriteLine($"Product name: {order.Product.ProductName} |" +
                            $" {order.Product.Supplier.CompanyName}");
                    }
                }
            }
        }
    }
}
