

# Oracle PL/Sql

widoki, funkcje, procedury, triggery
ćwiczenie

---


Imiona i nazwiska autorów : Stas Kochevenko & Wiktor Dybalski

---

# Tabele

![](img/ora-trip1-0.png)


- `Trip`  - wycieczki
	- `trip_id` - identyfikator, klucz główny
	- `trip_name` - nazwa wycieczki
	- `country` - nazwa kraju
	- `trip_date` - data
	- `max_no_places` -  maksymalna liczba miejsc na wycieczkę
- `Person` - osoby
	- `person_id` - identyfikator, klucz główny
	- `firstname` - imię
	- `lastname` - nazwisko


- `Reservation`  - rezerwacje
	- `reservation_id` - identyfikator, klucz główny
	- `trip_id` - identyfikator wycieczki
	- `person_id` - identyfikator osoby
	- `status` - status rezerwacji
		- `N` – New - Nowa
		- `P` – Confirmed and Paid – Potwierdzona  i zapłacona
		- `C` – Canceled - Anulowana
- `Log` - dziennik zmian statusów rezerwacji 
	- `log_id` - identyfikator, klucz główny
	- `reservation_id` - identyfikator rezerwacji
	- `log_date` - data zmiany
	- `status` - status


```sql
create sequence s_person_seq  
   start with 1  
   increment by 1;

create table person  
(  
  person_id int not null
      constraint pk_person  
         primary key,
  firstname varchar(50),  
  lastname varchar(50)
)  

alter table person  
    modify person_id int default s_person_seq.nextval;
   
```


```sql
create sequence s_trip_seq  
   start with 1  
   increment by 1;

create table trip  
(  
  trip_id int  not null
     constraint pk_trip  
         primary key, 
  trip_name varchar(100),  
  country varchar(50),  
  trip_date date,  
  max_no_places int
);  

alter table trip 
    modify trip_id int default s_trip_seq.nextval;
```


```sql
create sequence s_reservation_seq  
   start with 1  
   increment by 1;

create table reservation  
(  
  reservation_id int not null
      constraint pk_reservation  
         primary key, 
  trip_id int,  
  person_id int,  
  status char(1)
);  

alter table reservation 
    modify reservation_id int default s_reservation_seq.nextval;


alter table reservation  
add constraint reservation_fk1 foreign key  
( person_id ) references person ( person_id ); 
  
alter table reservation  
add constraint reservation_fk2 foreign key  
( trip_id ) references trip ( trip_id );  
  
alter table reservation  
add constraint reservation_chk1 check  
(status in ('N','P','C'));

```


```sql
create sequence s_log_seq  
   start with 1  
   increment by 1;


create table log  
(  
    log_id int not null
         constraint pk_log  
         primary key,
    reservation_id int not null,  
    log_date date not null,  
    status char(1)
);  

alter table log 
    modify log_id int default s_log_seq.nextval;
  
alter table log  
add constraint log_chk1 check  
(status in ('N','P','C')) enable;
  
alter table log  
add constraint log_fk1 foreign key  
( reservation_id ) references reservation ( reservation_id );
```


---
# Dane


Należy wypełnić  tabele przykładowymi danymi 
- 4 wycieczki
- 10 osób
- 10  rezerwacji

Dane testowe powinny być różnorodne (wycieczki w przyszłości, wycieczki w przeszłości, rezerwacje o różnym statusie itp.) tak, żeby umożliwić testowanie napisanych procedur.

W razie potrzeby należy zmodyfikować dane tak żeby przetestować różne przypadki.


