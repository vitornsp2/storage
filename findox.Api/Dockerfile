FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["findox.Api/findox.Api.csproj", "findox.Api/"]
RUN dotnet restore "findox.Api/findox.Api.csproj"
COPY . .
WORKDIR "/src/findox.Api/"
RUN dotnet build "findox.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "findox.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "findox.Api.dll"]