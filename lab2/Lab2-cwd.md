# Dokumentowe bazy danych – MongoDB

ćwiczenie 2


---

**Imiona i nazwiska autorów: Stas Kochevenko & Wiktor Dybalski**

--- 


## Yelp Dataset

- [www.yelp.com](http://www.yelp.com) - serwis społecznościowy – informacje o miejscach/lokalach
- restauracje, kluby, hotele itd. `businesses`,
- użytkownicy odwiedzają te miejsca - "meldują się"  `check-in`
- użytkownicy piszą recenzje `reviews` o miejscach/lokalach i wystawiają oceny oceny,
- przykładowy zbiór danych zawiera dane z 5 miast: Phoenix, Las Vegas, Madison, Waterloo i Edinburgh.

# Zadanie 1 - operacje wyszukiwania danych

Dla zbioru Yelp wykonaj następujące zapytania

W niektórych przypadkach może być potrzebne wykorzystanie mechanizmu Aggregation Pipeline

[https://www.mongodb.com/docs/manual/core/aggregation-pipeline/](https://www.mongodb.com/docs/manual/core/aggregation-pipeline/)


1. Zwróć dane wszystkich restauracji (kolekcja `business`, pole `categories` musi zawierać wartość "Restaurants"), które są otwarte w poniedziałki (pole hours) i mają ocenę co najmniej 4 gwiazdki (pole `stars`).  Zapytanie powinno zwracać: nazwę firmy, adres, kategorię, godziny otwarcia i gwiazdki. Posortuj wynik wg nazwy firmy.

2. Ile każda firma otrzymała ocen/wskazówek (kolekcja `tip` ) w 2012. Wynik powinien zawierać nazwę firmy oraz liczbę ocen/wskazówek Wynik posortuj według liczby ocen (`tip`).

3. Recenzje mogą być oceniane przez innych użytkowników jako `cool`, `funny` lub `useful` (kolekcja `review`, pole `votes`, jedna recenzja może mieć kilka głosów w każdej kategorii).  Napisz zapytanie, które zwraca dla każdej z tych kategorii, ile sumarycznie recenzji zostało oznaczonych przez te kategorie (np. recenzja ma kategorię `funny` jeśli co najmniej jedna osoba zagłosowała w ten sposób na daną recenzję)

4. Zwróć dane wszystkich użytkowników (kolekcja `user`), którzy nie mają ani jednego pozytywnego głosu (pole `votes`) z kategorii (`funny` lub `useful`), wynik posortuj alfabetycznie według nazwy użytkownika.

5. Wyznacz, jaką średnia ocenę uzyskała każda firma na podstawie wszystkich recenzji (kolekcja `review`, pole `stars`). Ogranicz do firm, które uzyskały średnią powyżej 3 gwiazdek.

	a) Wynik powinien zawierać id firmy oraz średnią ocenę. Posortuj wynik wg id firmy.

	b) Wynik powinien zawierać nazwę firmy oraz średnią ocenę. Posortuj wynik wg nazwy firmy.

## Zadanie 1  - rozwiązanie

1. Zwróć dane wszystkich restauracji (kolekcja `business`, pole `categories` musi zawierać wartość "Restaurants"), które są otwarte w poniedziałki (pole hours) i mają ocenę co najmniej 4 gwiazdki (pole `stars`).  Zapytanie powinno zwracać: nazwę firmy, adres, kategorię, godziny otwarcia i gwiazdki. Posortuj wynik wg nazwy firmy.

```js
db.business.find({categories: "Restaurants", "hours.Monday.open": {$exists: true}, stars: {$gte: 4}},
 {"_id": 0, "name": 1, "full_address": 1, "categories": 1, "hours": 1, "stars": 1}).sort({"name": 1})
```

Wynik zapytania:

![](img/business_query.png)


2. Ile każda firma otrzymała ocen/wskazówek (kolekcja `tip` ) w 2012. Wynik powinien zawierać nazwę firmy oraz liczbę ocen/wskazówek Wynik posortuj według liczby ocen (`tip`).

```js
db.tip.aggregate([
    {$match: {date: {$gte: "2012-01-01", $lte: "2012-12-31"}}},
    {$group: {_id: "$business_id", totalTips: {$sum: 1}}},
    {$lookup: {
        from: "business",
        localField: "_id",
        foreignField: "business_id",
        as: "business-info"
        }
    },
    {$unwind: "$business-info"},
    {$sort: {totalTips: -1}},
    {$project: {_id: 0, name: "$business_info.name", totalTips: 1}}
                    ])
```

Wynik zapytania:

Nie udało nam się zarejestrować wyniku zapytania z powodu zbyt długiego czasu wykonywania podzapytania.
Zatem użyliśmy prostzego zapytania bez kosztownego łączenia kolekcji:

```js
db.tip.aggregate([
    {$match: {date: {$gte: "2012-01-01", $lte: "2012-12-31"}}},
    {$group: {_id: "$business_id", totalTips: {$sum: 1}}},
    {$sort: {totalTips: -1}}])
```

