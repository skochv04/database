

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

internal class Program
{
    private static Product createNewProduct()
    {
        Console.Write("Podaj nazwę nowego produktu: ");
        var prodName = Console.ReadLine();
        Console.Write("Podaj ilość jednostek danego produktu dostępnych w sklepie: ");
        var quantity = int.Parse(Console.ReadLine());

        var product = new Product { ProductName = prodName, UnitsInStock = quantity };
        Console.Write($"Został utworzony produkt: {product}");

        return product;
    }

    private static Product findProduct(ProdContext productContext)
    {
        Console.Write("Podaj ID produktu do wyszukiwania: ");
        var prodID = int.Parse(Console.ReadLine());

        var query = from prod in productContext.Products
            where prod.ProductID == prodID
            select prod;

        return query.FirstOrDefault();
    }

    private static void showAllProducts(ProdContext productContext)
    {
        Console.WriteLine("Lista produktów: ");
        foreach (var product in productContext.Products) Console.WriteLine($"{product.ProductID} | {product}");
    }

    private static Supplier createNewSupplier()
    {
        Console.Write("Podaj nazwę nowego dostawcy: ");
        var companyName = Console.ReadLine();
        Console.Write("Podaj nazwę miasta: ");
        var city = Console.ReadLine();
        Console.Write("Podaj nazwę ulicy: ");
        var street = Console.ReadLine();

        var supplier = new Supplier { CompanyName = companyName, City = city, Street = street };
        Console.Write($"Został utworzony dostawca: {supplier}.\n");

        return supplier;
    }

    private static Supplier findSupplier(ProdContext productContext)
    {
        Console.Write("Podaj ID dostawcy do wyszukiwania: ");
        var supplierID = int.Parse(Console.ReadLine());

        var query = from supplier in productContext.Suppliers
            where supplier.SupplierID == supplierID
            select supplier;

        return query.FirstOrDefault();
    }

    private static void showAllSuppliers(ProdContext productContext)
    {
        Console.WriteLine("Lista dostawców: ");
        foreach (var supplier in productContext.Suppliers) Console.WriteLine($"{supplier.SupplierID} | {supplier}");
    }

    private static void Main()
    {
        var productContext = new ProdContext();

        Supplier supplier = null;
        Product product = null;

        Console.WriteLine("Dodać nowego dostawcę? (Tak/Nie)");
        var choice = Console.ReadLine();
        var correctAnswer = false;
        do
        {
            switch (choice)
            {
                case "Tak":
                    supplier = createNewSupplier();
                    productContext.Suppliers.Add(supplier);
                    correctAnswer = true;
                    break;
                case "Nie":
                    showAllSuppliers(productContext);
                    supplier = findSupplier(productContext);
                    correctAnswer = true;
                    break;
            }
        } while (!correctAnswer);

        do
        {
            Console.WriteLine("Dodać nowy produkt? (Tak/Nie)");
            choice = Console.ReadLine();
            correctAnswer = false;
            switch (choice)
            {
                case "Tak":
                    product = createNewProduct();
                    productContext.Products.Add(product);
                    correctAnswer = true;
                    break;
                case "Nie":
                    correctAnswer = true;
                    break;
            }

            Console.WriteLine("");
        } while (!correctAnswer || choice == "Tak");

        productContext.SaveChanges();


        product = findProduct(productContext);

        Console.WriteLine("");

        Console.WriteLine("Zmienić dostawcę dla produktu na podanego wyżej? (Tak/Nie)");
        choice = Console.ReadLine();
        correctAnswer = false;
        do
        {
            switch (choice)
            {
                case "Tak":
                    supplier.Products.Add(product);
                    product.supplier = supplier;
                    Console.Write(
                        $"\nDla productu: {product.ProductName} zmieniono dostawcę na: {supplier.CompanyName}.\n");
                    productContext.SaveChanges();
                    correctAnswer = true;
                    break;
                case "Nie":
                    correctAnswer = true;
                    break;
            }
        } while (!correctAnswer);

        showAllProducts(productContext);

        showAllSuppliers(productContext);
    }
}