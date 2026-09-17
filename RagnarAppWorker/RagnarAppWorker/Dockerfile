FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "RagnarAppWorker/RagnarAppWorker/RagnarAppWorker.csproj"
RUN dotnet publish "RagnarAppWorker/RagnarAppWorker/RagnarAppWorker.csproj" \
    -c Release -o /app/publish --no-restore /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "RagnarAppWorker.dll"]
