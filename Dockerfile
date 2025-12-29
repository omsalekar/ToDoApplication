# ---------- Base runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

ENV DOTNET_GC_SERVER=0
ENV DOTNET_GC_CONCURRENT=1
ENV DOTNET_EnableDiagnostics=0

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
ENTRYPOINT ["dotnet", "ToDoApi.dll"]
