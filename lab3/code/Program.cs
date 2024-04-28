

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

using System.Collections;
using Lab3;

internal class Program
{
    
    private static Product CreateNewProduct()
    {
        Console.Write("Podaj nazwę nowego produktu: ");
        var prodName = Console.ReadLine();
        Console.Write("Podaj ilość jednostek danego produktu dostępnych w sklepie: ");
        var quantity = int.Parse(Console.ReadLine());
    
        var product = new Product { ProductName = prodName, UnitsInStock = quantity };
        Console.Write($"Został utworzony produkt: {product}");
    
        return product;
    }
    
    private static Product? FindProduct(ProdContext productContext)
    {
        Console.Write("Podaj ID produktu do wyszukiwania: ");
        var prodID = int.Parse(Console.ReadLine());
    
        var query = from prod in productContext.Products
            where prod.ProductID == prodID
            select prod;
    
        return query.FirstOrDefault();
    }
    
    private static void ShowAllProducts(ProdContext productContext)
    {
        Console.WriteLine("Lista produktów: ");
        foreach (var product in productContext.Products) Console.WriteLine($"{product.ProductID} | {product}");
    }
    
    private static Supplier CreateNewSupplier()
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
    
    private static Supplier? FindSupplier(ProdContext productContext)
    {
        Console.Write("Podaj ID dostawcy do wyszukiwania: ");
        var supplierID = int.Parse(Console.ReadLine());
    
        var query = from supplier in productContext.Suppliers
            where supplier.SupplierID == supplierID
            select supplier;
    
        return query.FirstOrDefault();
    }
    
    private static void ShowAllSuppliers(ProdContext productContext)
    {
        Console.WriteLine("Lista dostawców: ");
        foreach (var supplier in productContext.Suppliers) Console.WriteLine($"{supplier.SupplierID} | {supplier}");
    }

    
    // ------------Task E------------------
    private static CompanyH? CreateCompanyH()
    {
        Console.Write("Podaj typ nowej firmy (Supplier/Customer): ");
        var companyType = Console.ReadLine();
        
        Console.Write("Podaj nazwę nowej firmy: ");
        var companyName = Console.ReadLine();
        
        Console.Write("Podaj ulicę nowej firmy: ");
        var companyStreet = Console.ReadLine();

        Console.Write("Podaj miasto nowej firmy: ");
        var companyCity = Console.ReadLine();

        Console.Write("Podaj kod pocztowy nowej firmy: ");
        var companyZipCode = Console.ReadLine();

        decimal? companyDiscount;
        String? companyBankAccountNumber;
        CompanyH companyH;
        switch (companyType)
        {
            case "Supplier":
                Console.Write("Podaj numer konta bankowego nowej firmy: ");
                companyBankAccountNumber = Console.ReadLine();
                companyH = new SupplierH {CompanyType = companyType, CompanyName = companyName, City = companyCity, Street = companyStreet, ZipCode = companyZipCode, BankAccountNumber = companyBankAccountNumber};
                
                break;
            case "Customer":
                Console.Write("Podaj zniżkę nowej firmy: ");
                companyDiscount = decimal.Parse(Console.ReadLine());
                companyH = new CustomerH {CompanyType = companyType, CompanyName = companyName, City = companyCity, Street = companyStreet, ZipCode = companyZipCode, Discount = companyDiscount};
                break;
            default:
                Console.Write("Nie podano typu firmy!");
                return null;
        }
        return companyH;
    }

    private static void RemoveCompanyH(ProdContext productContext)
    {
        Console.Write("Podaj ID firmy do usunięcia: ");
        var companyId = int.Parse(Console.ReadLine());
        var company = productContext.CompaniesH.FirstOrDefault(comp => comp.CompanyId == companyId);
        
        if (company == null)
        {
            Console.WriteLine("Nie znaleziono firmy o podanym ID.");
            return;
        }
        productContext.CompaniesH.Remove(company);
        productContext.SaveChanges();
        Console.WriteLine("Firma została usunięta.");
    }
    
    private static CompanyH? FindCompanyH(ProdContext productContext)
    {
        Console.Write("Podaj ID firmy do wyszukiwania: ");
        var companyId = int.Parse(Console.ReadLine());
    
        var query = from comp in productContext.CompaniesH
            where comp.CompanyId == companyId
            select comp;
    
        return query.FirstOrDefault();
    }
    
