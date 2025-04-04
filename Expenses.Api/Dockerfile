# Stage 1: Base image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

# Stage 2: Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Expenses.Api/Expenses.Api.csproj", "Expenses.Api/"]
RUN dotnet restore "Expenses.Api/Expenses.Api.csproj"
COPY . .
WORKDIR "/src/Expenses.Api"
RUN dotnet build "Expenses.Api.csproj" -c Release -o /app/build

# Stage 3: Publish stage
FROM build AS publish
RUN dotnet publish "Expenses.Api.csproj" -c Release -o /app/publish

# Stage 4: Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Expenses.Api.dll"]
