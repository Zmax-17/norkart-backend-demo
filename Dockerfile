# =========================
# Build stage
# =========================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /source

# Copy everything
COPY . .

# Restore dependencies
RUN dotnet restore

# Publish the app
RUN dotnet publish -c Release -o /app

# =========================
# Runtime stage
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Use the PORT environment variable from Render
ENV ASPNETCORE_URLS=http://+:${PORT:-10000}
EXPOSE ${PORT:-10000}

# Copy published app from build stage
COPY --from=build /app .

# Start the application
ENTRYPOINT ["dotnet", "NorkartDemo.dll"]