    private static void ShowAllSuppliersFromCompaniesH(ProdContext productContext)
    {
        Console.WriteLine("Lista dostawców: ");
        foreach (var company in productContext.CompaniesH)
        {
            if (company.CompanyType == "Supplier")
            {
                Console.WriteLine($"{company.CompanyId} | {company}");   
            }
        }
    }
    
    private static void ShowAllCustomersH(ProdContext productContext)
    {
        Console.WriteLine("Lista klientów: ");
        foreach (var company in productContext.CompaniesH)
        {
            if (company.CompanyType == "Customer")
            {
                Console.WriteLine($"{company.CompanyId} | {company}");   
            }
        }
    }
    
    private static void ShowAllCompaniesH(ProdContext productContext)
    {
        Console.WriteLine("Lista firm: ");
        foreach (var company  in productContext.CompaniesH) Console.WriteLine($"{company.CompanyId} | {company}");
    }

    
    // ---------------Task F-------------------
    // private static CompanyH? CreateCompanyT()
    // {
    //     Console.Write("Podaj typ nowej firmy (Supplier/Customer): ");
    //     var companyType = Console.ReadLine();
    //     
    //     Console.Write("Podaj nazwę nowej firmy: ");
    //     var companyName = Console.ReadLine();
    //     
    //     Console.Write("Podaj ulicę nowej firmy: ");
    //     var companyStreet = Console.ReadLine();
    //
    //     Console.Write("Podaj miasto nowej firmy: ");
    //     var companyCity = Console.ReadLine();
    //
    //     Console.Write("Podaj kod pocztowy nowej firmy: ");
    //     var companyZipCode = Console.ReadLine();
    //
    //     decimal? companyDiscount;
    //     String? companyBankAccountNumber;
    //     switch (companyType)
    //     {
    //         case "Supplier":
    //             Console.Write("Podaj numer konta bankowego nowej firmy: ");
    //             companyBankAccountNumber = Console.ReadLine();
    //             companyDiscount = null;
    //             
    //             break;
    //         case "Customer":
    //             Console.Write("Podaj zniżkę nowej firmy: ");
    //             companyDiscount = decimal.Parse(Console.ReadLine());
    //             companyBankAccountNumber = null;
    //             break;
    //         default:
    //             Console.Write("Nie podano typu firmy!");
    //             return null;
    //     }
    //
    //     CompanyH companyH = new CompanyH {CompanyType = companyType, CompanyName = companyName, City = companyCity, Street = companyStreet, ZipCode = companyZipCode, BankAccountNumber = companyBankAccountNumber, Discount = companyDiscount};
    //     return companyH;
    // }
    //
    // private static void RemoveCompanyT(ProdContext productContext)
    // {
    //     Console.Write("Podaj ID firmy do usunięcia: ");
    //     var companyId = int.Parse(Console.ReadLine());
    //     var company = productContext.Companies.FirstOrDefault(comp => comp.CompanyId == companyId);
    //     
    //     if (company == null)
    //     {
    //         Console.WriteLine("Nie znaleziono firmy o podanym ID.");
    //         return;
    //     }
    //     productContext.Companies.Remove(company);
    //     productContext.SaveChanges();
    //     Console.WriteLine("Firma została usunięta.");
    // }
    //
    // private static CompanyH? FindCompanyT(ProdContext productContext)
    // {
    //     Console.Write("Podaj ID firmy do wyszukiwania: ");
    //     var companyId = int.Parse(Console.ReadLine());
    //
    //     var query = from comp in productContext.Companies
    //         where comp.CompanyId == companyId
    //         select comp;
    //
    //     return query.FirstOrDefault();
    // }
    //
    // private static void ShowAllSuppliersFromCompaniesT(ProdContext productContext)
    // {
    //     Console.WriteLine("Lista dostawców: ");
    //     foreach (var company in productContext.Companies)
    //     {
    //         if (company.CompanyType == "Supplier")
    //         {
    //             Console.WriteLine($"{company.CompanyId} | {company}");   
    //         }
    //     }
    // }
    //
    // private static void ShowAllCustomersT(ProdContext productContext)
    // {
    //     Console.WriteLine("Lista klientów: ");
    //     foreach (var company in productContext.Companies)
    //     {
    //         if (company.CompanyType == "Customer")
    //         {
    //             Console.WriteLine($"{company.CompanyId} | {company}");   
    //         }
    //     }
    // }
    //
    // private static void ShowAllCompaniesT(ProdContext productContext)
    // {
    //     Console.WriteLine("Lista firm: ");
    //     foreach (var company  in productContext.Companies) Console.WriteLine($"{company.CompanyId} | {company}");
    // }    
    
    
    
