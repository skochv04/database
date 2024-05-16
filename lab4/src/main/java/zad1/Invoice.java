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

    @ManyToMany(cascade = CascadeType.PERSIST, fetch = FetchType.EAGER)
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
        if (product.getUnitsInStock() < quantity) {
            System.out.println("Unable to sell" + quantity + " products");
            return;
        }
        products.add(product);
        this.quantity += quantity;
    }

    public int getInvoiceNumber() {
        return invoiceNumber;
    }

    public int getQuantity() {
        return quantity;
    }

    public void updateProduct(int quantity) {
        this.quantity += quantity;
    }

    @Override
    public String toString() {
        return String.valueOf(invoiceNumber);
    }
}
