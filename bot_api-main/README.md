# Bot.Api

Bot.Api is a web api solution.

## Technologies

Asp.net core 8.0 LTS project created using CQRS(partly) and MediatR ,KAFKA as the broker

Others:

- ASP.NET Core
- MediatR
- Mapster
- Fluent Validation
- Serilog
- KAFKA
- MassTransit
- Minimal API

## Requirements

- https://www.microsoft.com/net/download/windows#/current the latest .NET Core 0.x SDK

### Running in Visual Studio

- Set Startup projects:
  - Web.API

## How to configure API & Swagger

- For development is running on url - `http://localhost:40767` and swagger UI is available on url - `https://localhost:7184/swagger`
- For swagger UI is configured an API


## KAFKA service

- To set up KAFKA via:

### KAFKA


"KAFKAQueueSettings": {
        "HostName": "",
        "UserName": "",
        "Port": ""
    }


### Solution structure:

- Core.Api:

  - `Web.API` - project that contains the web api

  ###Project folder structure
  
   - 'Application': Application
   - contains service definition and implementation,DTOs,Exceptions and  Dependency injection setup

