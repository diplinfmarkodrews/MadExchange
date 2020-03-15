# Distributed MadExchange TradeEngine .NET Core

**Coinbase, Bybit, Bitmex, FTX, Blade, etc..**

**What is Distributed MadExchange Trade Engine ?**
----------------

MadExchange is a high performance trade execution engine for (crypto) exchanges. Through its microservice architecture its highly scalable, allowing to trade on any number of accounts.
The Connector is very flexible, which makes integration of additional exchanges easy, fast and robust at the same time.
It only needs a configuration file, which describes the endpoints and datatypes of an exchange. 
Data is de/serialized with the fastest serializer available for dotnet framework. Reflection is used to map incoming data into the common domain model. 
As a common Data Store, the service architecture uses the distributed 
cache Redis, providing realtime data-access for tradeexecution. 
For other usecases, the Connector can be configured to store incoming data in databases or other data services, timeseries dbs, etc.
The Connector supports http requests and websocket subscriptions.
All control data is handled by RabbitMq, within a cqrs pattern.
The result is a clean service operation abstraction.

Trade execution
Todo :)

![MadExchange.Connector.Service](/MadExchange.Connector.Service.png)
![MadExchange.TradeExecution.Service](/MadExchange.TradeExecution.Service.png)
![MadExchange.TradeController.Service + DataAggregationService](/MadExchange.Controller.Service+DataAggregation.Service.png)

 This project is licensed under the terms of the MIT license.
 
**RoadMap **
----------------

MadExchange Connector Service
--
  -Basic Components
    
    SocketManager: *SocketSubscriptions
                   *RedisIntegration    
    
    HttpClientFactories: 
                *Configuration -> EndPointAccess                
                *FaultTolerance -> Resend/Reinitialisation / RateLimit                
                *Mapping to Domain-Models                
    BusIntegration
   
  - Extension: Tracing, further ExchangeIntegrations ...
  
Test : Basic Components

    Interface => Bybit
    private: Data/Operation Access
    public: Instrument Queries

      *HttpRequests
      *Sockets
      *RedisIntegration

 

MadExchange Client Trade Execution Service
--
   -Basic Components
      
      Execution Algorithms:
          *ChangePosition
          *ClosePosition
          *OrderPlacing/Cancellation         
      Registration/Deregistration/Monitoring ConnectorClients
        *Request Arbitration
        *Socket Support

  Test: TradeExecution in Testnet


  Testing/DeploymentServiceIntegration
  

**How to start the application?**
----------------

Service can be started locally via `dotnet run` (executed in the `src/` directory) or `./scripts/dotnet-run.sh` shell script, by default it will be available under http://localhost:5000.

You can also run the application using [Docker](https://www.docker.com) `docker run --name  -p 5000:5000 --network MadXchange  (include `-d` to run the container in the background).

It is required to have the basic infrastructure up and running first ([RabbitMQ](https://www.rabbitmq.com), [MongoDB](https://www.mongodb.com) and [Redis](https://redis.io)) in the same docker network named `MadXchange`. Services can be started using [Docker Compose](https://docs.docker.com/compose) (and [this file](https://github.com/diplinfmarkodrews/MadXchange/blob/master/compose/docker-compose-infrastructure.yml)) 

BTC: 1HF7XTPmkPPYzmj7AZr4eNfwVVLEZ1rogn