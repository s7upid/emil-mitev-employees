# EmployeesService

## Prerequisites

- Docker and Docker Compose installed on your machine
- (Optional) Node.js and npm/yarn if you want to run the frontend or backend separately without Docker
- Ensure ports 3000, 4444, and 7902 are free or adjust the ports in the `docker-compose.yml` accordingly

## Running the application

This project uses Docker Compose with multiple services:

- **sqlserver**: Microsoft SQL Server container with your database
- **api**: Backend API service
- **ui**: React frontend UI

To build and start all services, run:

```bash
docker-compose --profile fullApp up --build -d
```

## Accessing the application
- **Frontend UI**: Open your browser and navigate to http://localhost:3000
- **Backend API**: Accessible at http://localhost:4444
- **SQL Server**: Connect to the database on port 7902 (e.g., using SQL Server Management Studio or Azure Data Studio)