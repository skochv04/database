package zad1;

import jakarta.persistence.*;

import java.util.ArrayList;
import java.util.List;

@Entity
@SequenceGenerator(name = "Categories_SEQ")
class Category {

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO, generator = "Categories_SEQ")
    int categoryID;
    String name;

    @OneToMany(mappedBy = "category")
    List<Product> products = new ArrayList<>();

    public Category() {
    }

    public Category(String name) {
        this.name = name;
    }

    public int getCategoryID() {
        return categoryID;
    }

    public String getName() {
        return name;
    }

    public List<Product> getProducts() {
        return products;
    }

    public void addProducts(Product product) {
        products.add(product);
    }
}
