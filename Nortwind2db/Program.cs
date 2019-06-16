using DataModels;
using LinqToDB;
using LinqToDB.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nortwind2db
{
    class Program
    {
        static void Main(string[] args)
        {
            LinqToDB.Common.Configuration.Linq.AllowMultipleQuery = true;
            //FirstTask();
            //Console.WriteLine("--------------------");
            //SecondTask();
            //Console.WriteLine("--------------------");
            //ThirdTask();
            Console.WriteLine("--------------------");
            //FourthTask();
            //AddEmployee("Ha", "Sol", new List<string>() { "06897", "19713", "01581", "01730" });
            Console.WriteLine("--------------------");
            //MoveProductsToAnotherCategory();
            Console.WriteLine("--------------------");
            AddListOfProducts(new List<Product>()
            {
                new Product()
                {
                    Category = new Category()
                    {
                        CategoryName  = "Category"
                    },
                    Supplier = new Supplier()
                    {
                        CompanyName = "John Doe"
                    },
                    ProductName = "iProduct"
                }
            });
            Console.ReadKey();
        }

        #region Task 2
        static void FirstTask()
        {
            using (var db = new NorthwindDB())
            {
                var firstTask =
                    from p in db.Products
                    select new
                    {
                        p.ProductName,
                        p.Supplier.CompanyName,
                        p.Category.CategoryName
                    };

                foreach (var t in firstTask)
                {
                    Console.WriteLine(
                        $"Product Name: {t.ProductName} | " +
                        $"Supplier: {t.CompanyName} | " +
                        $"Category: {t.CategoryName}");
                }
            }
        }

        static void SecondTask()
        {
            using (var db = new NorthwindDB())
            {
                var secondTask =
                    from e in db.Employees.LoadWith(x => x.EmployeeTerritories)
                    select new
                    {
                        e,
                        et = db.EmployeeTerritories.LoadWith(x => x.Territory.Region).Where(x => x.EmployeeID == e.EmployeeID)
                    };
                foreach (var t in secondTask)
                {
                    Console.WriteLine(
                        $"ID: {t.e.EmployeeID} |" +
                        $"Employee's Name: {t.e.FirstName} {t.e.LastName} | " +
                        $"Region: {t.e.Region}");
                    foreach (var empTerr in t.et)
                    {
                        Console.WriteLine(
                            $"{empTerr.Territory.TerritoryDescription} - " +
                            $"{empTerr.Territory.Region.RegionDescription}");
                    }
                }
            }
        }

        static void ThirdTask()
        {
            using (var db = new NorthwindDB())
            {
                var thirdTask = from e in db.Employees
                                group e by e.Region into g
                                select new
                                {
                                    g.Key,
                                    Count = g.Count()
                                };


                foreach (var t in thirdTask)
                {
                    Console.WriteLine($"{t.Key} - {t.Count}");
                }
            }
        }

        static void FourthTask()
        {
            using (var db = new NorthwindDB())
            {
                var fourthTask = (from e in db.Orders.LoadWith(x => x.Shipper).LoadWith(x => x.Employee)
                                 select new
                                 {
                                     e.Employee.LastName,
                                     e.Employee.FirstName,
                                     e.Shipper.CompanyName
                                 }).Distinct();


                foreach (var t in fourthTask)
                {
                    Console.WriteLine($"{t.LastName} {t.FirstName}");
                    Console.WriteLine($"Company Name: {t.CompanyName}");
                    Console.WriteLine("---------------------------------");
                }
            }
        }
        #endregion

        #region Task 3
        /// <summary>
        /// Это плохой подход. Думаю, что можно сделать лучше.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="territoriesId"></param>
        static void AddEmployee(string firstName, string lastName, List<string> territoriesId)
        {
            using (var db = new NorthwindDB())
            {
                var lastId = Convert.ToInt32(db.Employees.InsertWithIdentity(() => new Employee()
                {
                    FirstName = firstName,
                    LastName = lastName
                }));             
                foreach (var item in territoriesId)
                {
                    db.EmployeeTerritories.InsertWithIdentity(() => new EmployeeTerritory()
                    {
                        EmployeeID = lastId,
                        TerritoryID = item
                    });
                }
            }
        }

        static void MoveProductsToAnotherCategory(int from = 1, int to = 2)
        {
            using (var db = new NorthwindDB())
            {
                db.Products.Where(p => p.CategoryID == from).Set(p => p.CategoryID, to).Update();
            }
        }

        static void AddListOfProducts(List<Product> products)
        {
            using (var db = new NorthwindDB())
            {
                foreach (var p in products)
                {
                    p.CategoryID = db.Categories.FirstOrDefault(x => x.CategoryName == p.Category.CategoryName)?.CategoryID 
                        ?? Convert.ToInt32(db.Categories.InsertWithIdentity(() => new Category() { CategoryName = p.Category.CategoryName }));                   
                    p.SupplierID = db.Suppliers.FirstOrDefault(x => x.CompanyName == p.Supplier.CompanyName)?.SupplierID 
                        ?? Convert.ToInt32(db.Suppliers.InsertWithIdentity(() => new Supplier() { CompanyName = p.Supplier.CompanyName }));                   
                }
                db.GetTable<Product>().Merge().Using(products).OnTargetKey().UpdateWhenMatched().InsertWhenNotMatched().Merge();               
            }
        }

        static void ReplaceProduct()
        {
            using (var db = new NorthwindDB())
            {
                db.OrderDetails.LoadWith(x => x.Product).LoadWith(x => x.Order)
                    .Where(x => x.Order.ShippedDate == null)
                    .Update(x => new OrderDetail()
                    {
                        ProductID = db.Products.FirstOrDefault(p => p.CategoryID == x.Product.CategoryID && p.ProductID != x.ProductID) != null 
                        ? db.Products.FirstOrDefault(p => p.CategoryID == x.Product.CategoryID && p.ProductID != x.ProductID).ProductID
                        : db.Products.FirstOrDefault(p => p.CategoryID == x.Product.CategoryID).ProductID
                    });
            }
        }
        #endregion
    }
}
