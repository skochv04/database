package zad9;

import jakarta.persistence.EntityManager;
import jakarta.persistence.EntityManagerFactory;
import jakarta.persistence.EntityTransaction;
import jakarta.persistence.Persistence;
import org.hibernate.Transaction;
import org.hibernate.cfg.Configuration;

import java.util.List;
import java.util.Scanner;
import java.util.Set;


class Main {
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
        EntityManager em = getEntityManager();
        EntityTransaction etx = em.getTransaction();

        etx.begin();
        Customer customer1 = new Customer("Client #1", "Short street", "Zlin",
                "27-001", 0.0);
        Customer customer2 = new Customer("Client #2", "29 listopada", "Gdynia", "35-693",
                2.0);
        Supplier supplier1 = new Supplier("Supplier #1", "Rondo Kosmiczne",
                "Warsaw", "40-367", "687453214569874536541236");
        Supplier supplier2 = new Supplier("Supplier #2", "Oil street", "Kyiv", "10-234",
                "98745632564632158963456");
        em.persist(customer1);
        em.persist(customer2);
        em.persist(supplier1);
        em.persist(supplier2);
        etx.commit();
        em.close();
    }
}