```sql
-- trip
insert into trip(trip_name, country, trip_date, max_no_places)  
values ('Wycieczka do Paryza', 'Francja', to_date('2023-09-12', 'YYYY-MM-DD'), 3);  
  
insert into trip(trip_name, country, trip_date,  max_no_places)  
values ('Piekny Krakow', 'Polska', to_date('2025-05-03','YYYY-MM-DD'), 2);  
  
insert into trip(trip_name, country, trip_date,  max_no_places)  
values ('Znow do Francji', 'Francja', to_date('2025-05-01','YYYY-MM-DD'), 2);  
  
insert into trip(trip_name, country, trip_date,  max_no_places)  
values ('Hel', 'Polska', to_date('2025-05-01','YYYY-MM-DD'),  2);

-- person
insert into person(firstname, lastname)  
values ('Jan', 'Nowak');  
  
insert into person(firstname, lastname)  
values ('Jan', 'Kowalski');  
  
insert into person(firstname, lastname)  
values ('Jan', 'Nowakowski');  
  
insert into person(firstname, lastname)  
values  ('Novak', 'Nowak');

-- reservation
-- trip1
insert  into reservation(trip_id, person_id, status)  
values (1, 1, 'P');  
  
insert into reservation(trip_id, person_id, status)  
values (1, 2, 'N');  
  
-- trip 2  
insert into reservation(trip_id, person_id, status)  
values (2, 1, 'P');  
  
insert into reservation(trip_id, person_id, status)  
values (2, 4, 'C');  
  
-- trip 3  
insert into reservation(trip_id, person_id, status)  
values (2, 4, 'P');
```

proszę pamiętać o zatwierdzeniu transakcji

---
# Zadanie 0 - modyfikacja danych, transakcje

Należy przeprowadzić kilka eksperymentów związanych ze wstawianiem, modyfikacją i usuwaniem danych
oraz wykorzystaniem transakcji

Skomentuj dzialanie transakcji. Jak działa polecenie `commit`, `rollback`?.
Co się dzieje w przypadku wystąpienia błędów podczas wykonywania transakcji? Porównaj sposób programowania operacji wykorzystujących transakcje w Oracle PL/SQL ze znanym ci systemem/językiem MS Sqlserver T-SQL

pomocne mogą być materiały dostępne tu:
https://upel.agh.edu.pl/mod/folder/view.php?id=214774
w szczególności dokument: `1_modyf.pdf`


```sql

insert into trip(trip_name, country, trip_date, max_no_places) values ('Wycieczka do Stolicy', 'Warszawa', to_date('2023-09-02', 'YYYY-MM-DD'), 5);
insert into trip(trip_name, country, trip_date, max_no_places) values ('Wycieczka do Stolicy', 'Warszawa', to_date('2024-12-02', 'YYYY-MM-DD'), 2);

INSERT INTO Person (firstname, lastname) VALUES ('Jan', 'Janowski');
INSERT INTO Person (firstname, lastname) VALUES ('Anna', 'Nowak');
INSERT INTO Person (firstname, lastname) VALUES ('Piotr', 'Wiśniewski');


Delete from PERSON where PERSON_ID > 20 and PERSON_ID < 28;
Delete from TRIP where TRIP.TRIP_ID > 20 and TRIP.TRIP_ID < 25;

SELECT * FROM TRIP;
SELECT * FROM RESERVATION;
SELECT * FROM PERSON;
SELECT * FROM LOG;

```
Możemy zauważyć, że w języku PL/SQL jeśli mamy np operacje dodawania 2 wierszy w jednym bloku "BEGIN-END", to w przypadku, gdy 2 polecenie kończy się błędem i nie obsługujemy ten błąd, żaden wiersz nie zostanie dodany, na zewnątrz zostanie wyrzucony wyjątek. 

Natomiast jeżelibyśmy wykonali to samo w języku Transact-SQL wewnątrz "BEGIN TRAN-COMMIT TRAN", pierwszy wiersz zostałby dodany do bazy, bo mimo że 2 polecenie kończy się błędem, to nie jest błąd krytyczny. Aby osiągnąć efekt, żeby cała sekwencja została wycofana w przypadku takiego błędu w języku Transact-SQL, możemy skorzystać z "TRY-CATCH", i w bloku "CATCH" wykonać "rollback tran".

