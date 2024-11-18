#Use the .NET 8 base image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the .NET 8 SDK image to build the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the csproj file and restore the dependencies
COPY ["ProjectManagementApp.csproj", "./"]
RUN dotnet restore "ProjectManagementApp.csproj"

# Copy the rest of the project files and build it
COPY . . 
WORKDIR "/src"
RUN dotnet build "ProjectManagementApp.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "ProjectManagementApp.csproj" -c Release -o /app/publish

# Use the base image to run the published application
FROM base AS final
WORKDIR /app

# Set the environment variable so ASP.NET Core listens on all IP addresses
ENV DOTNET_URLS=http://+:80

# Copy the published files from the previous build stage
COPY --from=publish /app/publish .

# Set the entry point to run the application
ENTRYPOINT ["dotnet", "ProjectManagementApp.dll"]
