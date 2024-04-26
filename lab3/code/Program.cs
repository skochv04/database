using System;
using System.Linq;

// // Console.WriteLine("Podaj nazwę produktu: ");
// // String? prodName = Console.ReadLine();

// ProdContext productContext = new ProdContext();

// // Supplier supplier = new Supplier { CompanyName = "Austria Wish", City = "Krakow", Street = "Black" };
// // productContext.Suppliers.Add(supplier);

// // Product product = new Product { ProductName = prodName, UnitsInStock = 24, Supplier = supplier };
// // productContext.Products.Add(product);
// // productContext.SaveChanges();

// var products = productContext.Products.ToList();

// Random random = new Random();

// foreach (var prod in products)
// {
//     int randomUnitsInStock = random.Next(1, 101);
//     prod.UnitsInStock = randomUnitsInStock;
// }

// productContext.SaveChanges();

// foreach (var prod in products)
// {
//     Console.WriteLine(prod);
// }

// var suppliers = productContext.Suppliers.ToList();

// foreach (var sup in suppliers)
// {
//     Console.WriteLine(sup);
// }

// // var query = from prod in productContext.Products
// //             select new { prod.ProductID, prod.ProductName };

// // foreach (var pName in query)
// // {
// //     Console.WriteLine(pName);
// // }

class Program
{
    private static Product createNewProduct()
    {
        Console.Write("Podaj nazwę nowego produktu: ");
        string prodName = Console.ReadLine();
        Console.Write("Podaj ilość jednostek danego produktu dostępnych w sklepie: ");
        int quantity = Int32.Parse(Console.ReadLine());

        Product product = new Product { ProductName = prodName, UnitsInStock = quantity };
        Console.Write($"Został utworzony produkt: {product};");

        return product;
    }

    private static Product findProduct(ProdContext productContext)
    {
        Console.Write("Podaj ID produktu do wyszukiwania: ");
        int prodID = Int32.Parse(Console.ReadLine());

        var query = from prod in productContext.Products
                    where prod.ProductID == prodID
                    select prod;

        return query.FirstOrDefault();
    }

    private static void showAllProducts(ProdContext productContext)
    {
        Console.WriteLine("Lista produktów: ");
        foreach (Product product in productContext.Products)
        {
            Console.WriteLine($"{product.ProductID} | {product}");
        }

    }

    private static Supplier createNewSupplier()
    {
        Console.Write("Podaj nazwę nowego dostawcy: ");
        string companyName = Console.ReadLine();
        Console.Write("Podaj nazwę miasta: ");
        string city = Console.ReadLine();
        Console.Write("Podaj nazwę ulicy: ");
        string street = Console.ReadLine();

        Supplier supplier = new Supplier { CompanyName = companyName, City = city, Street = street };
        Console.Write($"Został utworzony dostawca: {supplier};");

        return supplier;
    }

    private static Supplier findSupplier(ProdContext productContext)
    {
        Console.Write("Podaj ID dostawcy do wyszukiwania: ");
        int supplierID = Int32.Parse(Console.ReadLine());

        var query = from supplier in productContext.Suppliers
                    where supplier.SupplierID == supplierID
                    select supplier;

        return query.FirstOrDefault();
    }

    private static void showAllSuppliers(ProdContext productContext)
    {
        Console.WriteLine("Lista dostawców: ");
        foreach (Supplier supplier in productContext.Suppliers)
        {
            Console.WriteLine($"{supplier.SupplierID} | {supplier}");
        }

    }

    static void Main()
    {
        ProdContext productContext = new ProdContext();

        showAllProducts(productContext);

        showAllSuppliers(productContext);

        Product product = findProduct(productContext);

        Supplier supplier = findSupplier(productContext);

        product.supplier = supplier;

        Console.Write($"\nDla productu: {product.ProductName} zmieniono dostawcę na: {supplier.CompanyName}.\n");

        productContext.SaveChanges();

        showAllProducts(productContext);

        showAllSuppliers(productContext);
    }
}