W przypadku, gdy obsłużymy wyjątek spowodowany 2im poleceniem w języku PL/SQL i nie wyrzucimy "raise", to będziemy mieli 2 sytuacje: jeśli jesteśmy w trybie Auto-Commit, to ten poprawny wiersz zostanie dopisany do bazy, a jeśli jesteśmy w trybie Manual, to mamy jeszcze możliwość wykonania polecenia "rollback".

Oprócz tego, w języku PL/SQL nie mamy transakcji zagnieżdżonych.

---
# Zadanie 1 - widoki


Tworzenie widoków. Należy przygotować kilka widoków ułatwiających dostęp do danych. Należy zwrócić uwagę na strukturę kodu (należy unikać powielania kodu)

Widoki:
-   `vw_reservation`
	- widok łączy dane z tabel: `trip`,  `person`,  `reservation`
	- zwracane dane:  `reservation_id`,  `country`, `trip_date`, `trip_name`, `firstname`, `lastname`, `status`, `trip_id`, `person_id`
- `vw_trip` 
	- widok pokazuje liczbę wolnych miejsc na każdą wycieczkę
	- zwracane dane: `trip_id`, `country`, `trip_date`, `trip_name`, `max_no_places`, `no_available_places` (liczba wolnych miejsc)
-  `vw_available_trip`
	- podobnie jak w poprzednim punkcie, z tym że widok pokazuje jedynie dostępne wycieczki (takie które są w przyszłości i są na nie wolne miejsca)


Proponowany zestaw widoków można rozbudować wedle uznania/potrzeb
- np. można dodać nowe/pomocnicze widoki
- np. można zmienić def. widoków, dodając nowe/potrzebne pola

# Zadanie 1  - rozwiązanie

```sql
-- vw_reservation 
create view VW_RESERVATION as
SELECT  reservation_id, country, trip_date, trip_name, firstname, lastname, status, t.trip_id, p.person_id
FROM RESERVATION r
INNER JOIN person p on r.PERSON_ID = p.PERSON_ID
INNER JOIN trip t on r.TRIP_ID = t.TRIP_ID

-- vw_trip
create view VW_TRIP as
SELECT  t.trip_id, country, trip_date, trip_name, max_no_places,
        CASE
            WHEN (t.max_no_places - NVL(COUNT(r.trip_id), 0)) < 0 THEN 0
            ELSE (t.max_no_places - NVL(COUNT(r.trip_id), 0))
        END AS no_available_places
FROM TRIP t
LEFT JOIN RESERVATION r on t.TRIP_ID = r.TRIP_ID
GROUP BY t.trip_id, t.country, t.trip_date, t.trip_name, t.max_no_places

-- vw_available_trip
create view VW_AVAILABLE_TRIP as
SELECT t.trip_id, country, trip_date, trip_name, max_no_places,
        CASE
            WHEN (t.max_no_places - NVL(COUNT(r.trip_id), 0)) < 0 THEN 0
            ELSE (t.max_no_places - NVL(COUNT(r.trip_id), 0))
        END AS no_available_places
FROM TRIP t
LEFT JOIN RESERVATION r on t.TRIP_ID = r.TRIP_ID
GROUP BY t.trip_id, t.country, t.trip_date, t.trip_name, t.max_no_places
HAVING (t.max_no_places - NVL(COUNT(r.trip_id), 0)) > 0
```


---
# Zadanie 2  - funkcje


Tworzenie funkcji pobierających dane/tabele. Podobnie jak w poprzednim przykładzie należy przygotować kilka funkcji ułatwiających dostęp do danych

Funkcje:
- `f_trip_participants`
	- zadaniem funkcji jest zwrócenie listy uczestników wskazanej wycieczki
	- parametry funkcji: `trip_id`
	- funkcja zwraca podobny zestaw danych jak widok  `vw_reservation`
-  `f_person_reservations`
	- zadaniem funkcji jest zwrócenie listy rezerwacji danej osoby 
	- parametry funkcji: `person_id`
	- funkcja zwraca podobny zestaw danych jak widok `vw_reservation`
-  `f_available_trips_to`
	- zadaniem funkcji jest zwrócenie listy wycieczek do wskazanego kraju, dostępnych w zadanym okresie czasu (od `date_from` do `date_to`)
	- parametry funkcji: `country`, `date_from`, `date_to`