Wynik zapytania:

![](img/tipy.png)


3. Recenzje mogą być oceniane przez innych użytkowników jako `cool`, `funny` lub `useful` (kolekcja `review`, pole `votes`, jedna recenzja może mieć kilka głosów w każdej kategorii).  Napisz zapytanie, które zwraca dla każdej z tych kategorii, ile sumarycznie recenzji zostało oznaczonych przez te kategorie (np. recenzja ma kategorię `funny` jeśli co najmniej jedna osoba zagłosowała w ten sposób na daną recenzję).

```js
db.review1.aggregate([
    {$group: {
    _id: "Number_of_categories",
    totalFunny: {$sum: {
        $cond: {if: {$gt: ["$votes.funny", 0]},
                          then: 1, else: 0}}},
    totalUseful: {$sum: {
        $cond: {if: {$gt: ["$votes.useful", 0]},
                          then: 1, else: 0}}},
    totalCool: {$sum: {
        $cond: {if: {$gt: ["$votes.cool", 0]},
                          then: 1, else: 0}}}
    }}
])
```

Wynik zapytania:

![](img/number_of_categories.png)

<div style="page-break-after: always;"></div>

4. Zwróć dane wszystkich użytkowników (kolekcja `user`), którzy nie mają ani jednego pozytywnego głosu (pole `votes`) z kategorii (`funny` lub `useful`), wynik posortuj alfabetycznie według nazwy użytkownika.

```js
db.user.aggregate([
    {
        $group: {
            _id: "$name",
            totalFunny: {$sum: {$cond: {if: {$gt: ["$votes.funny", 0]}, then: "$votes.funny", else: 0}}},
            totalUseful: {$sum: {$cond: {if: {$gt: ["$votes.useful", 0]}, then: "$votes.useful", else: 0}}}
        }
    },
    {
        $match: {
            totalFunny: 0,
            totalUseful: 0
        }
    },
    {
        $project: {
            _id: 0,
            name: "$_id",
            totalFunny: 1,
            totalUseful: 1
        }
    },
    {
        $sort: {name: 1}
    }
])
```

Wynik zapytania:

![](img/users_result.png)

<div style="page-break-after: always;"></div>

5. Wyznacz, jaką średnia ocenę uzyskała każda firma na podstawie wszystkich recenzji (kolekcja `review`, pole `stars`). Ogranicz do firm, które uzyskały średnią powyżej 3 gwiazdek.

a) Wynik powinien zawierać id firmy oraz średnią ocenę. Posortuj wynik wg id firmy.

```js
db.review1.aggregate([
    {$group: {
        _id: "$business_id",
        avg_stars: {$avg: "$stars"}
    }},
    {$match: {
        "avg_stars": {$gt: 3}
    }},
    {$sort: {
        _id: 1
    }}
])
```

Wynika zapytania A:

![](img/businessA.png)

5. Wyznacz, jaką średnia ocenę uzyskała każda firma na podstawie wszystkich recenzji (kolekcja `review`, pole `stars`). Ogranicz do firm, które uzyskały średnią powyżej 3 gwiazdek.

b) Wynik powinien zawierać nazwę firmy oraz średnią ocenę. Posortuj wynik wg nazwy firmy.

```js
db.review1.aggregate([
    {$group: {
        _id: "$business_id",
        avg_stars: {$avg: "$stars"}
        }
    },
    {$lookup: {
        from: "business",
        localField: "_id",
        foreignField: "business_id",
        as: "business-info"
        }
    },
    {$unwind: "$business-info"},
    {$project: {
            _id: 0,
            business_name: "$business-info.name",
            avg_stars: 1
        }
    },
    {$sort:
        {business_name: 1}
        }
])
```

Wynik zapytania B:

![](img/businessB.png)

<div style="page-break-after: always;"></div>

# Zadanie 2 - modelowanie danych


Zaproponuj strukturę bazy danych dla wybranego/przykładowego zagadnienia/problemu

Należy wybrać jedno zagadnienie/problem (A lub B)

Przykład A
- Wykładowcy, przedmioty, studenci, oceny
	- Wykładowcy prowadzą zajęcia z poszczególnych przedmiotów
	- Studenci uczęszczają na zajęcia
	- Wykładowcy wystawiają oceny studentom
	- Studenci oceniają zajęcia

Przykład B
- Firmy, wycieczki, osoby
	- Firmy organizują wycieczki
	- Osoby rezerwują miejsca/wykupują bilety
	- Osoby oceniają wycieczki

a) Warto zaproponować/rozważyć różne warianty struktury bazy danych i dokumentów w poszczególnych kolekcjach oraz przeprowadzić dyskusję każdego wariantu (wskazać wady i zalety każdego z wariantów)

b) Kolekcje należy wypełnić przykładowymi danymi

