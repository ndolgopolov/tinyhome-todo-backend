FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/TinyHomeTodo.Api/TinyHomeTodo.Api.csproj", "src/TinyHomeTodo.Api/"]
COPY ["src/TinyHomeTodo.Application/TinyHomeTodo.Application.csproj", "src/TinyHomeTodo.Application/"]
COPY ["src/TinyHomeTodo.Infrastructure/TinyHomeTodo.Infrastructure.csproj", "src/TinyHomeTodo.Infrastructure/"]
RUN dotnet restore "src/TinyHomeTodo.Api/TinyHomeTodo.Api.csproj"

COPY . .
RUN dotnet publish "src/TinyHomeTodo.Api/TinyHomeTodo.Api.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "TinyHomeTodo.Api.dll"]
