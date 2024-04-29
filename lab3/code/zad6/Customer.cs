using System.ComponentModel.DataAnnotations.Schema;

namespace zad5;

[Table("Customers")] 
internal class Customer : Company
{
    public int CustomerID { get; set; }
    public int Discount { get; set; } // In %

    public override string ToString()
    {
        return $"{base.ToString()} (klient)";
    }
}