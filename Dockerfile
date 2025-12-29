# ---------- Base runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy AS base
WORKDIR /app
EXPOSE 8080

# ---------- Build ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ToDoApi/*.csproj ToDoApi/
RUN dotnet restore ToDoApi/ToDoApi.csproj

COPY . .
RUN dotnet publish ToDoApi/ToDoApi.csproj -c Release -o /app/publish

# ---------- Final image ----------
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENV DOTNET_RUNNING_IN_CONTAINER=true

ENTRYPOINT ["dotnet", "ToDoApi.dll"]