c) W kontekście zaprezentowania wad/zalet należy zaprezentować kilka przykładów/zapytań/zadań/operacji oraz dla których dedykowany jest dany wariant

W sprawozdaniu należy zamieścić przykładowe dokumenty w formacie JSON ( pkt a) i b)), oraz kod zapytań/operacji (pkt c)), wraz z odpowiednim komentarzem opisującym strukturę dokumentów oraz polecenia ilustrujące wykonanie przykładowych operacji na danych

Do sprawozdania należy kompletny zrzut wykonanych/przygotowanych baz danych (taki zrzut można wykonać np. za pomocą poleceń `mongoexport`, `mongdump` …) oraz plik z kodem operacji zapytań (załącznik powinien mieć format zip).


## Zadanie 2  - rozwiązanie
### Podejście tabelaryczne

#### a)  Analiza danego wariantu

Podejście tabelaryczne jest znane z modelu relacyjnego: uporządkujemy dane w taki sposób, że każda tabela/kolekcja reprezentuje pewien rodzaj encji, np. Klienci, Nauczyciele, Przedmioty. Encje są powiązane między sobą przez referencje.

- Zalety
    - Bardziej przejrzysty model danych: dane są pogrupowane w taki sposób, że nie ma potrzeby nadużywania dokumentów zagnieżdżonych
    - Brak redundancji danych (oprócz sytuacji, gdzie "rejestrujemy" referencje w obu encjach relacji (w MongoDB), np. Product ma informacje o swoim Dostawce, a Dostawca ma informacje o swoich produktach)
    - Szybka modyfikacji danych: dzięki temu, że nie ma redundancji, potrzebujemy modyfikować dane tylko w jednym miejscu
    - Zapewniona spójność danych: modyfikacja danych tylko w jednym miejscu zapewnia, że dane będą spójne
    - Większa wydajność w sytuacji, gdy potrzebujemy danych dotyczących konkretnego małego dokumentu, a nie całego dokumentu wraz z zagnieżdżonymi dokumentami

- Wady
    - Konieczność używania operacji łączenia dokumentów różnych kolekcji w sytuacji, gdy potrzebujemy dokument wraz z dokumentami, które znajdują się z nim w relacji
    - Konieczność użycia złożonych zapytań (w MongoDB) do łączenia wielu dokumentów z różnych kolekcji
    - W przypadku wielokrotnego odczytywania danych z powiązanych między sobą dokumentów różnych kolekcji zmniejsza się wydajność, ponieważ za każdym razem potrzebujemy ponownie łączyć dane

<div style="page-break-after: always;"></div>

#### b)  Utworzenie kolekcji i wypełnienie kolekcji przykładowymi danymi

Przykładowa struktura bazy danych wygląda następująco:

```js
Companies
{
	"_id": Number,
	"organized_trips_id": [Number], 
	"company_name": String
}

Customers
{
	"_id": Number,
	"owned_tickets_id": [Number],  
	"customer_review_id": [Number],
	"first_name": String,
	"last_name": String, 
}

Trips 
{
	"_id": Number,  
	"company_id": [Number], 
	"tickets_id": [Number], 
	"date": Date, 
	"location": String, 
	"trip_review_id": [Number],
    "took_place": Boolean
}

Tickets 
{
	"_id": Number,  
	"trip_id": [Number], 
	"reserved_seat_no": Number, 
    "ticket_status": String, 
    "first_name": String,
	"last_name": String 
}

Reviews
{
	"_id": Number, 
	"customer_id": Number,
	"trip_id": Number, 
	"company_id": Number, 
	"review_date": Date, 
	"stars": Number, 
	"review_description": String
}
```

Stworzenie bazy danych według wyżej przedstawionego pomysłu:

```js
use tab_trip_database

db.createCollection("Companies")
db.createCollection("Customers")
db.createCollection("Trips")
db.createCollection("Tickets")
db.createCollection("Reviews")
```

<div style="page-break-after: always;"></div>

Stworzenie rożnego rodzaju danych dla przykładowej bazy danych:

Kilka przydatnych informacji:
    
- took_place ustawione na True w kolekcji Trips oznacza, że wycieczka się już odbyła,
- review_id oraz ticket_id to liczby 3-cyfrowe zaczynające się od trip_id. 3 bilety dla np. trip_id=1 to: 101,102,103,
- seats_no to liczby odpowiadające numerom ticketów: np dla ticketa 102, seats_no to 2 itd
- ticket_status to jeden symbol spośród 'N', 'P' i 'C', oznaczających kolejno "New", "Paid" i "Canceled"

