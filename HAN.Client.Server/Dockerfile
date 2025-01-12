FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HAN.Client.Server/HAN.Client.Server.csproj", "HAN.Client.Server/"]
COPY ["HAN.Services/HAN.Services.csproj", "HAN.Services/"]
COPY ["Data/HAN.Data.csproj", "Data/"]
COPY ["HAN.Repositories/HAN.Repositories.csproj", "HAN.Repositories/"]
COPY ["HAN.Utilities/HAN.Utilities.csproj", "HAN.Utilities/"]
RUN dotnet restore "HAN.Client.Server/HAN.Client.Server.csproj"
COPY . .
WORKDIR "/src/HAN.Client.Server"
RUN dotnet build "HAN.Client.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "HAN.Client.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HAN.Client.Server.dll"]
