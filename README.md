# Smart Hotel
A web api using .NET Core based on microservices architecture

## Building locally
```
docker-compose build
```

## Running microservices locally
1. Start the data stores
```
docker-compose up sql-data
```
2. Start the microservices
```
docker-compose up
```
3. Verify services are running
```
docker ps
```

## References
* [SmartHotel360 - Backend Services](https://github.com/Microsoft/SmartHotel360-Azure-backend)
