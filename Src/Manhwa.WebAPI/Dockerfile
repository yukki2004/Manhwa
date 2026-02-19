FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src


COPY ["Src/Manhwa.WebAPI/Manhwa.WebAPI.csproj", "Src/Manhwa.WebAPI/"]
COPY ["Src/Manhwa.Infrastructure/Manhwa.Infrastructure.csproj", "Src/Manhwa.Infrastructure/"]
COPY ["Src/Manhwa.Application/Manhwa.Application.csproj", "Src/Manhwa.Application/"]
COPY ["Src/Manhwa.Domain/Manhwa.Domain.csproj", "Src/Manhwa.Domain/"]

RUN dotnet restore "Src/Manhwa.WebAPI/Manhwa.WebAPI.csproj"

COPY Src/ Src/

WORKDIR "/src/Src/Manhwa.WebAPI"
RUN dotnet build "Manhwa.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Manhwa.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "Manhwa.WebAPI.dll"]