FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Src/Services/Accounts.Api/Accounts.Api.csproj", "Src/Services/Accounts.Api/"]
RUN dotnet restore "Src/Services/Accounts.Api/Accounts.Api.csproj"
COPY . .
WORKDIR "/src/Src/Services/Accounts.Api"
RUN dotnet build "Accounts.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Accounts.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Accounts.Api.dll"]
