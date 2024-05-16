package zad1;

import jakarta.persistence.EntityManager;
import jakarta.persistence.EntityManagerFactory;
import jakarta.persistence.EntityTransaction;
import jakarta.persistence.Persistence;
import org.hibernate.Transaction;
import org.hibernate.cfg.Configuration;

import java.util.List;
import java.util.Scanner;
import java.util.Set;


class MainJPA {
    private static final EntityManagerFactory emf;

    static {
        try {
            emf = Persistence.createEntityManagerFactory("derby");
        } catch (Throwable ex) {
            throw new ExceptionInInitializerError(ex);
        }
    }
    public static EntityManager getEntityManager() {
        return emf.createEntityManager();
    }

    public static void main(String[] args) {
        final EntityManager entityManager = getEntityManager();
        EntityTransaction tx;

        Scanner scanner = new Scanner(System.in);
        int option;

        while (true) {
            System.out.println("--------------------");
            System.out.println("What do you want to do?: ");
            System.out.println("0. Add template");
            System.out.println("1. Add product");
            System.out.println("2. Add supplier");
            System.out.println("3. Show products");
            System.out.println("4. Show suppliers");
            System.out.println("5. Show category");
            System.out.println("6. Show products with Category");
            System.out.println("7. Show Sold products on InvoiceID: ");
            System.out.println("8. Show Invoices that product had been sold: ");
            System.out.println("9. Show Product Category");
            System.out.println("10. Exit");
            System.out.println("--------------------");
            option = scanner.nextInt();
            scanner.nextLine();

            switch (option) {
                case 0:
                    tx = entityManager.getTransaction();
                    tx.begin();

                    Supplier supplier1 = new Supplier("Acme Corp", "123 Main St", "Springfield");
                    Supplier supplier2 = new Supplier("Tech Solutions", "456 Elm St", "Shelbyville");
                    Supplier supplier3 = new Supplier("Global Traders", "789 Oak St", "Capital City");

                    Product product1 = new Product("Laptop", 20);
                    Product product2 = new Product("Tablet", 30);
                    Product product3 = new Product("Smartphone", 50);
                    Product product4 = new Product("Books", 15);
                    Product product5 = new Product("Car", 10);

                    Category category1 = new Category("Devices");
                    Category category2 = new Category("School");
                    Category category3 = new Category("Vehicles");

                    Invoice invoice1 = new Invoice(1001);
                    Invoice invoice2 = new Invoice(1002);
                    int soldNumber1 = 4;
                    int soldNumber2 = 6;
                    int soldNumber3 = 8;

                    product1.sell(invoice1, soldNumber1);
                    product2.sell(invoice1, soldNumber2);
                    invoice1.addProducts(product3, soldNumber3);
                    invoice2.addProducts(product4, soldNumber3);
                    product3.sell(invoice1, soldNumber1);


                    product1.setSupplier(supplier1);
                    product1.setCategory(category1);

                    product2.setSupplier(supplier1);
                    product2.setCategory(category1);

                    product3.setSupplier(supplier2);
                    product3.setCategory(category1);

                    product4.setSupplier(supplier2);
                    product4.setCategory(category2);

                    product5.setSupplier(supplier3);
                    product5.setCategory(category3);


                    supplier1.addProduct(product1);
                    supplier1.addProduct(product2);

                    supplier2.addProduct(product3);
                    supplier2.addProduct(product4);

                    supplier3.addProduct(product5);

                    category1.addProducts(product1);
                    category1.addProducts(product2);
                    category1.addProducts(product3);

                    category2.addProducts(product4);

                    category3.addProducts(product5);

                    entityManager.persist(supplier1);
                    entityManager.persist(supplier2);
                    entityManager.persist(supplier3);

                    entityManager.persist(category1);
                    entityManager.persist(category2);
                    entityManager.persist(category3);

                    entityManager.persist(product1);
                    entityManager.persist(product2);
                    entityManager.persist(product3);
                    entityManager.persist(product4);
                    entityManager.persist(product5);

                    entityManager.persist(invoice1);
                    entityManager.persist(invoice2);


                    tx.commit();
                    System.out.println("Product added successfully.");
                    break;
                case 1:
                    tx = entityManager.getTransaction();
                    tx.begin();

                    System.out.print("Enter product name: ");
                    String productName = scanner.nextLine();
                    System.out.print("Enter units in stock: ");
                    int unitsInStock = scanner.nextInt();
                    Product product = new Product(productName, unitsInStock);
                    entityManager.persist(product);
                    tx.commit();
                    System.out.println("Product added successfully.");
                    break;

                case 2:
                    tx = entityManager.getTransaction();
                    tx.begin();

                    System.out.print("Enter supplier name: ");
                    String supplierName = scanner.nextLine();
                    System.out.print("Enter supplier street: ");
                    String supplierRoad = scanner.nextLine();
                    System.out.print("Enter supplier city: ");
                    String supplierCity = scanner.nextLine();

                    Supplier supplier = new Supplier(supplierName, supplierRoad, supplierCity);
                    entityManager.persist(supplier);
                    tx.commit();
                    System.out.println("Supplier added successfully.");
                    break;
                case 3:
                    tx = entityManager.getTransaction();
                    tx.begin();

                    List<Product> products = entityManager.createQuery("FROM Product", Product.class).getResultList();
                    if (!products.isEmpty()) {
                        for (Product p : products) {
                            System.out.println("Product ID: " + p.getProductID());
                            System.out.println("Name: " + p.getProductName());
                            System.out.println("Supplier: " + p.getSupplier());
                            System.out.println("Category: " + p.getCategory());
                        }
                    } else {
                        System.out.println("No products found.");
                    }
                    tx.commit();
                    break;

                case 4:
                    tx = entityManager.getTransaction();
                    tx.begin();

                    List<Supplier> suppliers = entityManager.createQuery("FROM Supplier", Supplier.class).getResultList();
                    if (!suppliers.isEmpty()) {
                        for (Supplier s : suppliers) {
                            System.out.print("Supplier ID: " + s.getSupplierID());
                            System.out.print(", Name: " + s.getCompanyName());
                            System.out.print(", Street: " + s.getStreet());
                            System.out.println(", City: " + s.getCity());
                            System.out.println("Products: " + s.getProducts());
                        }
                    } else {
                        System.out.println("No suppliers found.");
                    }
                    tx.commit();
                    break;
                case 5:
                    tx = entityManager.getTransaction();
                    tx.begin();

                    List<Category> categories = entityManager.createQuery("FROM Category ", Category.class).getResultList();
                    if (!categories.isEmpty()) {
                        for (Category c : categories) {
                            System.out.print("Category ID: " + c.getCategoryID());
                            System.out.print(", Name: " + c.getName());
                            System.out.println(", Products: " + c.getProducts());
                        }
                    } else {
                        System.out.println("No categories found.");
                    }
                    tx.commit();
                    break;
                case 6:
                    tx = entityManager.getTransaction();
                    tx.begin();

                    System.out.println("Category Name: ");
                    String categoryName = scanner.nextLine();
                    List<Product> filteredProducts = entityManager.createQuery("FROM Product p WHERE p.category.name = :categoryName", Product.class).setParameter("categoryName", categoryName).getResultList();
                    if (!filteredProducts.isEmpty()) {
                        for (Product p : filteredProducts) {
                            System.out.print("Product ID: " + p.getProductID());
                            System.out.print(", Name: " + p.getProductName());
                            System.out.println(", Supplier: " + p.getSupplier());
                            System.out.println(", Category: " + p.getCategory());
                        }
                    } else {
                        System.out.println("No products found.");
                    }
                    tx.commit();
                    break;
                case 7:
                    tx = entityManager.getTransaction();
                    tx.begin();

                    System.out.println("Invoice Number: ");
                    String invoiceNumber = scanner.nextLine();
                    Invoice invoice = entityManager.createQuery("FROM Invoice inv where inv.invoiceNumber = :invoiceNumber", Invoice.class).setParameter("invoiceNumber", invoiceNumber).getSingleResult();
                    if (invoice != null) {
                        for (Product p : invoice.getProducts()) {
                            System.out.print("ProductID: " + p.getProductID());
                            System.out.print(", Name: " + p.getProductName());
                            System.out.print(", Supplier: " + p.getSupplier());
                            System.out.println(", Category: " + p.getCategory());
                        }
                    } else {
                        System.out.println("No invoice found.");
                    }
                    tx.commit();
                    break;
                case 8:
                    tx = entityManager.getTransaction();
                    tx.begin();

                    System.out.println("Product Name: ");
                    productName = scanner.nextLine();
                    products = entityManager.createQuery("FROM Product p WHERE p.productName = :productName", Product.class)
                            .setParameter("productName", productName)
                            .getResultList();
                    if (!products.isEmpty()) {
                        for (Product p : products) {
                            System.out.println("Product: " + p.getProductName() + " appears in the following invoices:");
                            for (Invoice inv : p.getInvoices()) {
                                System.out.println("Invoice Number: " + inv.getInvoiceNumber());
                            }
                        }
                    } else {
                        System.out.println("No products found.");
                    }
                    tx.commit();
                    break;
                case 9:
                    tx = entityManager.getTransaction();
                    tx.begin();

                    System.out.println("Product name you want to know category: ");
                    productName = scanner.nextLine();
                    Product specificProduct = entityManager.createQuery("FROM Product p where p.productName = :productName", Product.class).setParameter("productName", productName).getSingleResult();
                    if (specificProduct == null) {
                        System.out.println("Product not found");
                    }
                    Category category = specificProduct.getCategory();
                    if (category == null) {
                        System.out.println("Category not found");
                    } else {
                        System.out.println("Product Category: : " + category.getName());
                    }
                    tx.commit();
                    break;
                case 10:
                    entityManager.close();
                    scanner.close();
                    System.out.println("Goodbye!");
                    return;

                default:
                    System.out.println("Invalid option, please try again.");
            }
        }
    }
}
