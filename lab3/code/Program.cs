using System;
using System.Linq;

// Console.WriteLine("Podaj nazwę produktu: ");
// String? prodName = Console.ReadLine();

ProdContext productContext = new ProdContext();

// Supplier supplier = new Supplier { CompanyName = "Austria Wish", City = "Krakow", Street = "Black" };
// productContext.Suppliers.Add(supplier);

// Product product = new Product { ProductName = prodName, UnitsInStock = 24, Supplier = supplier };
// productContext.Products.Add(product);
// productContext.SaveChanges();

var products = productContext.Products.ToList();

Random random = new Random();

foreach (var prod in products)
{
    int randomUnitsInStock = random.Next(1, 101);
    prod.UnitsInStock = randomUnitsInStock;
}

productContext.SaveChanges();

foreach (var prod in products)
{
    Console.WriteLine(prod);
}

var suppliers = productContext.Suppliers.ToList();

foreach (var sup in suppliers)
{
    Console.WriteLine(sup);
}

// var query = from prod in productContext.Products
//             select new { prod.ProductID, prod.ProductName };

// foreach (var pName in query)
// {
//     Console.WriteLine(pName);
// }