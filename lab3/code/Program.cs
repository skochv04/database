using System;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

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
    private static void createNewProduct(ProdContext productContext)
    {
        Console.Write("Podaj nazwę nowego produktu: ");
        string prodName = Console.ReadLine();
        Console.Write("Podaj ilość jednostek danego produktu dostępnych w sklepie: ");
        int quantity = Int32.Parse(Console.ReadLine());

        Product product = new Product { ProductName = prodName, UnitsInStock = quantity };
        Console.Write($"Został utworzony produkt: {product.ProductName}");


        productContext.Products.Add(product);
        productContext.SaveChanges();
    }

    private static void addProductToBasket(ProdContext productContext, List<InvoiceItem> basketItems)
    {
        int prodID, prodQuantity;
        bool correctQuantity = false;
        showAvailableProducts(productContext);
        do
        {
            Console.Write("\nPodaj ID produktu, który należy dodać do koszyka: ");
            prodID = Int32.Parse(Console.ReadLine());
            Console.WriteLine();
        } while (!productAvailable(productContext, prodID));


        do
        {
            Console.WriteLine();
            Console.Write("Podaj ilość sztuk produktu: ");
            prodQuantity = Int32.Parse(Console.ReadLine());

            var query = from product in productContext.Products
                        where product.ProductID == prodID && product.UnitsInStock >= prodQuantity
                        select product;
            if ((query?.Count() > 0))
            {
                correctQuantity = true;
            }
            else
            {
                Console.WriteLine("W sklepie nie ma takiej ilości produktów");
            }
        } while (!correctQuantity);
        InvoiceItem item = new InvoiceItem { ProductID = prodID, Quantity = prodQuantity };
        basketItems.Add(item);
        Console.Write($"Do koszyka został dodany produkt o numerze {prodID}");
        productContext.SaveChanges();
    }

    private static void removeProductFromBasket(ProdContext productContext, List<InvoiceItem> basketItems)
    {
        int prodID;
        showProductsInBasket(productContext, basketItems);
        do
        {
            Console.Write("\nPodaj ID produktu, który należy usunąć z koszyka: ");
            prodID = Int32.Parse(Console.ReadLine());
            Console.WriteLine();
        } while (!productExists(productContext, prodID));

        InvoiceItem item_to_remove = null;
        foreach (InvoiceItem item in basketItems)
        {
            if (item.ProductID == prodID) item_to_remove = item;
        }
        if (item_to_remove != null)
        {
            basketItems.Remove(item_to_remove);
            Console.Write($"Z koszyka został usunięty produkt o numerze {prodID}");
        }
        else { Console.Write($"W koszyka nie znaleziono produktu o numerze {prodID}"); }

    }

    private static void buyProductsInBasket(ProdContext prodContext, List<InvoiceItem> basketItems)
    {
        Invoice invoice = CreateInvoice(basketItems);
        prodContext.Invoices.Add(invoice);
        foreach (InvoiceItem item in basketItems)
        {
            var product = prodContext.Products.FirstOrDefault(prod => prod.ProductID == item.ProductID);
            product.UnitsInStock -= item.Quantity;
        }
        prodContext.SaveChanges();
        Console.WriteLine("Produkty w kosyzku zostały kupione");
    }

    private static bool productAvailable(ProdContext productContext, int UserProductID)
    {
        var query = from product in productContext.Products
                    where product.ProductID == UserProductID && product.UnitsInStock > 0
                    select product;
        return (query?.Count() > 0);
    }

    private static bool productExists(ProdContext productContext, int UserProductID)
    {
        var query = from product in productContext.Products
                    where product.ProductID == UserProductID
                    select product;
        return (query?.Count() > 0);
    }

    private static void showAllProducts(ProdContext prodContext)
    {
        var query = from product in prodContext.Products
                    select product;
        foreach (var product in query)
        {
            product.printProductInStock();
        }
        if (query.Count() == 0)
        {
            Console.WriteLine("Nie znaleziono produktów w bazie danych");
        }
    }
    private static void showAvailableProducts(ProdContext prodContext)
    {
        var query = from product in prodContext.Products
                    where product.UnitsInStock > 0
                    select product;
        foreach (var product in query)
        {
            product.printProductInStock();
        }
        if (query?.Count() == 0)
        {
            Console.WriteLine("Nie znaleziono dostępnych do zakupu produktów w bazie danych");
        }
    }

    private static void showProductsInBasket(ProdContext prodContext, List<InvoiceItem> basketItems)
    {
        foreach (InvoiceItem item in basketItems)
        {
            var product = prodContext.Products
                                  .Include(ii => ii.InvoiceItems)
                                  .FirstOrDefault(ii => ii.ProductID == item.ProductID);

            Console.WriteLine($"{product}: {item.Quantity} szt.");
        }
        if (basketItems.Count() == 0)
        {
            Console.WriteLine("Nie znaleziono produktów w koszyku");
        }
    }

    private static Invoice CreateInvoice(List<InvoiceItem> items)
    {
        return new Invoice
        {
            InvoiceItems = items
        };
    }

    private static void showSoldInTransaction(ProdContext productContext)
    {
        int invoiceNumber;

        Console.Write("Podaj ID transakcji do wypisywania zakupionych pozycji: ");
        invoiceNumber = Int32.Parse(Console.ReadLine());
        Console.WriteLine();

        var invoice = productContext.Invoices
                              .Include(inv => inv.InvoiceItems)
                              .ThenInclude(item => item.Product)
                              .FirstOrDefault(inv => inv.InvoiceNumber == invoiceNumber);
        if (invoice != null)
        {
            Console.WriteLine(invoice);
        }
        else
        {
            Console.WriteLine("Nie znaleziono transakcji o takim ID");
        }
    }

    private static void showInvoicesIncludeProduct(ProdContext productContext)
    {
        int prodID;
        showAllProducts(productContext);
        do
        {
            Console.Write("Podaj ID produktu, dla którego należy wyszukać transakcji: ");
            prodID = Int32.Parse(Console.ReadLine());
            Console.WriteLine();
        } while (!productExists(productContext, prodID));

        var query = from item in productContext.InvoiceItems.Include(ii => ii.Invoice)
                    where item.ProductID == prodID
                    select item;

        foreach (var item in query)
        {
            Console.WriteLine("Invoice #{0}", item.InvoiceNumber);
        }
        if (query?.Count() == 0)
        {
            Console.WriteLine("Nie znaleziono transakcji, w których by występował dany produkt");
        }
    }

    private static void showOptions()
    {
        Console.WriteLine("1. Add new product");
        Console.WriteLine("2. Show all products");
        Console.WriteLine("3. Show available products");
        Console.WriteLine("4. Add product to basket");
        Console.WriteLine("5. Remove product from basket");
        Console.WriteLine("6. Show products in basket");
        Console.WriteLine("7. Buy products in basket");
        Console.WriteLine("8. Show products sold in transaction");
        Console.WriteLine("9. Show invoices having sold the product");
        Console.WriteLine("10. EXIT");
    }

    private static int getUserChoice()
    {
        int choice = 0;
        string input;
        do
        {
            Console.WriteLine();
            showOptions();
            Console.WriteLine("\nPodaj jedną liczbę z zakresu 1-10");
            input = Console.ReadLine();
            int.TryParse(input, out choice);
        } while (choice < 0 || choice > 10);
        Console.WriteLine();
        return choice;
    }

    static void Main()
    {
        ProdContext productContext = new ProdContext();
        List<InvoiceItem> basketItems = new List<InvoiceItem>();

        bool exited = false;

        while (!exited)
        {
            switch (getUserChoice())
            {
                case 1: // Add new product
                    createNewProduct(productContext);
                    break;
                case 2: // Show all products
                    showAllProducts(productContext);
                    break;
                case 3: // Show available products
                    showAvailableProducts(productContext);
                    break;
                case 4: // Add product to basket
                    addProductToBasket(productContext, basketItems);
                    break;
                case 5: // Remove product from basket
                    removeProductFromBasket(productContext, basketItems);
                    break;
                case 6: // Show products in basket
                    showProductsInBasket(productContext, basketItems);
                    break;
                case 7: // Buy products in basket
                    buyProductsInBasket(productContext, basketItems);
                    basketItems = new List<InvoiceItem>();
                    break;
                case 8: // Show products sold in transaction
                    showSoldInTransaction(productContext);
                    break;
                case 9: // Show invoices having sold the product
                    showInvoicesIncludeProduct(productContext);
                    break;
                case 10: // EXIT
                    exited = true;
                    break;
            }
        }

        // Product product = null;

        // Console.WriteLine("Dodać nowego dostawcę? (Tak/Nie)");
        // string choice = Console.ReadLine();
        // bool correctAnswer = false;
        // do
        // {
        //     switch (choice)
        //     {
        //         case "Tak":
        //             supplier = createNewSupplier();
        //             productContext.Suppliers.Add(supplier);
        //             correctAnswer = true;
        //             break;
        //         case "Nie":
        //             showAllSuppliers(productContext);
        //             supplier = findSupplier(productContext);
        //             correctAnswer = true;
        //             break;
        //     }
        // } while (!correctAnswer);

        // do
        // {
        //     Console.WriteLine("Dodać nowy produkt? (Tak/Nie)");
        //     choice = Console.ReadLine();
        //     correctAnswer = false;
        //     switch (choice)
        //     {
        //         case "Tak":
        //             product = createNewProduct();
        //             productContext.Products.Add(product);
        //             correctAnswer = true;
        //             break;
        //         case "Nie":
        //             correctAnswer = true;
        //             break;
        //     }
        //     Console.WriteLine("");
        // } while (!correctAnswer || choice == "Tak");

        // productContext.SaveChanges();



        // product = findProduct(productContext);

        // Console.WriteLine("");

        // Console.WriteLine("Zmienić dostawcę dla produktu na podanego wyżej? (Tak/Nie)");
        // choice = Console.ReadLine();
        // correctAnswer = false;
        // do
        // {
        //     switch (choice)
        //     {
        //         case "Tak":
        //             supplier.Products.Add(product);
        //             product.supplier = supplier;
        //             Console.Write($"\nDla productu: {product.ProductName} zmieniono dostawcę na: {supplier.CompanyName}.\n");
        //             productContext.SaveChanges();
        //             correctAnswer = true;
        //             break;
        //         case "Nie":
        //             correctAnswer = true;
        //             break;
        //     }
        // } while (!correctAnswer);

        // showAllProducts(productContext);

        // showAllSuppliers(productContext);
    }
}