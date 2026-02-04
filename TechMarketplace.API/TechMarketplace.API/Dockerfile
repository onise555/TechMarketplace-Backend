FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# მიუთითე სრული გზა მთავარი საქაღალდიდან
COPY ["TechMarketplace.API/TechMarketplace.API/TechMarketplace.API.csproj", "TechMarketplace.API/TechMarketplace.API/"]
RUN dotnet restore "TechMarketplace.API/TechMarketplace.API/TechMarketplace.API.csproj"

COPY . .
WORKDIR "/src/TechMarketplace.API/TechMarketplace.API"
RUN dotnet publish "TechMarketplace.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "TechMarketplace.API.dll"]