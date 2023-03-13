# RewardPointsCalculator

Customer's Transaction Data:

| Customer ID | Customer Name | Transaction Amount | Transaction Date |
| ----------- | ------------- | ------------------ | ---------------- |
| 001         | Ross snow     | $100.00            | 2023-03-01       |
| 001         | Ross snow     | $50.00             | 2023-02-01       |
| 001         | Ross snow     | $70.00             | 2023-01-01       |
| 002         | Sarah Kim     | $120.00            | 2023-03-04       |
| 002         | Sarah Kim     | $20.00             | 2023-02-04       |
| 002         | Sarah Kim     | $20.00             | 2023-01-04       |
| 003         | Bob Johnson   | $75.00             | 2023-03-03       |
| 003         | Bob Johnson   | $60.00             | 2023-02-03       |
| 003         | Bob Johnson   | $80.00             | 2023-01-03       |


We can then use this information to calculate the total reward points earned by each customer:

| Customer ID | Customer Name | Total Points | Jan Points | Feb Points | Mar Points |
| -----------| ------------- | ----------- | ----------| ----------| ---------- |
| 001         | Ross Snow    | 70         | 20        | 0         | 50         |
| 002         | Sarah Kim      | 90         | 0         | 0        | 90         |
| 003         | Bob Johnson   | 555         | 30         | 10         | 25        |

# Rewards Point Calculator API
The Rewards Point Calculator API is a RESTful API that allows you to calculate the reward points earned by customers based on their transaction history.

##  API Endpoints
The following endpoints are available:

| Endpoint                               | Method | Description                                                      |
| -------------------------------------- | ------ | ---------------------------------------------------------------- |
| /api/rewards/                             | GET    |Returns the reward points earned by all customers.   |
| /api/rewards/{customerId}  | GET    | Returns the reward points earned by a specific customer.         |

## Sample Requests

### Get Reward Points for All Customers
This endpoint returns a list of reward points and monthly points earned by all customers.

#### Request
GET /api/rewards

#### Response

```json
HTTP/1.1 200 OK
Content-Type: application/json

[
  {
    "customerId": "45b3d191-008f-4ef6-b9b6-4f4a4c4e4567",
    "customerName": "John Doe",
    "totalPoints": 180,
    "monthlyPoints": {
      "01/2022": 80,
      "02/2022": 100
    }
  },
  {
    "customerId": "8c33f47d-0a50-4872-8d8b-1c28e772a890",
    "customerName": "Jane Smith",
    "totalPoints": 360,
    "monthlyPoints": {
      "01/2022": 160,
      "02/2022": 200
    }
  },
  {
    "customerId": "8cfafccf-7b0f-4279-b3bb-43d746b034a2",
    "customerName": "Bob Johnson",
    "totalPoints": 240,
    "monthlyPoints": {
      "01/2022": 60,
      "02/2022": 180
    }
  }
]
```
#### Error Responses
If an error occurs while retrieving reward points for all customers, the API will return an error response with an appropriate status code and message:

```json
HTTP/1.1 500 Internal Server Error
Content-Type: application/json

{
  "message": "An error occurred while retrieving reward points for all customers"
}
```
If there are no customers in the database, the API will return a 404 Not Found response:

```json
HTTP/1.1 404 Not Found
Content-Type: application/json

{
  "message": "No customers found"
}
```
To invoke this API using a client, you can use an HTTP client such as Postman or cURL. Here's an example of how to invoke the GetRewardPointsForAllCustomerAsync endpoint using cURL:

```sql
curl -X GET "http://localhost:5017/api/rewards/"  -H "accept: application/json"
```

### Get Reward Points for a Specific Customer
This Endpoint returns reward points and montly points earned by a specific customer.

#### Request:

```json
GET /api/rewards/{id}
{
  "id": "8fba563e-43c9-4b1d-bc38-f942bbcd7dbf"
}
```
#### Response:
```json
Status: 200 OK

{
  "customerId": "8fba563e-43c9-4b1d-bc38-f942bbcd7dbf",
  "customerName": "John Doe",
  "totalPoints": 325,
  "monthlyPoints": {
    "01/2022": 100,
    "02/2022": 75,
    "03/2022": 150
  }
}
```

#### Error Responses
If an error occurs while retrieving reward points for a customer, the API will return an error response with an appropriate status code and message:

```json
HTTP/1.1 500 Internal Server Error
Content-Type: application/json

{
  "message": "An error occurred while retrieving reward points for customer 8fba563e-43c9-4b1d-bc38-f942bbcd7dbf"
}
```
If there is no such customer in the database, the API will return a 404 Not Found response:

```json
HTTP/1.1 404 Not Found
Content-Type: application/json

{
  "message": "Customer not found"
}
```
To invoke this API using a client, you can use an HTTP client such as Postman or cURL. Here's an example of how to invoke the GetRewardPointsForCustomerAsync endpoint using cURL:

```sql
curl -X GET "http://localhost:5017/api/rewards/8fba563e-43c9-4b1d-bc38-f942bbcd7dbf" -H "accept: application/json"
```








