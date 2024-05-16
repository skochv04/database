package zad9;

import jakarta.persistence.*;

@Entity
public class Supplier extends Company {
    private String bankAccountNumber;
    public Supplier() {}
    public Supplier(String companyName, String street, String city, String
            zipCode, String bankAccountNumber) {
        super(companyName, street, city, zipCode);
        this.bankAccountNumber = bankAccountNumber;
    }
}
