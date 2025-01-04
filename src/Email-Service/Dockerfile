FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /app
EXPOSE 80

# copy all .csproj files and restore as distinct layers. Use of the same COPY command
# for every dockerfile in the project to take advantage of docker caching
COPY Notification-Service.sln Notification-Service.sln
COPY src/API-Service/API-Service.csproj src/API-Service/API-Service.csproj
COPY src/SMS-Service/SMS-Service.csproj src/SMS-Service/SMS-Service.csproj
COPY src/Email-Service/Email-Service.csproj src/Email-Service/Email-Service.csproj
COPY src/Contracts/Contracts.csproj src/Contracts/Contracts.csproj

# Restore package deps
RUN dotnet restore Notification-Service.sln

# Copy the app folders
COPY src/Email-Service src/Email-Service
COPY src/Contracts src/Contracts
WORKDIR /app/src/Email-Service
RUN dotnet publish -c Release -o /app/src/out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/src/out .
ENTRYPOINT ["dotnet", "Email-Service.dll"]