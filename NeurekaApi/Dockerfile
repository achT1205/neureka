FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["NeurekaApi/NeurekaApi.csproj", "NeurekaApi/"]
RUN dotnet restore "NeurekaApi/NeurekaApi.csproj"
COPY . .
WORKDIR "/src/NeurekaApi"
RUN dotnet build "NeurekaApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NeurekaApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NeurekaApi.dll"]