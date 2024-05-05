using Console = System.Console;

namespace zad5;

internal class Program
{
    private static Company? CreateCompany()
    {
        var companyType = "";
        do
        {
            Console.Write("Podaj typ nowej firmy (Supplier/Customer): ");
            companyType = Console.ReadLine();
        }
        while (companyType?.Trim().ToLower() != "supplier" && companyType?.Trim().ToLower() != "customer");

        Console.Write("Podaj nazwę nowej firmy: ");
        var companyName = Console.ReadLine();

        Console.Write("Podaj ulicę nowej firmy: ");
        var companyStreet = Console.ReadLine();

        Console.Write("Podaj miasto nowej firmy: ");
        var companyCity = Console.ReadLine();

        Console.Write("Podaj kod pocztowy nowej firmy: ");
        var companyZipCode = Console.ReadLine();

        switch (companyType?.Trim().ToLower())
        {
            case "supplier":
                Console.Write("Podaj numer konta bankowego nowej firmy: ");
                String? companyBankAccountNumber = Console.ReadLine();
                return new Supplier
                {
                    CompanyName = companyName,
                    City = companyCity,
                    Street = companyStreet,
                    ZipCode = companyZipCode,
                    BankAccountNumber = companyBankAccountNumber
                };

            case "customer":
                Console.Write("Podaj zniżkę nowej firmy: ");
                int companyDiscount = int.Parse(Console.ReadLine());
                return new Customer
                {
                    CompanyName = companyName,
                    City = companyCity,
                    Street = companyStreet,
                    ZipCode = companyZipCode,
                    Discount = companyDiscount,
                };
            default:
                Console.WriteLine("Nie podano typu firmy!");
                return null;
        }
    }

    private static void RemoveCompany(CompanyContext companyContext)
    {
        Console.Write("Podaj ID firmy do usunięcia: ");
        var companyId = int.Parse(Console.ReadLine());
        var company = companyContext.Companies.FirstOrDefault(comp => comp.CompanyID == companyId);

        if (company == null)
        {
            Console.WriteLine("Nie znaleziono firmy o podanym ID.");
            return;
        }

        companyContext.Companies.Remove(company);
        companyContext.SaveChanges();
        Console.WriteLine("Firma została usunięta.");
    }

    private static Company? FindCompany(CompanyContext productContext)
    {
        Console.Write("Podaj ID firmy do wyszukiwania: ");
        var companyId = int.Parse(Console.ReadLine());

        var query = from comp in productContext.Companies
                    where comp.CompanyID == companyId
                    select comp;

        return query.FirstOrDefault();
    }

    private static void ShowAllSuppliers(CompanyContext companyContext)
    {
        Console.WriteLine("Lista dostawców: ");

        foreach (Supplier customer in companyContext.Suppliers)
        {
            Console.WriteLine(customer);
        }
    }

    private static void ShowAllCustomers(CompanyContext companyContext)
    {
        Console.WriteLine("Lista klientów: ");
        foreach (Customer customer in companyContext.Customers)
        {
            Console.WriteLine(customer);
        }
    }

    private static void ShowAllCompanies(CompanyContext companyContext)
    {
        Console.WriteLine("Lista wszystkich firm: ");
        foreach (Company company in companyContext.Companies)
        {
            Console.WriteLine(company);
        }
    }


    // Main Function
    private static void Main()
    {
        var companyContext = new CompanyContext();

        Company? company = null;
        var correctAnswer = false;
        String? choice;
        do
        {
            Console.WriteLine("Dodać nową firmę? (Tak/Nie)");
            choice = Console.ReadLine();
            switch (choice)
            {
                case "Tak":
                    company = CreateCompany();
                    if (company == null)
                    {
                        return;
                    }

                    companyContext.Companies.Add(company);
                    companyContext.SaveChanges();
                    correctAnswer = true;
                    break;
                case "Nie":
                    correctAnswer = true;
                    companyContext.SaveChanges();
                    break;
            }
        } while (!correctAnswer || choice == "Tak");
        ShowAllCompanies(companyContext);
        ShowAllSuppliers(companyContext);
        ShowAllCustomers(companyContext);

    }
}