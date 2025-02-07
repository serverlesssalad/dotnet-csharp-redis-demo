# Use the .NET 9.0 SDK as the base image
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build

# Set environment to Development
ENV ASPNETCORE_ENVIRONMENT=Development

# Set the working directory
WORKDIR /app

# Copy the project files
COPY . .

# Restore dependencies
RUN dotnet restore

# Publish the application
RUN dotnet publish -c Release -o out

# Use the .NET 9.0 ASP.NET Runtime as the final base image
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine

# Set the working directory
WORKDIR /app

# Copy the published application from the build image
COPY --from=build /app/out .

# Expose port 8080 (matching the log)
EXPOSE 8080

# Set the entrypoint
ENTRYPOINT ["dotnet", "dotnet-csharp-redis-demo.dll"]
