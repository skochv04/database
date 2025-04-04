using System.Runtime.Intrinsics.X86;

public class Product
{
    public int ProductID { get; set; }
    public String? ProductName { get; set; }
    public int UnitsInStock { get; set; }

    // Navigation properties
    public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }

    public override string ToString()
    {
        return $"Product #{ProductID} {ProductName}";
    }

    public void printProductInStock()
    {
        Console.WriteLine($"Product #{ProductID} {ProductName}: {UnitsInStock} szt.");
    }
}