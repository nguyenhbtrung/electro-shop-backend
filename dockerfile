# Stage 1: Build aplication
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["electro-shop-backend.csproj", "./"]
RUN dotnet restore "electro-shop-backend.csproj"

COPY . .
WORKDIR "/src"

RUN dotnet build "electro-shop-backend.csproj" -c Release -o /app/build

# Stage 2: Publish application
FROM build AS publish
RUN dotnet publish "electro-shop-backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

# stage 3: Create final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:${PORT}

# Expose port 8080.
# EXPOSE 8080

ENTRYPOINT ["dotnet", "electro-shop-backend.dll"]