```js
db.Companies.insertMany([
  { "_id": 1, "organized_trips_id": [1, 2], "company_name": "Adventure Works" },
  { "_id": 2, "organized_trips_id": [3, 4], "company_name": "Travel Corp" },
  { "_id": 3, "organized_trips_id": [5], "company_name": "Holiday Makers" },
  { "_id": 4, "organized_trips_id": [6, 7], "company_name": "Globe Trotters" },
  { "_id": 5, "organized_trips_id": [8], "company_name": "Pathfinders" }
])

db.Customers.insertMany([
  { "_id": 1, "owned_tickets_id": [101, 504], "customer_review_id": [101], "first_name": "John", "last_name": "Doe" },
  { "_id": 2, "owned_tickets_id": [201], "customer_review_id": [201], "first_name": "Anna", "last_name": "Smith" },
  { "_id": 3, "owned_tickets_id": [102, 301], "customer_review_id": [102, 301], "first_name": "David", "last_name": "Brown" },
  { "_id": 4, "owned_tickets_id": [501, 502], "customer_review_id": [], "first_name": "Lisa", "last_name": "White" },
  { "_id": 5, "owned_tickets_id": [503], "customer_review_id": [], "first_name": "Mark", "last_name": "Taylor" }
])

db.Trips.insertMany([
  { "_id": 1, "company_id": 1, "tickets_id": [101, 102], "date": {"$date": "2024-04-15T00:00:00Z"}, "location": "Warsaw", "trip_review_id": [101, 102], "took_place": true },
  { "_id": 2, "company_id": 1, "tickets_id": [201], "date": {"$date": "2024-05-20T00:00:00Z"}, "location": "Krakow", "trip_review_id": [201], "took_place": true },
  { "_id": 3, "company_id": 2, "tickets_id": [301], "date": {"$date": "2024-06-10T00:00:00Z"}, "location": "Gdansk", "trip_review_id": [301], "took_place": true },
  { "_id": 4, "company_id": 2, "tickets_id": [], "date": {"$date": "2024-07-15T00:00:00Z"}, "location": "Poznan", "trip_review_id": [], "took_place": false },
  { "_id": 5, "company_id": 3, "tickets_id": [501, 502, 503, 504], "date": {"$date": "2024-08-25T00:00:00Z"}, "location": "Wroclaw", "trip_review_id": [], "took_place": false },
  { "_id": 6, "company_id": 4, "tickets_id": [], "date": {"$date": "2024-09-14T00:00:00Z"}, "location": "Sopot", "trip_review_id": [], "took_place": false },
  { "_id": 7, "company_id": 4, "tickets_id": [], "date": {"$date": "2024-09-15T00:00:00Z"}, "location": "Gdynia", "trip_review_id": [], "took_place": false },
  { "_id": 8, "company_id": 5, "tickets_id": [], "date": {"$date": "2024-09-29T00:00:00Z"}, "location": "Katowice", "trip_review_id": [], "took_place": false }
])


db.Tickets.insertMany([
  { "_id": 501, "trip_id": 5, "reserved_seat_no": 1, "ticket_status": 'P', "first_name": "Lisa", "last_name": "White" },
  { "_id": 101, "trip_id": 1, "reserved_seat_no": 1, "ticket_status": 'P', "first_name": "John", "last_name": "Doe" },
  { "_id": 102, "trip_id": 1, "reserved_seat_no": 2, "ticket_status": 'P', "first_name": "David", "last_name": "Brown" },
  { "_id": 201, "trip_id": 2, "reserved_seat_no": 1, "ticket_status": 'P', "first_name": "Anna", "last_name": "Smith" },
  { "_id": 502, "trip_id": 5, "reserved_seat_no": 2, "ticket_status": 'N', "first_name": "Lisa", "last_name": "White" },
  { "_id": 503, "trip_id": 5, "reserved_seat_no": 1, "ticket_status": 'C', "first_name": "Mark", "last_name": "Taylor" },
  { "_id": 301, "trip_id": 3, "reserved_seat_no": 1, "ticket_status": 'N', "first_name": "David", "last_name": "Brown" },
  { "_id": 504, "trip_id": 5, "reserved_seat_no": 4, "ticket_status": 'C', "first_name": "John", "last_name": "Doe" }
])

db.Reviews.insertMany([
  { "_id": 101, "customer_id": 1, "trip_id": 1, "company_id": 1, "review_date": {"$date": "2023-03-01T00:00:00Z"}, "stars": 5, "review_description": "Excellent experience!" },
  { "_id": 102, "customer_id": 3, "trip_id": 1, "company_id": 1, "review_date": {"$date": "2023-03-02T00:00:00Z"}, "stars": 4, "review_description": "Very good trip, well organized." },
  { "_id": 201, "customer_id": 2, "trip_id": 2, "company_id": 1, "review_date": {"$date": "2023-04-01T00:00:00Z"}, "stars": 3, "review_description": "Decent, but could be better." },
  { "_id": 301, "customer_id": 3, "trip_id": 3, "company_id": 2, "review_date": {"$date": "2023-04-26T00:00:00Z"}, "stars": 5, "review_description": "It was super!" },
])
```

Kolekcje po wypełnieniu danymi:

- Companies

![](img/60%20-%20companies.png)

