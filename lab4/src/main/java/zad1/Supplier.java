package zad1;

import jakarta.persistence.*;

import java.util.ArrayList;
import java.util.List;

@Entity
@SecondaryTable(name = "Address")
@SequenceGenerator(name = "Supplier_SEQ")
class Supplier {

    @Id
    @GeneratedValue(strategy = GenerationType.SEQUENCE, generator = "Supplier_SEQ")
    private int supplierID;

    private String companyName;

    @Column(table = "Address")
    private String city;
    @Column(table = "Address")
    private String street;

    @OneToMany(mappedBy = "supplier")
    private List<Product> products = new ArrayList<>();

    public Supplier() {}

    public Supplier(String companyName, String city, String street) {
        this.companyName = companyName;
        this.city = city;
        this.street = street;
    }
    public String getCity() {
        return city;
    }
    public String getStreet() {
        return street;
    }
    int getSupplierID() {
        return supplierID;
    }
    String getCompanyName() {
        return companyName;
    }
    List<Product> getProducts() {
        return products;
    }
    void addProduct(Product product) {
        this.products.add(product);
    }
    @Override
    public String toString() { return companyName; }
}