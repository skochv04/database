package zad1;

import jakarta.persistence.*;

import javax.management.InvalidAttributeValueException;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Set;

@Entity
@SequenceGenerator(name = "Products_SEQ")
class Product{

    @Id
    @GeneratedValue(strategy = GenerationType.SEQUENCE, generator = "Products_SEQ")
    private int productID;

    private String productName;
    private int unitsInStock;

    @ManyToOne
    @JoinColumn(name = "supplierID")
    private Supplier supplier;

    @ManyToOne
    @JoinColumn(name = "categoryID")
    private Category category;

    @ManyToMany(mappedBy = "products")
    private Set<Invoice> invoices = new HashSet<>();

    public Product() {}

    Product(String productName, int unitsInStock) {
        this.productName = productName;
        this.unitsInStock = unitsInStock;
    }

    int getProductID() {
        return productID;
    }

    String getProductName() {
        return productName;
    }

    int getUnitsInStock() {
        return unitsInStock;
    }

    public Supplier getSupplier() {
        return supplier;
    }

    public void setSupplier(Supplier supplier) {
        this.supplier = supplier;
    }

    public void setCategory(Category category) {
        this.category = category;
    }

    public Category getCategory() {
        return category;
    }

    @Override
    public String toString() {
        return "Product{" +
                "productName='" + productName + '\'' +
                '}';
    }

    public Set<Invoice> getInvoices() {
        return invoices;
    }

    public void sell(Invoice invoice, int quantity) {
        if (unitsInStock < quantity) {
            System.out.println("Unable to sell" + quantity + " products");
            return;
        }
        unitsInStock -= quantity;
        invoice.addProducts(this, quantity);
        invoices.add(invoice);
    }
}
