
# Entity Framework

ćwiczenie 3


---

**Imiona i nazwiska autorów: Stas Kochevenko & Wiktor Dybalski**

--- 

# Zadanie 1 - wprowadzenie

Zostały dodane podstawowe klasy Product, ProdContext oraz Program, baza danych MyProductDatabase.

![](img/1.png)

<!-- <div style="page-break-after: always;"></div> -->

Kod:

- Product.cs

```c#
using System.Runtime.Intrinsics.X86;

public class Product
{
    public int ProductID { get; set; }
    public String? ProductName { get; set; }
    public int UnitsInStock { get; set; }
}
```

- ProdContext.cs

```c#
using Microsoft.EntityFrameworkCore;
public class ProdContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlite("Datasource=MyProductDatabase");
    }
}
```

- Program.cs

```c#
using System;
using System.Linq;

Console.WriteLine("Podaj nazwę produktu: ");
String? prodName = Console.ReadLine();

ProdContext productContext = new ProdContext();

Product product = new Product { ProductName = prodName, UnitsInStock = 24, Supplier = supplier };
productContext.Products.Add(product);
productContext.SaveChanges();

var query = from prod in productContext.Products
            select new { prod.ProductID, prod.ProductName };

foreach (var pName in query)
{
    Console.WriteLine(pName);
}
```

--- 

# Zadanie 2 - wprowadzenie pojęcia Dostawcy

Została dodana klasa Supplier oraz zaktualizowana odpowiednio klasa Product, żeby mieć połączenie z klasą Supplier.
Schemat zmienionej bazy danych, wygenerowany przez DataGrip:

![](img/2.png)

Testowanie dodania nowego dostawcy i ustawienia dostawcy poprzednio wprowadzonego produktu na dodanego dostawcę. Tabela Products przed wywołaniem metody dodającej:

![](img/4.png)

Wynik działania programu w postaci konsolowych komunikatów:

![](img/3.png)

Sprawdżmy trwałość zmian za pomocą DataGrip:

![](img/5.png)

Testowanie ustawienia dostawcy dla poprzednio wprowadzonego produktu. Wynik działania programu w postaci konsolowych komunikatów:

![](img/6.png)

Sprawdżmy trwałość zmian za pomocą DataGrip:

![](img/7.png)

Kod:

- Product.cs

```c#
using System.Runtime.Intrinsics.X86;

public class Product
{
    public int ProductID { get; set; }
    public String? ProductName { get; set; }
    public int UnitsInStock { get; set; }
    public Supplier? supplier { get; set; } = null;
    public override string ToString()
    {
        return $"{ProductName}: {UnitsInStock} szt";
    }
}
```

- Supplier.cs

```c#
public class Supplier
{
    public int SupplierID { get; set; }
    public string CompanyName { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public override string ToString()
    {
        return CompanyName;
    }
}
```

- ProdContext.cs

```c#
using Microsoft.EntityFrameworkCore;
public class ProdContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlite("Datasource=MyProductDatabase");
    }
}
```

- Program.cs

```c#
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
        Console.Write($"Został utworzony dostawca: {supplier}.\n");

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

        Supplier supplier = null;

        Console.WriteLine("Dodać nowego dostawcę? (Tak/Nie)");
        string choice = Console.ReadLine();
        bool correctAnswer = false;
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

        productContext.SaveChanges();

        Console.WriteLine("");

        Product product = findProduct(productContext);

        Console.WriteLine("");

        Console.WriteLine("Zmienić dostawcę dla produktu na podanego wyżej? (Tak/Nie)");
        choice = Console.ReadLine();
        correctAnswer = false;
        do
        {
            switch (choice)
            {
                case "Tak":
                    product.supplier = supplier;
                    Console.Write($"\nDla productu: {product.ProductName} zmieniono dostawcę na: {supplier.CompanyName}.\n");
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
```

--- 

# Zadanie 3 - odwrócenie relacji Supplier -> Product

- Z klasy "Product" został usunięty atrybut "Supplier" zgodnie ze schematem. 
- W klasie "Supplier" dodano kolekcję produktów, dostarczanych przez danego dostawcę.
- W kodzie głównego programu został dodany fragment kodu odpowiedzialny za dodanie nowych produktów. W poruwnywniu do zadania 1, jedyną zmianą było ustawienie produktu do dostawcy, zamiast ustawienia dostawcy do produktu. Niżej będzie podany fragment kodu, który uległ zmianie.
- Klasa ProdContext pozostała bez zmian.
Schemat zmienionej bazy danych, wygenerowany przez DataGrip:

![](img/2.png)

Jak widać, pomimo odwrócenia relacji, schemat bazy danych się nie zmienił. Entity Framework dokonał optymalizacji, dzięki czemu uniknęliśmy powielania danych w tabeli Suppliers (ponieważ nie możemy trzymać listy w polu). Oprócz tego takie podejście miało by gorsze konsekwencje: każdy rekord miałby nowy SupplierID, mimo że byłby to ten sam dostawca, jedynie mający przypisany inny ProductID. W takiej sytuacji nie bylibyśmy w stanie odróżnic dostawców.

Testowanie dodania nowych produktów i ustawienia ich dostawcy na nowo stworzonego. Tabela Products przed wywołaniem metody dodającej:

![](img/7.png)

Wynik działania programu w postaci konsolowych komunikatów:

![](img/8.png)

Sprawdżmy trwałość zmian za pomocą DataGrip. Tabela Product po dodaniu produktów:

![](img/9.png)

Sprawdżmy trwałość zmian za pomocą DataGrip. Tabela Product po wprowadzeniu zmiany dostawcy:

![](img/10.png)

Kod:

- Product.cs

```c#
using System.Runtime.Intrinsics.X86;

public class Product
{
    public int ProductID { get; set; }
    public String? ProductName { get; set; }
    public int UnitsInStock { get; set; }
    public override string ToString()
    {
        return $"{ProductName}: {UnitsInStock} szt.";
    }
}
```

- Supplier.cs

```c#
public class Supplier
{
    public int SupplierID { get; set; }
    public string CompanyName { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
    public override string ToString()
    {
        return CompanyName;
    }
}
```

- Program.cs (fragment starego kodu)

```c#
static void Main()
    {
        ...
            product.supplier = supplier;
        ...
    }
```

- Program.cs (fragment nowego kodu)

```c#
static void Main()
    {   
        ...

        do {
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

        ...

            supplier.Products.Add(product);

        ...
    }
```
