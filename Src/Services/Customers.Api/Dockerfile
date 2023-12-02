FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Src/Services/Customers.Api/Customers.Api.csproj", "Src/Services/Customers.Api/"]
RUN dotnet restore "Src/Services/Customers.Api/Customers.Api.csproj"
COPY . .
WORKDIR "/src/Src/Services/Customers.Api"
RUN dotnet build "Customers.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Customers.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Customers.Api.dll"]
