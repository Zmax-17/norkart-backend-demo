# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /source

# Copy everything
COPY . .

# Restore
RUN dotnet restore

# Publish
RUN dotnet publish -c Release -o /app

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Render exposes port 10000
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

# Copy build output
COPY --from=build /app .

# Start the app
ENTRYPOINT ["dotnet", "NorkartDemo.dll"]