Funkcje powinny zwracać tabelę/zbiór wynikowy. Należy rozważyć dodanie kontroli parametrów, (np. jeśli parametrem jest `trip_id` to można sprawdzić czy taka wycieczka istnieje). Podobnie jak w przypadku widoków należy zwrócić uwagę na strukturę kodu

Czy kontrola parametrów w przypadku funkcji ma sens?
- jakie są zalety/wady takiego rozwiązania?

Proponowany zestaw funkcji można rozbudować wedle uznania/potrzeb
- np. można dodać nowe/pomocnicze funkcje/procedury

# Zadanie 2  - rozwiązanie

```sql
CREATE OR REPLACE TYPE ob_trip_participants AS OBJECT (
    reservation_id INT,
    country varchar(50),
    trip_date date,
    trip_name varchar(100),
    firstname VARCHAR2(50),
    lastname VARCHAR2(50),
    status CHAR(1),
    trip_id int,
    person_id int
);

create or replace type tab_trip_participants is table of ob_trip_participants;

create or replace function f_trip_participants(trip_id int)
    return tab_trip_participants
as
    result tab_trip_participants;
begin
    select ob_trip_participants(vw_r.RESERVATION_ID, vw_r.COUNTRY, vw_r.TRIP_DATE, vw_r.TRIP_NAME, vw_r.FIRSTNAME, vw_r.LASTNAME, vw_r.STATUS, vw_r.TRIP_ID, vw_r.PERSON_ID)
    bulk collect
    into result
    from vw_reservation vw_r
    where vw_r.TRIP_ID = f_trip_participants.trip_id;

    return result;
end;

-----------
select * from VW_RESERVATION where VW_RESERVATION.TRIP_ID = 2;
select * from f_trip_participants(2)

/*f_available_trips_to
zadaniem funkcji jest zwrócenie listy wycieczek do wskazanego kraju, dostępnych w zadanym okresie czasu (od date_from do date_to)
parametry funkcji: country, date_from, date_to*/

select * from VW_TRIP;
select * from VW_AVAILABLE_TRIP;

CREATE OR REPLACE TYPE ob_trip AS OBJECT (
    trip_id int,
    country varchar(50),
    trip_date date,
    trip_name varchar(100),
    max_no_places int,
    no_available_places int
);

create or replace type tab_trip is table of ob_trip;

create or replace function f_available_trips_to(country varchar(50), date_from date, date_to date)
    return tab_trip
as
    result tab_trip;
begin
    select ob_trip(vw_av.TRIP_ID, vw_av.COUNTRY, vw_av.TRIP_DATE, vw_av.TRIP_NAME, vw_av.MAX_NO_PLACES, vw_av.NO_AVAILABLE_PLACES)
    bulk collect
    into result
    from vw_available_trip vw_av
    where vw_av.COUNTRY = f_available_trips_to.country and vw_av.TRIP_DATE between date_from and date_to;

    return result;
end;
-------------
select * from VW_AVAILABLE_TRIP vw_av where vw_av.COUNTRY = 'Francja' and vw_av.TRIP_DATE between TO_DATE('2023-01-01', 'YYYY-MM-DD') AND TO_DATE('2023-12-31', 'YYYY-MM-DD');
select * from f_available_trips_to('Francja', '2023-01-01',  '2023-12-31');
-------------

-- wyniki, kod, zrzuty ekranów, komentarz ...

```


---
# Zadanie 3  - procedury


Tworzenie procedur modyfikujących dane. Należy przygotować zestaw procedur pozwalających na modyfikację danych oraz kontrolę poprawności ich wprowadzania

Procedury
- `p_add_reservation`
	- zadaniem procedury jest dopisanie nowej rezerwacji
	- parametry: `trip_id`, `person_id`, 
	- procedura powinna kontrolować czy wycieczka jeszcze się nie odbyła, i czy sa wolne miejsca
	- procedura powinna również dopisywać inf. do tabeli `log`
