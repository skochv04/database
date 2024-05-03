using System.ComponentModel.DataAnnotations.Schema;

namespace zad5;

[Table("Supplier")] 
internal class Supplier : Company
{
    public int SupplierID { get; set; }
    public string BankAccountNumber { get; set; } = String.Empty;

    public override string ToString()
    {
        return $"{base.ToString()} (dostawca)";
    }
}