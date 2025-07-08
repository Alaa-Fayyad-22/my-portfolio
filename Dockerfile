# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of your source code
COPY . ./

# Publish the app to /app/publish folder
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app

# Copy published app from build stage
COPY --from=build /app/publish ./

# Run the app
ENTRYPOINT ["dotnet", "MyPortfolio.dll"]
