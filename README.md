To effectively run the "TransactionalSystem" application, follow these steps:

1. Clone the application to your desired directory.

2. Ensure Docker is installed and running on your system.

3. Navigate to the main directory (where the `docker-compose.yml` and solution files are located) and execute the command `docker-compose up --build`.

4. Open a web browser and go to `http://localhost:8013`.

5. Use the GET Data endpoint to generate and display customer data.

6. For a specific customer, identified by their `customerId`, use the POST account endpoint.

7. You can utilize other GET endpoints to confirm the functionality of the system.

Additional Note:
- The Swagger UI is accessible at `http://localhost:8014/swagger/`. Here, you can delete all existing customers and regenerate them. Customers are created using the Bogus library, so the naming conventions are not under our control.
- CI is connected and verifies whether the solution builds.
