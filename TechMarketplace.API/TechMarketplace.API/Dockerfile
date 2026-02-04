FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# ყურადღება მიაქციე, რომ პროექტის სახელი ემთხვეოდეს შენსას
COPY ["TechMarketplace.API.csproj", "./"]
RUN dotnet restore "./TechMarketplace.API.csproj"

COPY . .
RUN dotnet publish "TechMarketplace.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "TechMarketplace.API.dll"]