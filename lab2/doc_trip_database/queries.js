/// use doc_trip_database

db.createCollection("Customers")
db.createCollection("Trips")

db.Customers.insertMany([
    {
        "_id": 1, "owned_tickets": [

            { "_id": 101, "trip_id": 1, "reserved_seat_no": 1, "ticket_status": 'P' },
            { "_id": 504, "trip_id": 5, "reserved_seat_no": 4, "ticket_status": 'C' }

        ], "customer_reviews": [
            { "review_id": 101, "trip_id": 1, "review_date": { "$date": "2023-03-01T00:00:00Z" }, "stars": 5, "review_description": "Excellent experience!" }
        ], "first_name": "John", "last_name": "Doe"
    },

    {
        "_id": 2, "owned_tickets": [

            { "_id": 201, "trip_id": 2, "reserved_seat_no": 1, "ticket_status": 'P' }

        ], "customer_reviews": [

            { "review_id": 201, "customer_id": 2, "trip_id": 2, "review_date": { "$date": "2023-04-01T00:00:00Z" }, "stars": 3, "review_description": "Decent, but could be better." }

        ], "first_name": "Anna", "last_name": "Smith"
    },
    {
        "_id": 3, "owned_tickets": [

            { "_id": 102, "trip_id": 1, "reserved_seat_no": 2, "ticket_status": 'P' },
            { "_id": 301, "trip_id": 3, "reserved_seat_no": 1, "ticket_status": 'N' }

        ], "customer_reviews": [

            { "review_id": 102, "trip_id": 1, "review_date": { "$date": "2023-03-02T00:00:00Z" }, "stars": 4, "review_description": "Very good trip, well organized." },
            { "review_id": 301, "trip_id": 3, "review_date": { "$date": "2023-04-26T00:00:00Z" }, "stars": 5, "review_description": "It was super!" }


        ], "first_name": "David", "last_name": "Brown"
    },
    {
        "_id": 4, "owned_tickets": [

            { "_id": 501, "trip_id": 5, "reserved_seat_no": 1, "ticket_status": 'P' },
            { "_id": 502, "trip_id": 5, "reserved_seat_no": 2, "ticket_status": 'N' }

        ], "customer_reviews": [], "first_name": "Lisa", "last_name": "White"
    },
    { "_id": 5, "owned_tickets": [{ "_id": 503, "trip_id": 5, "reserved_seat_no": 1, "ticket_status": 'C' }], "customer_reviews": [], "first_name": "Mark", "last_name": "Taylor" }
])

db.Trips.insertMany([
    {
        "_id": 1, "company_name": "Adventure Works", "tickets": [

            { "_id": 101, "customer_id": 1, "reserved_seat_no": 1, "ticket_status": 'P' },
            { "_id": 102, "customer_id": 3, "reserved_seat_no": 2, "ticket_status": 'P' }

        ], "date": { "$date": "2024-04-15T00:00:00Z" }, "location": "Warsaw", "trip_reviews": [

            { "review_id": 101, "customer_id": 1, "review_date": { "$date": "2023-03-01T00:00:00Z" }, "stars": 5, "review_description": "Excellent experience!" },
            { "review_id": 102, "customer_id": 3, "review_date": { "$date": "2023-03-02T00:00:00Z" }, "stars": 4, "review_description": "Very good trip, well organized." }


        ], "took_place": true
    },
    {
        "_id": 2, "company_name": "Adventure Works", "tickets": [

            { "_id": 201, "customer_id": 2, "trip_id": 2, "reserved_seat_no": 1, "ticket_status": 'P' }

        ], "date": { "$date": "2024-05-20T00:00:00Z" }, "location": "Krakow", "trip_reviews": [

            { "review_id": 201, "customer_id": 2, "review_date": { "$date": "2023-04-01T00:00:00Z" }, "stars": 3, "review_description": "Decent, but could be better." }


        ], "took_place": true
    },
    {
        "_id": 3, "company_name": "Travel Corp", "tickets": [

            { "_id": 301, "customer_id": 3, "reserved_seat_no": 1, "ticket_status": 'N' }

        ], "date": { "$date": "2024-06-10T00:00:00Z" }, "location": "Gdansk", "trip_reviews": [

            { "review_id": 301, "customer_id": 3, "trip_id": 3, "review_date": { "$date": "2023-04-26T00:00:00Z" }, "stars": 5, "review_description": "It was super!" }

        ], "took_place": true
    },
    { "_id": 4, "company_name": "Travel Corp", "tickets": [], "date": { "$date": "2024-07-15T00:00:00Z" }, "location": "Poznan", "trip_reviews": [], "took_place": false },
    {
        "_id": 5, "company_name": "Holiday Makers", "tickets": [

            { "_id": 501, "customer_id": 4, "reserved_seat_no": 1, "ticket_status": 'P' },
            { "_id": 502, "customer_id": 4, "reserved_seat_no": 2, "ticket_status": 'N' },
            { "_id": 503, "customer_id": 5, "reserved_seat_no": 1, "ticket_status": 'C' },
            { "_id": 504, "customer_id": 1, "reserved_seat_no": 4, "ticket_status": 'C' }

        ], "date": { "$date": "2024-08-25T00:00:00Z" }, "location": "Wroclaw", "trip_reviews": [], "took_place": false
    },
    { "_id": 6, "company_name": "Globe Trotters", "tickets": [], "date": { "$date": "2024-09-14T00:00:00Z" }, "location": "Sopot", "trip_reviews": [], "took_place": false },
    { "_id": 7, "company_name": "Globe Trotters", "tickets": [], "date": { "$date": "2024-09-15T00:00:00Z" }, "location": "Gdynia", "trip_reviews": [], "took_place": false },
    { "_id": 8, "company_name": "Pathfinders", "tickets": [], "date": { "$date": "2024-09-29T00:00:00Z" }, "location": "Katowice", "trip_reviews": [], "took_place": false }
])

db.Trips.aggregate(
    { $match: { "took_place": true } },
    { $project: { company_name: 1, location: 1, "marks": "$trip_reviews.stars" } }
)

db.Customers.aggregate(
    { $unwind: "$owned_tickets" },
    { $match: { $or: [{ "owned_tickets.ticket_status": 'P' }, { "owned_tickets.ticket_status": 'N' }] } },
    {
        $project: {
            _id: 0, _id: "$owned_tickets._id", first_name: 1, last_name: 1, reserved_seat_no: "$owned_tickets.reserved_seat_no",
            ticket_status: "$owned_tickets.ticket_status", trip_id: "$owned_tickets.trip_id"
        }
    }
)

db.Trips.updateOne(
    { _id: 5 },
    { $set: { took_place: true } }
)

db.Customers.updateOne({ "_id": 1 }, { $push: { customer_reviews: { "_id": 501, "trip_id": 5, "review_date": { "$date": "2024-05-03T00:00:00Z" }, "stars": 4.5, "review_description": "Great trip but it seems to be too expensive" } } })
db.Trips.updateOne({ "_id": 5 }, { $push: { trip_reviews: { "_id": 501, "customer_id": 1, "review_date": { "$date": "2024-05-03T00:00:00Z" }, "stars": 4.5, "review_description": "Great trip but it seems to be too expensive" } } })


db.Trips.aggregate(
    { $match: { "took_place": true } },
    { $project: { company_name: 1, "marks": "$trip_reviews.stars" } },
    { $unwind: "$marks" },
    {
        $group: {
            _id: "$company_name",
            average_rating: { $avg: "$marks" }
        }
    }
)