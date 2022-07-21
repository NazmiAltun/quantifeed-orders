# Orders Api

Api that's developed based on the requirements provided by QuantiFeed

## Requirements

* **dotnet 6 SDK** : To build the solution
* **dotnet 6 Runtime** : To run the API
* **redis server**: Ideally the latest version or a version that support lua scripting.
* **docker & docker-compose** (optional) : Ideally the latest version. To run redis dependency and stress/perf tests.

## How to run
* Integration tests and the API itself requires redis. For local development and testing redis defined in the docker-compose can be used by running  `docker-compose up -d redis` shell command
* To run stress tests run `./stress.sh`  script located in the repo

## Assumptions
There're some assumptions made regarding to the API design since some of the requirements are either unclear or ambiguous

* Client that will consume the API supports Grpc
* Currency code check is case insensitive. e.g;  USD is accepted but usd is not accepted.
* Requests are executed in all or nothing manner ; the entire request is rolled back if persisting one of the orders fails due to orderId duplication

## Steps taken to ensure high performance
* **Redis is the choice for db**  : redis is chosen to persist orders. Since, the requirements don't mention about ACID compilant solution. Redis performs better since it's an in-memory db that comes with optional durability (ideally AOF) and the order records we need to use are not huge.
* **One db roundtrip per request** : redis supports lua scripting (version 2.6 and onwards ), which enables us to send one request to the db server to persist order records.Which is also atomic.
* **Flattening the orders**: redis [HSETNX](https://redis.io/commands/hsetnx/) is used to store orders and orderId is used for hash key. However, this doesn't work if we store the orders as we receive them , since the orderId of the child orders will be ignored in child/parent structure. We want to ensure all the orders (both stock and basket) have unique id. Hence we need to flatten the orders before storing them. Which is also a trade-off that'll make the reads a bit more complicated.
* **Using Grpc protocol** : Features such as binary messaging format and http/2.0 protocol(bidirectional streaming) makes GRPC more performant option than traditional REST APIs.

## Performance Improvements That Can Be Made
* Some trade-offs can be made based on the use cases. Example; how often the API would receive orders with order Id that already exists in the db ? If this happens too often, it might be useful to have another caching layer that validates order ids first before persisting them.
* [Redis Cluster](https://redis.io/docs/manual/scaling/) can be used in production environment.
* For validations FluentValidation framework is used which is a general-purpose framework. Even though it has internal caching mechanism that caches expression trees that are defined as rules. More performant options can be considered, if micro level optimizations are really important.
