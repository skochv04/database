package zad1;

import jakarta.persistence.*;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Set;

@Entity
@SequenceGenerator(name = "Invoice_SEQ")
public class Invoice {

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO, generator = "Invoice_SEQ")
    private int invoiceID;
    private int invoiceNumber;
    private int quantity = 0;

    @ManyToMany
    @JoinTable(
            name = "Invoice_Products",
            joinColumns = @JoinColumn(name = "invoiceID"),
            inverseJoinColumns = @JoinColumn(name = "productID")
    )
    private Set<Product> products = new HashSet<>();

    public Invoice() {
    }

    public Invoice(int invoiceNumber) {
        this.invoiceNumber = invoiceNumber;
    }

    public Set<Product> getProducts() {
        return products;
    }

    public void addProducts(Product product, int quantity) {
        products.add(product);
        this.quantity += quantity;
    }

    public int getInvoiceNumber() {
        return invoiceNumber;
    }

    public int getQuantity() {
        return quantity;
    }

    @Override
    public String toString() {
        return "Invoice{" +
                "invoiceNumber=" + invoiceNumber +
                '}';
    }
}
