FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["RewardsPointCalculator.Api.csproj", "./"]
RUN dotnet restore "RewardsPointCalculator.Api.csproj"
COPY . .

RUN dotnet publish "RewardsPointCalculator.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "RewardsPointCalculator.Api.dll"]
