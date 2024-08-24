# Transaction Processing System

This project is a transaction processing system built using .NET, gRPC, and MassTransit. It consists of three main components:

1. **Minimal API** - Exposes an endpoint to receive transactions.
2. **gRPC Service** - Handles payment processing.
3. **MassTransit Consumer** - Communicates between the Minimal API and the gRPC service.

## Overview

The system processes transactions by receiving HTTP requests, forwarding them to a consumer via MassTransit, and then communicating with a gRPC service for payment processing. The gRPC response is stored in a Mongo database.

### Flow

1. A user posts a transaction (with user ID, reference ID, and amount) to the Minimal API.
2. The Minimal API sends a message to the MassTransit consumer using RabbitMQ.
3. The consumer communicates with the gRPC service to process the payment.
4. The response from the gRPC service is stored in MongoDB, including the transaction details.

## Components

### 1. Minimal API

- **Project**: `ChapChap.Api`
- **Endpoint**: `POST /transactions`

### 2. gRPC Service

- **Project**: `ChapChap.gRPC`

### 3. MassTransit Consumer

- **Project**: `ChapChap.Consumers`

#### Shared Protobuf Definitions

- **Project**: `ChapChap.ProtobuffDefinitions`


## Prerequisites

- .NET 8 SDK
- RabbitMQ
- MongoDB

## Build and Run on Windows

### 1. Set Up RabbitMQ and MongoDB

Ensure that RabbitMQ and MongoDB are installed and running on your machine.

MongDB Installation https://www.mongodb.com/docs/manual/tutorial/install-mongodb-on-windows/

MongoDB Shell https://www.mongodb.com/try/download/shell

#### The simplest way to install RabbitMQ is with Chocolatey a package manager for windows from here
https://community.chocolatey.org/packages/rabbitmq

#### Chocolatey can be installed by following the instructions below
```bash
# Open PowerShell as Administrator and run the following script

# Set the execution policy to allow the script to run
Set-ExecutionPolicy Bypass -Scope Process -Force;

# Download and install Chocolatey using the official installation script
[System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072;
Invoke-Expression ((New-Object System.Net.WebClient).DownloadString('https://community.chocolatey.org/install.ps1'));

```
Once installed the application will use the default credentials to connect to RabbitMQ
```username: guest```
```password: guest ```

Navigate to the RabbitMQ admin portal default endpoint: http://localhost:15672/#/

### 2. Clone the Repository and cd into the folder

```bash
git clone https://github.com/dreadedcoder/tps-chap.git
cd tps-chap/
```

For each project, open the terminal and run:

#### gRPC Service

```bash
cd ChapChap.gRPC
dotnet build
dotnet run
```
Ensure the service is running at http://localhost:5062
#### Minimal API
In another terminal run:
```bash
cd ChapChap.Api
dotnet build
dotnet run
```

Navigate to http://localhost:5012/swagger/index.html for the swagger UI and execute a request. 

The the request and responses will be visible in the ChapChap.Api terminal window.

------------------------
------------------------

You can connect to the running instance of MongoDB via the default user (no username, no password) running at ```mongodb://localhost:27017``` by running ```mongosh ``` in the command prompt or powershell and then finding the database named ```chapdb``` with the collection ```transactions```. The Transaction details will be saved there. 

### 4. Testing

The project includes unit tests for the API, gRPC service, and consumer. To run the tests, navigate to each test project directory and execute:

```bash
dotnet test
```

---end---
-----
