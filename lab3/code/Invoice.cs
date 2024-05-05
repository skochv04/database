using System.ComponentModel.DataAnnotations;
using System.Text;

public class Invoice
{
    // Primary key
    [Key]
    public int InvoiceNumber { get; set; }
    public ICollection<InvoiceItem> InvoiceItems { get; set; }
    public override string ToString()
    {
        StringBuilder sb = new($"Invoice {InvoiceNumber}:");
        int i = 0;
        foreach (InvoiceItem item in InvoiceItems)
        {
            i++;
            sb.Append($"\n\t{i}) {item}");
        }
        return sb.ToString();
    }
}