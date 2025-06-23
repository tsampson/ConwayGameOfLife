# Project Title

## Overview

Conway's Game of Life WebApi implementation

## Architecture

ConwayGameOfLife.Api uses an approach that marries Hexagonal Architecture with 3 Tiered architecture, each layer has its own responsibility. Presentation is responsible for incomming communication (port in), Orchestration (or managers) is responsible for application flow, Processing (or engines) is responsible for algorithims and processing data, and Infrastructure (ports out) is responsible for communication outward to services, databases, etc.

## Volatility / Accounting for Change

While programming Conways game of life (intitially created in 1970), I found myself want to add some variations to make it cooler. Like:
1. Changing or adding to the Game Rules.
2. Being able to add seasons to the game board iterations.
3. Being able to identify patterns in the boards (still lifes, oscillators, spaceships, pulsars).

Although I account for these types wants, the code I wrote would be scalable to add these features.

## Problems Faced

Easily the hardest problem faced was to track Board State and identify when a Board state starts to repeat in a performant fashion. If I were to do it over again, I would probably go with an eventual consistancy approach that searched for board loops, while processing boards as fast as possible.

## Installation
### Mongo Persistance Layer Instructions
install chocolaty and then install mongodb
choco install mongodb.install --version=7.0.0

uninstall mongodb and then chocolatey
choco uninstall mongodb.install --version=7.0.0

***Note
default dataPath: $env:ProgramData\MongoDB\data\db
default logPath: $env:ProgramData\MongoDB\log

### Appsettings.json
The folowing settings are accounted for in the appsettings.json file:

ConnectionString for "GameOfLifeMongoDb"

MaxGenerations

## Validation, Logging, and Error Handling
Validation added around UploadBoard endpoint.

Logging added to log all errors to the console and a logfile.

Error Handling should be talked about. I often see to many developers throw and swallow exceptions, instead of accounting for use cases appropriately. This is a source for instability issues.

## Heartbeat Endpoint
api/Diagnostics/Heartbeat endpoint added.


