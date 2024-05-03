/// use tab_trip_database

db.createCollection("Companies")
db.createCollection("Customers")
db.createCollection("Trips")
db.createCollection("Tickets")
db.createCollection("Reviews")

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
    { "_id": 1, "company_id": 1, "tickets_id": [101, 102], "date": { "$date": "2024-04-15T00:00:00Z" }, "location": "Warsaw", "trip_review_id": [101, 102], "took_place": true },
    { "_id": 2, "company_id": 1, "tickets_id": [201], "date": { "$date": "2024-05-20T00:00:00Z" }, "location": "Krakow", "trip_review_id": [201], "took_place": true },
    { "_id": 3, "company_id": 2, "tickets_id": [301], "date": { "$date": "2024-06-10T00:00:00Z" }, "location": "Gdansk", "trip_review_id": [301], "took_place": true },
    { "_id": 4, "company_id": 2, "tickets_id": [], "date": { "$date": "2024-07-15T00:00:00Z" }, "location": "Poznan", "trip_review_id": [], "took_place": false },
    { "_id": 5, "company_id": 3, "tickets_id": [501, 502, 503, 504], "date": { "$date": "2024-08-25T00:00:00Z" }, "location": "Wroclaw", "trip_review_id": [], "took_place": false },
    { "_id": 6, "company_id": 4, "tickets_id": [], "date": { "$date": "2024-09-14T00:00:00Z" }, "location": "Sopot", "trip_review_id": [], "took_place": false },
    { "_id": 7, "company_id": 4, "tickets_id": [], "date": { "$date": "2024-09-15T00:00:00Z" }, "location": "Gdynia", "trip_review_id": [], "took_place": false },
    { "_id": 8, "company_id": 5, "tickets_id": [], "date": { "$date": "2024-09-29T00:00:00Z" }, "location": "Katowice", "trip_review_id": [], "took_place": false }
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
    { "_id": 101, "customer_id": 1, "trip_id": 1, "company_id": 1, "review_date": { "$date": "2023-03-01T00:00:00Z" }, "stars": 5, "review_description": "Excellent experience!" },
    { "_id": 102, "customer_id": 3, "trip_id": 1, "company_id": 1, "review_date": { "$date": "2023-03-02T00:00:00Z" }, "stars": 4, "review_description": "Very good trip, well organized." },
    { "_id": 201, "customer_id": 2, "trip_id": 2, "company_id": 1, "review_date": { "$date": "2023-04-01T00:00:00Z" }, "stars": 3, "review_description": "Decent, but could be better." },
    { "_id": 301, "customer_id": 3, "trip_id": 3, "company_id": 2, "review_date": { "$date": "2023-04-26T00:00:00Z" }, "stars": 5, "review_description": "It was super!" },
])

db.Trips.aggregate(
    { $match: { took_place: true } },

    {
        $lookup: {
            from: "Companies",
            localField: "company_id",
            foreignField: "_id",
            as: "Company"
        }
    },

    { $unwind: "$Company" },

    {
        $lookup: {
            from: "Reviews",
            localField: "trip_review_id",
            foreignField: "_id",
            as: "Review"
        }
    },

    { $project: { "company_name": "$Company.company_name", location: 1, "marks": "$Review.stars" } }
)

db.Tickets.find({ $or: [{ ticket_status: 'P' }, { ticket_status: 'N' }] })

db.Trips.updateOne(
    { _id: 5 }, { $set: { took_place: true } }
)

db.Reviews.insertOne({ "_id": 501, "customer_id": 1, "trip_id": 5, "company_id": 3, "review_date": { "$date": "2024-05-03T00:00:00Z" }, "stars": 4.5, "review_description": "Great trip but it seems to be too expensive" },)
db.Customers.updateOne({ "_id": 1 }, { $push: { customer_review_id: 501 } })
db.Trips.updateOne({ "_id": 5 }, { $push: { trip_review_id: 501 } })

db.Trips.aggregate([
    { $match: { took_place: true } },
    {
        $lookup: {
            from: "Companies",
            localField: "company_id",
            foreignField: "_id",
            as: "Company"
        }
    },
    { $unwind: "$Company" },
    {
        $lookup: {
            from: "Reviews",
            localField: "trip_review_id",
            foreignField: "_id",
            as: "Review"
        }
    },
    { $unwind: "$Review" },
    {
        $project: {
            "company_id": "$Company._id",
            "company_name": "$Company.company_name",
            "marks": "$Review.stars"
        }
    },
    {
        $group: {
            _id: "$company_name",
            average_rating: { $avg: "$marks" }
        }
    }
])