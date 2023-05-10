FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TodoTests/TodoTests.csproj", "TodoTests/"]
RUN dotnet restore "TodoTests/TodoTests.csproj"
COPY . .
WORKDIR "/src/TodoTests"
RUN dotnet build "TodoTests.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TodoTests.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TodoTests.dll"]
