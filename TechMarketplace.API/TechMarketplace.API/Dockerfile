FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# ფაილების კოპირება (რადგან Dockerfile იმავე საქაღალდეშია, სადაც .csproj)
COPY ["TechMarketplace.API.csproj", "./"]
RUN dotnet restore "./TechMarketplace.API.csproj" [cite: 3]

COPY . .
RUN dotnet publish "TechMarketplace.API.csproj" -c Release -o /app/publish [cite: 2]

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Railway-ს პორტი
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "TechMarketplace.API.dll"]