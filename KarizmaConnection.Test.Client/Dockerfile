FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["KarizmaConnection.Test.Client/KarizmaConnection.Test.Client.csproj", "KarizmaConnection.Test.Client/"]
RUN dotnet restore "KarizmaConnection.Test.Client/KarizmaConnection.Test.Client.csproj"
COPY . .
WORKDIR "/src/KarizmaConnection.Test.Client"
RUN dotnet build "KarizmaConnection.Test.Client.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "KarizmaConnection.Test.Client.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KarizmaConnection.Test.Client.dll"]
