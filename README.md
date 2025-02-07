# dotnet-csharp-redis-demo

## Prerequisites
- .NET SDK (version 7 or above)
- Redis server (local or remote)
- IDE or text editor (e.g., Visual Studio, VS Code)

## Setup Instructions
1. Clone or download the project.

2. Install Redis if it's not running already. You can use either a local or a remote Redis server.

3. Configure your environment variables by creating a .env file in the root directory of the project and add the following configuration:

```env
REDIS_HOST=localhost
REDIS_PORT=6379
REDIS_USERNAME=default
REDIS_PASSWORD=your-local-password

```

4. Modify appsettings.json to include your Redis connection string using the values from the .env file (e.g., localhost:6379).

5. Run the application using the following command:

```bash
dotnet run
```

6. Open your browser and go to https://localhost:5001 to test the API.

## API Endpoints
- GET /api/words: Get all words from Redis.
- GET /api/words/{word_id}: Get a word by ID from Redis.
- POST /api/words: Create a new word.
- PUT /api/words/{word_id}: Update an existing word.
- DELETE /api/words/{word_id}: Delete a word.
- GET /health: Check the health status of the application.


## Environment Variables
To ensure smooth operation, the following environment variables need to be configured for your local environment:
- REDIS_HOST: The hostname or IP address of your Redis server (default: localhost).
- REDIS_PORT: The port on which Redis is running (default: 6379).
- REDIS_USERNAME: The username for Redis authentication (default: default).
- REDIS_PASSWORD: The password for Redis authentication (replace with your actual password).

Make sure to modify these settings according to your local or production Redis server configuration.

