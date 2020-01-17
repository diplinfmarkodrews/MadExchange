# Distributed MadExchange TradeEngine .NET Core


**What is Distributed MadExchange Trade Engine ?**
----------------

![MadExchange.Connector.Service](/MadExchange.Connector.Service.png)
![MadExchange.TradeExecution.Service](/MadExchange.TradeExecution.Service.png)
![MadExchange.TradeController.Service + DataAggregationService](/MadExchange.Controller.Service+DataAggregation.Service.png)



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


MadExchange Trade Controller Service
--
  -Basic Components
      
      Connection to Client Execution Service
      ProfitView Integration
        *Alerts = > TradeGenerator
        *ClientConfiguration


Required for final engine tests => Approx Begin March
---



MadExchange.DataAggregation.Service
--
  Basic Components:
  
    Telegram UI ApiKey
    Telegram Reporting
      *Channnel
      *ClientWise
    Payment Service
  
  March Testing/DeploymentServiceIntegration
  

**How to start the application?**
----------------

Service can be started locally via `dotnet run` (executed in the `src/` directory) or `./scripts/dotnet-run.sh` shell script, by default it will be available under http://localhost:5000.

You can also run the application using [Docker](https://www.docker.com) `docker run --name  -p 5000:5000 --network MadXchange  (include `-d` to run the container in the background).

It is required to have the basic infrastructure up and running first ([RabbitMQ](https://www.rabbitmq.com), [MongoDB](https://www.mongodb.com) and [Redis](https://redis.io)) in the same docker network named `MadXchange`. Services can be started using [Docker Compose](https://docs.docker.com/compose) (and [this file](https://github.com/diplinfmarkodrews/MadXchange/blob/master/compose/docker-compose-infrastructure.yml)) 
