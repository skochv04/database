using System.Runtime.Intrinsics.X86;

public class Product
{
    public int ProductID { get; set; }
    public String? ProductName { get; set; }
    public int UnitsInStock { get; set; }
    public Supplier? supplier { get; set; } = null;
    public override string ToString()
    {
        string supName = "undefined";
        if (supplier != null)
        {
            supName = supplier.CompanyName;
        }
        return $"{ProductName}: {UnitsInStock} szt. Dostawca: {supName}";
    }
}