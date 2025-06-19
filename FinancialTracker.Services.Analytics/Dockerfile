#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5030

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["FinancialTracker.Services.Analytics/FinancialTracker.Services.Analytics.csproj", "FinancialTracker.Services.Analytics/"]
RUN dotnet restore "FinancialTracker.Services.Analytics/FinancialTracker.Services.Analytics.csproj"
COPY . .
WORKDIR "/src/FinancialTracker.Services.Analytics"
RUN dotnet build "FinancialTracker.Services.Analytics.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FinancialTracker.Services.Analytics.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FinancialTracker.Services.Analytics.dll"]