- `p_modify_reservation_tatus`
	- zadaniem procedury jest zmiana statusu rezerwacji 
	- parametry: `reservation_id`, `status` 
	- procedura powinna kontrolować czy możliwa jest zmiana statusu, np. zmiana statusu już anulowanej wycieczki (przywrócenie do stanu aktywnego nie zawsze jest możliwa – może już nie być miejsc)
	- procedura powinna również dopisywać inf. do tabeli `log`


Procedury:
- `p_modify_max_no_places`
	- zadaniem procedury jest zmiana maksymalnej liczby miejsc na daną wycieczkę 
	- parametry: `trip_id`, `max_no_places`
	- nie wszystkie zmiany liczby miejsc są dozwolone, nie można zmniejszyć liczby miejsc na wartość poniżej liczby zarezerwowanych miejsc

Należy rozważyć użycie transakcji

Należy zwrócić uwagę na kontrolę parametrów (np. jeśli parametrem jest trip_id to należy sprawdzić czy taka wycieczka istnieje, jeśli robimy rezerwację to należy sprawdzać czy są wolne miejsca itp..)


Proponowany zestaw procedur można rozbudować wedle uznania/potrzeb
- np. można dodać nowe/pomocnicze funkcje/procedury

# Zadanie 3  - rozwiązanie

```sql
-- p_add_reservation
CREATE OR REPLACE PROCEDURE p_add_reservation(
    p_trip_id IN NUMBER,
    p_person_id IN NUMBER
)
IS
    v_trip_date DATE;
    v_available_seats NUMBER;
    v_reservation_id NUMBER;
BEGIN
--  Validating
    SELECT TRIP_DATE INTO v_trip_date from TRIP
    WHERE TRIP_ID = p_trip_id;

    IF v_trip_date IS NULL THEN
        RAISE_APPLICATION_ERROR(-20001, 'The specified trip does not exist');
    END IF;

    IF v_trip_date <= SYSDATE THEN
        RAISE_APPLICATION_ERROR(-20002, 'The trip has already taken place! :(');
    END IF;

    SELECT NO_AVAILABLE_PLACES INTO v_available_seats FROM VW_TRIP
    WHERE TRIP_ID = p_trip_id;

    IF v_available_seats <= 0 THEN
        RAISE_APPLICATION_ERROR(-20003, 'There are no available places on this trip.! :(');
    END IF;

--  Add reservation
    INSERT INTO RESERVATION (TRIP_ID, PERSON_ID, STATUS)
    VALUES (p_trip_id, p_person_id, 'N')
    RETURNING RESERVATION_ID INTO v_reservation_id;

    IF v_reservation_id IS NULL THEN
        RAISE_APPLICATION_ERROR(-20004, 'Failed to get reservation_id! :(');
    END IF;

    INSERT INTO LOG (RESERVATION_ID, LOG_DATE, STATUS)
    VALUES  (v_reservation_id, SYSDATE, 'N');

    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Reservation add successfully!');
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('ERROR: ' || SQLERRM);
END;



-- p_modify_reservation_status
CREATE OR REPLACE PROCEDURE p_modify_reservation_status (
    p_reservation_id IN NUMBER,
    p_status IN CHAR
)
IS
    v_current_status CHAR;
    v_trip_date DATE;
    v_available_seats NUMBER;
BEGIN
    SELECT STATUS INTO v_current_status FROM RESERVATION
    WHERE RESERVATION_ID = p_reservation_id;

    IF v_current_status IS NULL THEN
        RAISE_APPLICATION_ERROR(-20005, 'The specified reservation does not exist! :(');
    END IF;

    IF p_status = 'C' THEN
        SELECT TRIP_DATE INTO v_trip_date FROM TRIP
        WHERE TRIP_ID = (SELECT TRIP_ID FROM RESERVATION WHERE RESERVATION_ID = p_reservation_id);

        IF v_trip_date <= SYSDATE THEN
            RAISE_APPLICATION_ERROR(-20006, 'Cannot cancel reservation for past trip! :(');
        END IF;
    END IF;
    IF p_status = 'N' AND v_current_status = 'C' THEN
        SELECT NO_AVAILABLE_PLACES INTO v_available_seats FROM VW_TRIP
        WHERE TRIP_ID = (SELECT TRIP_ID FROM RESERVATION WHERE RESERVATION_ID = p_reservation_id);

        IF v_available_seats IS NULL OR v_available_seats <= 0 THEN
            RAISE_APPLICATION_ERROR(-20007, 'Cannot create reservation, no available places on trip! :(');
        END IF;
    END IF;

    UPDATE RESERVATION SET STATUS = p_status
    WHERE RESERVATION_ID = p_reservation_id;

    INSERT INTO LOG (RESERVATION_ID, LOG_DATE, STATUS)
    VALUES  (p_reservation_id, SYSDATE, p_status);

    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Reservation status modified successfully!');
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('ERROR: ' || SQLERRM);
END;



-- p_modify_max_no_places
CREATE OR REPLACE PROCEDURE p_modify_max_no_places (
    p_trip_id IN NUMBER,
    p_max_no_places IN NUMBER
)
IS
    v_current_max_no_places NUMBER;
    v_no_available NUMBER;
BEGIN

    SELECT MAX_NO_PLACES INTO v_current_max_no_places FROM TRIP
    WHERE TRIP_ID = p_trip_id;

    if v_current_max_no_places IS NULL THEN
        RAISE_APPLICATION_ERROR(-20006, 'The specified trip does not exist!');
    END IF;

    if v_current_max_no_places <= 0 THEN
        RAISE_APPLICATION_ERROR(-20007, 'This number is smaller than 0!');
    END IF;

    SELECT NO_AVAILABLE_PLACES INTO v_no_available FROM VW_TRIP
    WHERE TRIP_ID = p_trip_id;

    if p_max_no_places < v_current_max_no_places - v_no_available THEN
        RAISE_APPLICATION_ERROR(-20008, 'This number is smaller than the number of reserved seats!');
    END IF;

    UPDATE TRIP SET MAX_NO_PLACES = p_max_no_places
    WHERE TRIP_ID = p_trip_id;

    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Max number of places modified successfully!');
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('ERROR: ' || SQLERRM);
END;

```


