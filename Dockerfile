FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY HappyAddress/HappyAddress.csproj HappyAddress/
RUN dotnet restore HappyAddress/HappyAddress.csproj

COPY HappyAddress/ HappyAddress/
RUN dotnet publish HappyAddress/HappyAddress.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .
RUN mkdir -p /app/wwwroot/uploads /app/wwwroot/uploads/avatars

ENTRYPOINT ["dotnet", "HappyAddress.dll"]
