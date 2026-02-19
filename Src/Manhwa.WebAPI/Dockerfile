FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Manhwa.WebAPI/Manhwa.WebAPI.csproj", "Manhwa.WebAPI/"]
COPY ["Manhwa.Infrastructure/Manhwa.Infrastructure.csproj", "Manhwa.Infrastructure/"]
COPY ["Manhwa.Application/Manhwa.Application.csproj", "Manhwa.Application/"]
COPY ["Manhwa.Domain/Manhwa.Domain.csproj", "Manhwa.Domain/"]

RUN dotnet restore "Manhwa.WebAPI/Manhwa.WebAPI.csproj"

COPY . .
WORKDIR "/src/Manhwa.WebAPI"
RUN dotnet build "Manhwa.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Manhwa.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "Manhwa.WebAPI.dll"]