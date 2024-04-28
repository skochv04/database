using System.ComponentModel.DataAnnotations;

namespace Lab3;

public class CompanyH
{
    [Key]
    public int CompanyId { get; set; }
    public String? CompanyType { get; set; }
    public String? CompanyName { get; set; }
    public String? Street { get; set; }
    public String? City { get; set; }
    public String? ZipCode { get; set; }
    public String? BankAccountNumber { get; set; }
    public decimal? Discount { get; set; }

    public override string ToString()
    {
        return "Nazwa firmy: " + $"{CompanyName}: " + ", typ firmy: " + $"{CompanyType}";
    }
}