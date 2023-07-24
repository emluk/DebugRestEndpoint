FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DebugRestEndpoint/DebugRestEndpoint.csproj", "DebugRestEndpoint/"]
RUN dotnet restore "DebugRestEndpoint/DebugRestEndpoint.csproj"
COPY . .
WORKDIR "/src/DebugRestEndpoint"
RUN dotnet build "DebugRestEndpoint.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DebugRestEndpoint.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DebugRestEndpoint.dll"]