---
# Zadanie 4  - triggery


Zmiana strategii zapisywania do dziennika rezerwacji. Realizacja przy pomocy triggerów

Należy wprowadzić zmianę, która spowoduje, że zapis do dziennika rezerwacji będzie realizowany przy pomocy trierów

Triggery:
- trigger/triggery obsługujące 
	- dodanie rezerwacji
	- zmianę statusu
- trigger zabraniający usunięcia rezerwacji

Oczywiście po wprowadzeniu tej zmiany należy "uaktualnić" procedury modyfikujące dane. 

>UWAGA
Należy stworzyć nowe wersje tych procedur (dodając do nazwy dopisek 4 - od numeru zadania). Poprzednie wersje procedur należy pozostawić w celu  umożliwienia weryfikacji ich poprawności

Należy przygotować procedury: `p_add_reservation_4`, `p_modify_reservation_status_4` 


# Zadanie 4  - rozwiązanie

```sql

-- Add reservation Trigger 
CREATE OR REPLACE TRIGGER trg_insert_log_reservation
AFTER INSERT ON RESERVATION
FOR EACH ROW
DECLARE
    v_reservation_id NUMBER := :new.reservation_id;
BEGIN
    INSERT INTO LOG (RESERVATION_ID, LOG_DATE, STATUS)
    VALUES (v_reservation_id, SYSDATE, 'N');
END;

-- p_add_reservation_4
CREATE OR REPLACE PROCEDURE p_add_reservation_4(
    p_trip_id IN NUMBER,
    p_person_id IN NUMBER
)
IS
    v_trip_date DATE;
    v_available_seats NUMBER;
    v_reservation_id NUMBER;
BEGIN
--  Validating
    SELECT TRIP_DATE INTO v_trip_date from TRIP
    WHERE TRIP_ID = p_trip_id;

    IF v_trip_date IS NULL THEN
        RAISE_APPLICATION_ERROR(-20001, 'The specified trip does not exist');
    END IF;

    IF v_trip_date <= SYSDATE THEN
        RAISE_APPLICATION_ERROR(-20002, 'The trip has already taken place! :(');
    END IF;

    SELECT NO_AVAILABLE_PLACES INTO v_available_seats FROM VW_TRIP
    WHERE TRIP_ID = p_trip_id;

    IF v_available_seats <= 0 THEN
        RAISE_APPLICATION_ERROR(-20003, 'There are no available places on this trip.! :(');
    END IF;

--  Add reservation
    INSERT INTO RESERVATION (TRIP_ID, PERSON_ID, STATUS)
    VALUES (p_trip_id, p_person_id, 'N')
    RETURNING RESERVATION_ID INTO v_reservation_id;

    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Reservation add successfully!');
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('ERROR: ' || SQLERRM);
END;




-- Change status Trigger
CREATE OR REPLACE TRIGGER trg_modify_reservation_status
AFTER UPDATE OF status ON RESERVATION
FOR EACH ROW
DECLARE
    v_log_status CHAR;
BEGIN
    CASE
        WHEN :new.STATUS = 'N' THEN
            v_log_status := 'N';
        WHEN :new.STATUS = 'P' THEN
            v_log_status := 'P';
        WHEN :new.STATUS = 'C' THEN
            v_log_status := 'C';
        ELSE
            RAISE_APPLICATION_ERROR(-20009, 'ERROR');
    END CASE;

    INSERT INTO LOG (RESERVATION_ID, LOG_DATE, STATUS)
    VALUES (:new.RESERVATION_ID, SYSDATE, v_log_status);
END;

-- p_modify_reservation_status_4
CREATE OR REPLACE PROCEDURE p_modify_reservation_status_4 (
    p_reservation_id IN NUMBER,
    p_status IN CHAR
)
IS
    v_current_status CHAR;
    v_trip_date DATE;
    v_available_seats NUMBER;
BEGIN
    SELECT STATUS INTO v_current_status FROM RESERVATION
    WHERE RESERVATION_ID = p_reservation_id;

    IF v_current_status IS NULL THEN
        RAISE_APPLICATION_ERROR(-20005, 'The specified reservation does not exist! :(');
    END IF;

    IF p_status = 'C' THEN
        SELECT TRIP_DATE INTO v_trip_date FROM TRIP
        WHERE TRIP_ID = (SELECT TRIP_ID FROM RESERVATION WHERE RESERVATION_ID = p_reservation_id);

        IF v_trip_date <= SYSDATE THEN
            RAISE_APPLICATION_ERROR(-20006, 'Cannot cancel reservation for past trip! :(');
        END IF;
    END IF;
    IF p_status = 'N' AND v_current_status = 'C' THEN
        SELECT NO_AVAILABLE_PLACES INTO v_available_seats FROM VW_TRIP
        WHERE TRIP_ID = (SELECT TRIP_ID FROM RESERVATION WHERE RESERVATION_ID = p_reservation_id);

        IF v_available_seats IS NULL OR v_available_seats <= 0 THEN
            RAISE_APPLICATION_ERROR(-20007, 'Cannot create reservation, no available places on trip! :(');
        END IF;
    END IF;

    UPDATE RESERVATION SET STATUS = p_status
    WHERE RESERVATION_ID = p_reservation_id;

    COMMIT;
    DBMS_OUTPUT.PUT_LINE('Reservation status modified successfully!');
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('ERROR: ' || SQLERRM);
END;

```



