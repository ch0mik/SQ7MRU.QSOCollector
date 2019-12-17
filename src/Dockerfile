FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
WORKDIR /app

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o /out ./QSOCollector/SQ7MRU.QSOCollector.csproj

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
ENV ASPNETCORE_URLS http://*:5000
EXPOSE 5000
COPY --from=build-env /out .
ENTRYPOINT ["dotnet", "SQ7MRU.QSOCollector.dll"]