# Learn about building .NET container images:
# https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Create folders for project files
RUN mkdir Bot.Application

# copy csproj and restore as distinct layers
COPY *.csproj .
COPY Bot.Application/*.csproj ./Bot.Application/
RUN dotnet restore

# copy and publish app and libraries
COPY . .
RUN dotnet publish --no-restore -o /app

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["./Bot.Api"]
