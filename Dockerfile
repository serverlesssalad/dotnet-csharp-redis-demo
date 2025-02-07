# Use the lighter version of the .NET SDK as the base image
FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build

# Set the working directory
WORKDIR /app

# Copy the project files
COPY . .

# Restore dependencies
RUN dotnet restore

# Publish the application
RUN dotnet publish -c Release -o out

# Use the lighter version of the ASP.NET Runtime as the final base image
FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine

# Set the working directory
WORKDIR /app

# Copy the published application from the build image
COPY --from=build /app/out .

# Set the entrypoint
ENTRYPOINT ["dotnet", "dotnet-csharp-redis-demo.dll"]