- Customers

![](img/61%20-%20customers.png)

- Reviews

![](img/62%20-%20reviews.png)

- Tickets

![](img/63%20-%20tickets.png)

- Trips

![](img/64%20-%20trips.png)

<div style="page-break-after: always;"></div>

#### c)  Analiza wad/zalet danego podejścia na konkretnych przykładach

- Wyświetlenie wszystkich ocen z `reviews` wraz z `company_name` dla każdej wycieczki `trip`, która już się odbyła

Przez to, że mamy umieszczone te dane w kilku różnych kolekcjach, zapytanie będzie składać się z kilku operatorów, a czas wykonania będzie nie najlepszy, ponieważ potrzebujemy łączyć kilka tabel. W drugim podejściu będzie pokazane alternatywne wykonanie tego zypatania.

```js
db.Trips.aggregate(
{$match: {took_place: true}},

{$lookup: {from: "Companies",
           localField: "company_id",
           foreignField: "_id",
           as: "Company"}},

{$unwind: "$Company"},

{$lookup: {from: "Reviews",
           localField: "trip_review_id",
           foreignField: "_id",
           as: "Review"}},

{$project: {"company_name" : "$Company.company_name", location: 1, "marks" : "$Review.stars"}}
)
```

Wynik danego zypytania:

![](img/71.png)

- Wyświetlenie wszystkich biletów `tickets`, które mają status "Paid" albo "New"

W tym przypadku w łatwy sposób można skorzystać z kolekcji Tickets, która zawiera interesujące nas informacje (wystarczy dodać prosty warunek). Natomiast w strukturze z zagnieżdżonymi dokumentami mielibyśmy odwoływaś się do nich wewnątrz innych kolekcji (będzie pokazane niżej).

```js
db.Tickets.find({$or : [{ticket_status: 'P'}, {ticket_status: 'N'}]})
```

Wynik danego zypytania:

![](img/72.png)

- Dodanie nowego `review`

W podejściu tabelarycznym potrzebujemy tylko 1 raz wstawić dokument, zawierający informacje o danym review, dodając jego ID do list `reviews` w kolekcjach `trips` oraz `customers`. Dzięki braku redundacji takich złożonych struktur danych, wstawione dane będa zajmowały mniej miejsca, niż w przypadku podejścia dokumentowego, ponieważ w tym podejściu będziemy potrzebowali zrobić kilka razy więcej insertów i odpowiednio wykorzystać kilka razy więcej pamięci do przechowywania redundantych danych, natomiast będą one wygodniejsze w wykorzystaniu.

Przed wstawieniem ustawiliśmy status wydarzenia na odbyty.

```js
db.Trips.updateOne(
    { _id: 5 }, { $set: { took_place: true } }
)
```

```js
db.Reviews.insertOne({ "_id": 501, "customer_id": 1, "trip_id": 5, "company_id": 3, "review_date": {"$date": "2024-05-03T00:00:00Z"}, "stars": 4.5, "review_description": "Great trip but it seems to be too expensive" },)
db.Customers.updateOne({"_id" : 1}, {$push: {customer_review_id: 501}})
db.Trips.updateOne({"_id" : 5}, {$push: {trip_review_id: 501}})
```

Wynik danego zypytania:

Reviews:

![](img/74.png)

Customers:

![](img/75.png)

Trips:

![](img/76.png)

- Wyświetlenie średniej oceny dla każdej firmy `company_name` na podstawie wszystkich ocen z `reviews` dla każdej wycieczki `trip`, która już się odbyła

Skoro dane są podzielone na kilka różnych kolekcji, zapytanie będzie dość skomplikowane i nie wydajne. Nie możemy uniknąć łączenia kilku tabel, co ma istotny wpływ na czas wykonania polecenia i jego rozmiar. W drugim podejściu będzie pokazane alternatywne wykonanie tego zypatania.

```js
db.Trips.aggregate([
  {$match: {took_place: true}},
  {$lookup: {
    from: "Companies",
    localField: "company_id",
    foreignField: "_id",
    as: "Company"
  }},
  {$unwind: "$Company"},
  {$lookup: {
    from: "Reviews",
    localField: "trip_review_id",
    foreignField: "_id",
    as: "Review"
  }},
  {$unwind: "$Review"},
  {$project: {
    "company_id": "$Company._id",
    "company_name": "$Company.company_name",
    "marks": "$Review.stars"
  }},
  {$group: {
    _id: "$company_name",
    average_rating: {$avg: "$marks"}
  }}
])
```

Wynik danego zypytania:

![](img/79.png)

### Podejście dokumentowe

#### a)  Analiza danego wariantu

W takim podejściu uporządkujemy dane w taki sposób, że każda kolekcja przedstawia pewien rodzaj encji, ale nie potrzebujemy przedstawiać wszystkich encji w osobnych kolekcjach. Encje mogą być powiązane między sobą przez referencje (w przypadku kilku dokumentów w różnych kolekcjach) albo za pomocą dokumentów zagnieżdżonych.