    // Main Function
    private static void Main()
    {
        
        
        // MAIN FOR A-D TASKS
        
    //     var productContext = new ProdContext();
    //     Supplier supplier = null;
    //     Product product = null;
    //
    //     Console.WriteLine("Dodać nowego dostawcę? (Tak/Nie)");
    //     var choice = Console.ReadLine();
    //     var correctAnswer = false;
    //     do
    //     {
    //         switch (choice)
    //         {
    //             case "Tak":
    //                 supplier = createNewSupplier();
    //                 productContext.Suppliers.Add(supplier);
    //                 correctAnswer = true;
    //                 break;
    //             case "Nie":
    //                 showAllSuppliers(productContext);
    //                 supplier = findSupplier(productContext);
    //                 correctAnswer = true;
    //                 break;
    //         }
    //     } while (!correctAnswer);
    //
    //     do
    //     {
    //         Console.WriteLine("Dodać nowy produkt? (Tak/Nie)");
    //         choice = Console.ReadLine();
    //         correctAnswer = false;
    //         switch (choice)
    //         {
    //             case "Tak":
    //                 product = createNewProduct();
    //                 productContext.Products.Add(product);
    //                 correctAnswer = true;
    //                 break;
    //             case "Nie":
    //                 correctAnswer = true;
    //                 break;
    //         }
    //
    //         Console.WriteLine("");
    //     } while (!correctAnswer || choice == "Tak");
    //
    //     productContext.SaveChanges();
    //
    //
    //     product = findProduct(productContext);
    //
    //     Console.WriteLine("");
    //
    //     Console.WriteLine("Zmienić dostawcę dla produktu na podanego wyżej? (Tak/Nie)");
    //     choice = Console.ReadLine();
    //     correctAnswer = false;
    //     do
    //     {
    //         switch (choice)
    //         {
    //             case "Tak":
    //                 supplier.Products.Add(product);
    //                 product.supplier = supplier;
    //                 Console.Write(
    //                     $"\nDla productu: {product.ProductName} zmieniono dostawcę na: {supplier.CompanyName}.\n");
    //                 productContext.SaveChanges();
    //                 correctAnswer = true;
    //                 break;
    //             case "Nie":
    //                 correctAnswer = true;
    //                 break;
    //         }
    //     } while (!correctAnswer);
    //
    //     showAllProducts(productContext);
    //
    //     showAllSuppliers(productContext);
    
    // MAIN FOR E TASK
    
    var productContext = new ProdContext();
    CompanyH? company = null;
    var correctAnswer = false;
    String? choice;
    do
    {
        Console.WriteLine("Dodać nową firmę? (Tak/Nie)");
        choice = Console.ReadLine();
        switch (choice)
        {
            case "Tak":
                company = CreateCompanyH();
                if (company == null)
                {
                    return;
                }
                productContext.CompaniesH.Add(company);
                correctAnswer = true;
                break;
            case "Nie":
                correctAnswer = true;
                break;
        }
    } while (!correctAnswer || choice == "Tak");
    
    productContext.SaveChanges();
    
    Console.WriteLine(FindCompanyH(productContext));
    ShowAllSuppliersFromCompaniesH(productContext);
    ShowAllCustomersH(productContext);
    ShowAllCompaniesH(productContext);
    
    
    // MAIN FOR F TASK
    //
    // var productContext = new ProdContext();
    // CompanyH? company = null;
    // var correctAnswer = false;
    // String? choice;
    //
    // do
    // {
    //     Console.WriteLine("Dodać nową firmę? (Tak/Nie)");
    //     choice = Console.ReadLine();
    //     switch (choice)
    //     {
    //         case "Tak":
    //             company = CreateCompanyH();
    //             if (company == null)
    //             {
    //                 return;
    //             }
    //             productContext.Companies.Add(company);
    //             correctAnswer = true;
    //             break;
    //         case "Nie":
    //             correctAnswer = true;
    //             break;
    //     }
    // } while (!correctAnswer || choice == "Tak");
    //
    // productContext.SaveChanges();
    //
    // Console.WriteLine(FindCompanyH(productContext));
    // ShowAllSuppliersFromCompaniesH(productContext);
    // ShowAllCustomersH(productContext);
    // ShowAllCompaniesH(productContext);
    //
    }
}