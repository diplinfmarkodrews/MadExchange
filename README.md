# Distributed MadExchange TradeEngine .NET Core


**What is Distributed MadExchange Trade Engine ?**
----------------


**How to start the application?**
----------------

Service can be started locally via `dotnet run` (executed in the `src/` directory) or `./scripts/dotnet-run.sh` shell script, by default it will be available under http://localhost:5000.

You can also run the application using [Docker](https://www.docker.com) `docker run --name orders-service -p 5000:5000 --network MadXchange  (include `-d` to run the container in the background).

It is required to have the basic infrastructure up and running first ([RabbitMQ](https://www.rabbitmq.com), [MongoDB](https://www.mongodb.com) and [Redis](https://redis.io)) in the same docker network named `MadXchange`. Services can be started using [Docker Compose](https://docs.docker.com/compose) (and [this file](https://github.com/diplinfmarkodrews/MadXchange/blob/master/compose/docker-compose-infrastructure.yml)) 