- Zalety
    - Brak konieczności używania kosztownych operacji do łączenia kilku dokumentów różnych kolekcji
    - Dobre podejście w przypadku rzadkiego modyfikowania danych, ponieważ mamy redundację danych
    - Lepsza wydajność w przypadku, gdy potrzebujemy informacji z całego dokumentu wraz z zagnieżdżonymi dokumentami
    - Utrzymywanie powiązynch elementów w jednym dokumencie poprzez użycie dokumentów zagnieżdżonych

- Wady
    - Redundacja danych
    - Utrudnienia modyfikacji: w konsekwencji redundacji danych potrzebujemy modyfikować te same dane wielokrotnie w różnych miejscah
    - Konieczność użycia bardziej złożonych zapytań w przypadku, gdy potrzebujemy dostać się do informacji zawartych w dokumentach zagnieżdżonych
    - W szczególnych przypadkach trudny do zrozumienia model danych (np. w przypadku wielokrotnego zagnieżdżenia dokumentów w zagnieżdżnych dokumentach)
    - Brak zagwarantowanej spójności danych

W takim podejściu przekształciliśmy poprzednią strukturę tabelaryczną, używając dokumentów zagnieżdżonych. Z 5 kolekcji (Customers, Trips, Reviews, Companies, Tickets) zostało tylko 2 (Customers i Trips). Informacje z kolekcji "Companies" zostały przeniesione do "Trips", informacje o ticketach i reviews każdego z kilentów zostały przeniesione do dwóch tablic dokumentów zagnieżdżonych w kolekcji "Customers". Wynika to z tego, że te dane prawie nigdy nie są zmieniane (chyba że status Ticketu, ale to nawet łatwiej zmieniać w dokumencie danego klienta). W taki sposób zmniejszyła się ilość danych, natomias trochę utrudnił się cały schemat.

#### b)  Utworzenie kolekcji i wypełnienie kolekcji przykładowymi danymi

Przykładowa struktura bazy danych wygląda następująco:

```js
Customers
{
	"_id": Number,
	"owned_tickets": [
            {
                "_id": Number,  
                "reserved_seat_no": Number, 
                "ticket_status": String
            }
    ],  
	"customer_reviews": [
            {
                "_id": Number, 
                "trip_id": Number, 
                "review_date": Date, 
                "stars": Number, 
                "review_description": String
            }
    ],
	"first_name": String,
	"last_name": String, 
}

Trips 
{
	"_id": Number,  
	"company_name": String, 
	"tickets": [
            {
                "_id": Number,  
                "reserved_seat_no": Number, 
                "ticket_status": String
            }
    ], 
	"date": Date, 
	"location": String, 
	"trip_reviews": [
            {
                "_id": Number, 
                "customer_id": Number,
                "review_date": Date, 
                "stars": Number, 
                "review_description": String
            }
    ],
    "took_place": Boolean
}

```

Stworzenie bazy danych według wyżej przedstawionego pomysłu:

```js
use doc_trip_database

db.createCollection("Customers")
db.createCollection("Trips")
```

Stworzenie rożnego rodzaju danych dla przykładowej bazy danych:
    
Kilka przydatnych informacji:
    
- took_place ustawione na True w kolekcji Trips oznacza, że wycieczka się już odbyła,
- review_id oraz ticket_id to liczby 3-cyfrowe zaczynające się od trip_id. 3 bilety dla np. trip_id=1 to: 101,102,103,
- seats_no to liczby odpowiadające numerom ticketów: np dla ticketa 102, seats_no to 2 itd
- ticket_status to jeden symbol spośród 'N', 'P' i 'C', oznaczających kolejno "New", "Paid" i "Canceled"

