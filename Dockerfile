FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app 
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["UserRegistry/UserRegistry.csproj", "UserRegistry/"]
RUN dotnet restore "UserRegistry/UserRegistry.csproj"
COPY . . 
WORKDIR "/src/UserRegistry"
RUN dotnet build "./UserRegistry.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UserRegistry.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserRegistry.dll"]