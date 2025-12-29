# ---------- Base runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Limit memory + GC behavior
ENV DOTNET_GC_SERVER=0
ENV DOTNET_GC_CONCURRENT=1
ENV DOTNET_EnableDiagnostics=0

# ---------- Build ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish ToDoApi/ToDoApi.csproj -c Release -o /app/publish --no-restore

# ---------- Final image ----------
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ToDoApi.dll"]