```js
db.Customers.insertMany([
  { "_id": 1, "owned_tickets": [

  { "_id": 101, "trip_id": 1, "reserved_seat_no": 1, "ticket_status": 'P'},
  { "_id": 504, "trip_id": 5, "reserved_seat_no": 4, "ticket_status": 'C'}

  ], "customer_reviews": [
        { "review_id": 101, "trip_id": 1, "review_date": {"$date": "2023-03-01T00:00:00Z"}, "stars": 5, "review_description": "Excellent experience!" }
  ], "first_name": "John", "last_name": "Doe" },

  { "_id": 2, "owned_tickets": [

  { "_id": 201, "trip_id": 2, "reserved_seat_no": 1, "ticket_status": 'P'}

  ], "customer_reviews": [

  { "review_id": 201, "customer_id": 2, "trip_id": 2, "review_date": {"$date": "2023-04-01T00:00:00Z"}, "stars": 3, "review_description": "Decent, but could be better." }

  ], "first_name": "Anna", "last_name": "Smith" },
  { "_id": 3, "owned_tickets": [

  { "_id": 102, "trip_id": 1, "reserved_seat_no": 2, "ticket_status": 'P'},
  { "_id": 301, "trip_id": 3, "reserved_seat_no": 1, "ticket_status": 'N'}

  ], "customer_reviews": [

  { "review_id": 102, "trip_id": 1, "review_date": {"$date": "2023-03-02T00:00:00Z"}, "stars": 4, "review_description": "Very good trip, well organized." },
  { "review_id": 301, "trip_id": 3, "review_date": {"$date": "2023-04-26T00:00:00Z"}, "stars": 5, "review_description": "It was super!" }


  ], "first_name": "David", "last_name": "Brown" },
  { "_id": 4, "owned_tickets": [

   { "_id": 501, "trip_id": 5, "reserved_seat_no": 1, "ticket_status": 'P'},
   { "_id": 502, "trip_id": 5, "reserved_seat_no": 2, "ticket_status": 'N'}

  ], "customer_reviews": [], "first_name": "Lisa", "last_name": "White" },
  { "_id": 5, "owned_tickets": [{ "_id": 503, "trip_id": 5, "reserved_seat_no": 1, "ticket_status": 'C'}], "customer_reviews": [], "first_name": "Mark", "last_name": "Taylor" }
])

db.Trips.insertMany([
  { "_id": 1, "company_name": "Adventure Works", "tickets": [

  { "_id": 101, "customer_id": 1, "reserved_seat_no": 1, "ticket_status": 'P'},
  { "_id": 102, "customer_id": 3, "reserved_seat_no": 2, "ticket_status": 'P'}

  ], "date": {"$date": "2024-04-15T00:00:00Z"}, "location": "Warsaw", "trip_reviews": [

  { "review_id": 101, "customer_id": 1, "review_date": {"$date": "2023-03-01T00:00:00Z"}, "stars": 5, "review_description": "Excellent experience!" },
  { "review_id": 102, "customer_id": 3, "review_date": {"$date": "2023-03-02T00:00:00Z"}, "stars": 4, "review_description": "Very good trip, well organized." }


  ], "took_place": true },
  { "_id": 2, "company_name": "Adventure Works", "tickets": [

  { "_id": 201, "customer_id": 2, "trip_id": 2, "reserved_seat_no": 1, "ticket_status": 'P'}

  ], "date": {"$date": "2024-05-20T00:00:00Z"}, "location": "Krakow", "trip_reviews": [

  { "review_id": 201, "customer_id": 2, "review_date": {"$date": "2023-04-01T00:00:00Z"}, "stars": 3, "review_description": "Decent, but could be better." }


  ], "took_place": true },
  { "_id": 3, "company_name": "Travel Corp", "tickets": [

  { "_id": 301, "customer_id": 3, "reserved_seat_no": 1, "ticket_status": 'N'}

  ], "date": {"$date": "2024-06-10T00:00:00Z"}, "location": "Gdansk", "trip_reviews": [

  { "review_id": 301, "customer_id": 3, "trip_id": 3, "review_date": {"$date": "2023-04-26T00:00:00Z"}, "stars": 5, "review_description": "It was super!" }

  ], "took_place": true },
  { "_id": 4, "company_name": "Travel Corp", "tickets": [], "date": {"$date": "2024-07-15T00:00:00Z"}, "location": "Poznan", "trip_reviews": [], "took_place": false },
  { "_id": 5, "company_name": "Holiday Makers", "tickets": [

  { "_id": 501, "customer_id": 4, "reserved_seat_no": 1, "ticket_status": 'P'},
  { "_id": 502, "customer_id": 4, "reserved_seat_no": 2, "ticket_status": 'N'},
  { "_id": 503, "customer_id": 5, "reserved_seat_no": 1, "ticket_status": 'C'},
  { "_id": 504, "customer_id": 1, "reserved_seat_no": 4, "ticket_status": 'C'}

  ], "date": {"$date": "2024-08-25T00:00:00Z"}, "location": "Wroclaw", "trip_reviews": [], "took_place": false },
  { "_id": 6, "company_name": "Globe Trotters", "tickets": [], "date": {"$date": "2024-09-14T00:00:00Z"}, "location": "Sopot", "trip_reviews": [], "took_place": false },
  { "_id": 7, "company_name": "Globe Trotters", "tickets": [], "date": {"$date": "2024-09-15T00:00:00Z"}, "location": "Gdynia", "trip_reviews": [], "took_place": false },
  { "_id": 8, "company_name": "Pathfinders", "tickets": [], "date": {"$date": "2024-09-29T00:00:00Z"}, "location": "Katowice", "trip_reviews": [], "took_place": false }
])
```

Kolekcje po wypełnieniu danymi:

- Customers

![](img/65%20-%20customers.png)

- Trips

![](img/66%20-%20trips.png)

#### c)  Analiza wad/zalet danego podejścia na konkretnych przykładach

- Wyświetlenie wszystkich ocen z `reviews` wraz z `company_name` dla każdej wycieczki `trip`, która już się odbyła

