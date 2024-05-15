package zad1;

import jakarta.persistence.*;

import java.util.ArrayList;
import java.util.List;

@Entity
@SequenceGenerator(name = "Supplier_SEQ")
class Supplier {

    @Id
    @GeneratedValue(strategy = GenerationType.SEQUENCE, generator = "Supplier_SEQ")
    private int supplierID;

    private String companyName;
    private String street;
    private String city;

    @OneToMany(mappedBy = "supplier")
    private List<Product> products = new ArrayList<>();

    public Supplier() {}

    Supplier(String companyName, String street, String city) {
        this.companyName = companyName;
        this.street = street;
        this.city = city;
    }

    int getSupplierID() {
        return supplierID;
    }
    String getCompanyName() {
        return companyName;
    }
    String getStreet() {
        return street;
    }
    String getCity() {
        return city;
    }

    List<Product> getProducts() {
        return products;
    }
    void addProduct(Product product) {
        this.products.add(product);
    }
}