---
# Zadanie 5  - triggery


Zmiana strategii kontroli dostępności miejsc. Realizacja przy pomocy triggerów

Należy wprowadzić zmianę, która spowoduje, że kontrola dostępności miejsc na wycieczki (przy dodawaniu nowej rezerwacji, zmianie statusu) będzie realizowana przy pomocy trierów

Triggery:
- Trigger/triggery obsługujące: 
	- dodanie rezerwacji
	- zmianę statusu

Oczywiście po wprowadzeniu tej zmiany należy "uaktualnić" procedury modyfikujące dane. 

>UWAGA
Należy stworzyć nowe wersje tych procedur (np. dodając do nazwy dopisek 5 - od numeru zadania). Poprzednie wersje procedur należy pozostawić w celu  umożliwienia weryfikacji ich poprawności. 

Należy przygotować procedury: `p_add_reservation_5`, `p_modify_reservation_status_5`


# Zadanie 5  - rozwiązanie

```sql

-- wyniki, kod, zrzuty ekranów, komentarz ...

```

---
# Zadanie 6


Zmiana struktury bazy danych. W tabeli `trip`  należy dodać  redundantne pole `no_available_places`.  Dodanie redundantnego pola uprości kontrolę dostępnych miejsc, ale nieco skomplikuje procedury dodawania rezerwacji, zmiany statusu czy też zmiany maksymalnej liczby miejsc na wycieczki.

