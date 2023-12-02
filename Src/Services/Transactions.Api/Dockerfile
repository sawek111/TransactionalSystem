FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Src/Services/Transactions.Api/Transactions.Api.csproj", "Src/Services/Transactions.Api/"]
RUN dotnet restore "Src/Services/Transactions.Api/Transactions.Api.csproj"
COPY . .
WORKDIR "/src/Src/Services/Transactions.Api"
RUN dotnet build "Transactions.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Transactions.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Transactions.Api.dll"]