Dzięki temu, że mamy umieszczone te dane w jednym miejscu, zapytanie będzie dość proste, a czas wykonania polecenia krótki, ponieważ nie potrzebujemy łączyć kilku tabel, jak to było pokazane wyżej w przypadku innego podejścia

```js
db.Trips.aggregate(
    {$match: {"took_place": true}},
    {$project: {company_name: 1, location: 1, "marks": "$trip_reviews.stars"}}
)
```

Wynik danego zypytania:

![](img/70.png)

- Wyświetlenie wszystkich biletów `tickets`, które mają status "Paid" albo "New"

W tym przypadku Customer agreguje Tickets, więc potrzebujemy bardziej skomplikowanych zapytań aby dostać się do informacji zamieszczonych w dokumentach zagnieżdżonych. Natomiast w strukturze tablicowej, jak było pokazane wyżej, takie zapytanie jest bardziej przejrzyste, ponieważ wszystkie dane są na jednym poziome w ramach jednej kolekcji.

```js
db.Customers.aggregate(
    {$unwind: "$owned_tickets"},
    {$match: {$or: [{"owned_tickets.ticket_status": 'P'}, {"owned_tickets.ticket_status": 'N'}]}},
    {$project: {_id: 0, _id: "$owned_tickets._id", first_name: 1, last_name: 1, reserved_seat_no : "$owned_tickets.reserved_seat_no",
    ticket_status: "$owned_tickets.ticket_status", trip_id: "$owned_tickets.trip_id"}}
)
```
<div style="page-break-after: always;"></div>

Wynik danego zypytania:

![](img/73.png)

- Dodanie nowego `review`

W podejściu dokumentowym potrzebujemy kilka razy (zależnie od zamodelowanej struktury bazy danych) wstawić dokument, zawierający informacje o danym review, do różnych kolekcji. W porównywaniu do sposoby tabelarycznego, tutaj będziemy potrzebowali zrobić kilka razy więcej insertów i odpowiednio wykorzystać kilka razy więcej pamięci do przechowywania redundantych danych, natomiast będą one wygodniejsze w wykorzystaniu.

Przed wstawieniem ustawiliśmy status wydarzenia na odbyty.

```js
db.Trips.updateOne(
    { _id: 5 },
    { $set: { took_place: true } }
)
```

```js
db.Customers.updateOne({"_id" : 1}, {$push: {customer_reviews: { "_id": 501, "trip_id": 5, "review_date": {"$date": "2024-05-03T00:00:00Z"}, "stars": 4.5, "review_description": "Great trip but it seems to be too expensive" }}})
db.Trips.updateOne({"_id" : 5}, {$push: {trip_reviews: { "_id": 501, "customer_id": 1, "review_date": {"$date": "2024-05-03T00:00:00Z"}, "stars": 4.5, "review_description": "Great trip but it seems to be too expensive" }}})
```

Wynik danego zypytania:

Customers:

![](img/77.png)

Trips:

![](img/78.png)

- Wyświetlenie średniej oceny dla każdej firmy `company_name` na podstawie wszystkich ocen z `reviews` dla każdej wycieczki `trip`, która już się odbyła

W porównywaniu do podejścia tabelarycznego, tutaj polecenie jest prostsze i wydajniejsze, skoro mamy informacje w jednej kolekcji i nie potrzebujemy używać kosztowny operator łączenia.

```js
db.Trips.aggregate(
    {$match: {"took_place": true}},
    {$project: {company_name: 1, "marks": "$trip_reviews.stars"}},
    {$unwind: "$marks"},
    {$group: {
        _id: "$company_name",
        average_rating: {$avg: "$marks"}
        }}
)

```

Wynik danego zypytania:

![](img/80.png)

---

## Wnioski

W ramach danego ćwiczenia przetestowaliśmy działanie różnych operatorów do wyszukiwania danych oraz rozważyliśmy różne podejścia do modelowania dokumentowej bazy danych w MongoDB. Po przeprowadzeniu różnych eksperymentów, możemy wyciągnąć wniosek, że dokumentowe bazy danych są "samoopisujące się" i bardzo przydatne. Na podstawie zrealizowanego zadania №2 możemy stwierdzić, że każde podejście ma swoje wady i zalety. W przypadku, gdy dane są często modyfikowane, a zapytania do kilku encji są rzadkie - najlepszym sposobem jest "tabelaryczna" baza danych (podobnie do Transact SQL, dane każdej encji są umieszczone w różnych kolekcjach), natomiast w przypadku, gdy dane są modyfikowane rzadko, ale często korzystamy z różnych relacji, lepszym wariantem będzie "dokumentowe" podejście (dane są umieszczone w dokumentach zagnieżdżonych).

Punktacja:

|         |     |
| ------- | --- |
| zadanie | pkt |
| 1       | 0,6 |
| 2       | 1,4 |
| razem   | 2   |



