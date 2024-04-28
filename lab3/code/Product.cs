public class Product
{
    public int ProductID { get; set; }
    public string? ProductName { get; set; }
    public int UnitsInStock { get; set; }
    public Supplier? supplier { get; set; } = null;

    public override string ToString()
    {
        var supName = "undefined";
        if (supplier != null) supName = supplier.CompanyName;
        return $"{ProductName}: {UnitsInStock} szt. Dostawca: {supName}";
    }
}