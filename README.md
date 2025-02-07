# dotnet-csharp-redis-demo

## Prerequisites
- .NET SDK (version 7 or above)
- Redis server (local or remote)
- IDE or text editor (e.g., Visual Studio, VS Code)

## Setup Instructions
1. Clone or download the project.
2. Install Redis if it's not running already.
3. Modify appsettings.json to include your Redis connection string.
4. Run the application using:
```bash
dotnet run
```
5. Open your browser and go to https://localhost:5001 to test the API.

## API Endpoints
- GET /api/words: Get all words from Redis.
- GET /api/words/{word_id}: Get a word by ID from Redis.
- POST /api/words: Create a new word.
- PUT /api/words/{word_id}: Update an existing word.
- DELETE /api/words/{word_id}: Delete a word.
- GET /health: Check the health status of the application.