Należy przygotować polecenie/procedurę przeliczającą wartość pola `no_available_places` dla wszystkich wycieczek (do jednorazowego wykonania)

Obsługę pola `no_available_places` można zrealizować przy pomocy procedur lub triggerów

Należy zwrócić uwagę na spójność rozwiązania.

>UWAGA
Należy stworzyć nowe wersje tych widoków/procedur/triggerów (np. dodając do nazwy dopisek 6 - od numeru zadania). Poprzednie wersje procedur należy pozostawić w celu  umożliwienia weryfikacji ich poprawności. 


- zmiana struktury tabeli

```sql
alter table trip add  
    no_available_places int null
```

- polecenie przeliczające wartość `no_available_places`
	- należy wykonać operację "przeliczenia"  liczby wolnych miejsc i aktualizacji pola  `no_available_places`

# Zadanie 6  - rozwiązanie

```sql

-- wyniki, kod, zrzuty ekranów, komentarz ...

```



---
# Zadanie 6a  - procedury



Obsługę pola `no_available_places` należy zrealizować przy pomocy procedur
- procedura dodająca rezerwację powinna aktualizować pole `no_available_places` w tabeli trip
- podobnie procedury odpowiedzialne za zmianę statusu oraz zmianę maksymalnej liczby miejsc na wycieczkę
- należy przygotować procedury oraz jeśli jest to potrzebne, zaktualizować triggery oraz widoki



>UWAGA
Należy stworzyć nowe wersje tych widoków/procedur/triggerów (np. dodając do nazwy dopisek 6a - od numeru zadania). Poprzednie wersje procedur należy pozostawić w celu  umożliwienia weryfikacji ich poprawności. 
- może  być potrzebne wyłączenie 'poprzednich wersji' triggerów 


# Zadanie 6a  - rozwiązanie

```sql

-- wyniki, kod, zrzuty ekranów, komentarz ...

```



---
# Zadanie 6b -  triggery


Obsługę pola `no_available_places` należy zrealizować przy pomocy triggerów
- podczas dodawania rezerwacji trigger powinien aktualizować pole `no_available_places` w tabeli trip
- podobnie, podczas zmiany statusu rezerwacji
- należy przygotować trigger/triggery oraz jeśli jest to potrzebne, zaktualizować procedury modyfikujące dane oraz widoki


>UWAGA
Należy stworzyć nowe wersje tych widoków/procedur/triggerów (np. dodając do nazwy dopisek 6b - od numeru zadania). Poprzednie wersje procedur należy pozostawić w celu  umożliwienia weryfikacji ich poprawności. 
- może  być potrzebne wyłączenie 'poprzednich wersji' triggerów 



# Zadanie 6b  - rozwiązanie


```sql

-- wyniki, kod, zrzuty ekranów, komentarz ...

```


# Zadanie 7 - podsumowanie

Porównaj sposób programowania w systemie Oracle PL/SQL ze znanym ci systemem/językiem MS Sqlserver T-SQL

```sql

-- komentarz ...

```