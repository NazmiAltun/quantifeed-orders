# Orders Api

A solution developed based on the requirements provided by QuantiFeed for exercise purposes

## Requirements

* **dotnet 6 SDK** : To build the solution
* **dotnet 6 Runtime** : To run the API
* **redis server**: Ideally the latest version or a version that support lua scripting.
* **docker & docker-compose** (optional) : Ideally the latest version. To run redis dependency and stress/perf tests.

## How to run
* Integration tests and the API itself require redis. For local development and testing redis service added in the docker-compose that can be used by running  `docker-compose up -d redis` shell command
* To run stress tests run `docker-compose up perf-test`  command in the terminal

## Stress Test Tool
A small,primitive console app is developed to performance test the api. Following environment variables/configurations can be used to configure the app.

* *StartRequestCount* : Tool stresses the API by sending requests concurrently initially it starts with  *StartRequestCount* . Default: 1000
* *RampUpRequestCount* : After each step, concurrent request count is incremented(ramped up) by the number of *RampUpRequestCount*. Default: 500.
* *TotalRequestCount* : Maximum request count. Default: 100000000
* *InvalidRequestPercent*: % of the invalid request to be sent. e.g;  if it's set to 3, it means %3,  which means every 3 out of 100 request will be invalid. Feel free to change this number for each test runs to see the difference.  Default: 0
* *ExistingIdRequestPercent*: % of the requests that contains orderIds that already exist in the db. e.g;  if it's set to 1, it means %1,  which means every 1 out of 100 request will be invalid.  Feel free to change this number for each test runs to see the difference. Default: 0

## Assumptions
There're some assumptions made regarding to the API design since some of the requirements were ambiguous

* Client that will consume the API supports Grpc
* Currency code check is case insensitive. e.g;  USD is accepted but usd is not accepted.
* Requests are executed in all or nothing manner ; the entire request is rolled back if persisting one of the orders fails due to orderId duplication
* Currency list for validation comes from currencies.json file in the solution which is randomly found on the github. It's assumed it contains all the valid currencies

## Steps taken to ensure high performance
* **Redis is the choice for db**  : redis is preferred to persist orders. Since, the requirements don't mention about an ACID compilant solution. Redis performs better since it's an in-memory db that comes with optional durability (ideally AOF) and the orders records we need to persist are not huge.
* **One db roundtrip per request** : redis supports lua scripting (version 2.6 and onwards ), which enables us to send one request to the db server to persist order records by writing a few lines of lua script. It is also atomic.
* **Flattening the orders**: redis [HSETNX](https://redis.io/commands/hsetnx/) is used to store orders and orderId is used for hash key. However, this wouldn't work if we store the orders in the format we receive them , since the orderId of the child orders will be ignored in child/parent structure. We want to ensure all the orders (both stock and basket) have unique id. Hence we need to flatten the orders before storing them. Which is a trade-off that'll make the reads a bit more complicated and will make writes easier and faster.
* **Using Grpc protocol** : Features such as; binary messaging format and http/2.0 protocol(bidirectional streaming) makes GRPC more performant option than traditional REST APIs.

## Performance Improvements That Can Be Made
* Some trade-offs can be made based on the use cases. Example; how often the API would receive orders with order Id that already exists in the db ? If that happens too often, it might be useful to have another caching layer that validates order ids first before persisting them. Since this is ambiguous, it wasn't used in the solution to keep solution simpler. Some of the known use cases/business cases can allow to make different trade-offs.

* [Redis Cluster](https://redis.io/docs/manual/scaling/) or any other third party redis clustering solutions can  be used if redis becomes the bottleneck.

* Flattened orders are stored as json strings for simplicity. A more performant alternative would be, sending order properties separately and storing them as values in the hashset with redis HSETNX command. This might improve the performance since we'll avoid JSON serialization and we'll send smaller payload to redis. However, this would be less readable solution with more complex db writes(lua script).


* For validations FluentValidation framework is used which is a general-purpose framework. Even though it has internal caching mechanism that caches expression trees that are defined as rules. More performant options can be considered, if micro level optimizations are really important.
