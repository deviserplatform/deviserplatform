Deviser Detached
=========

This project is based on [GraphDiff](https://github.com/zzzprojects/GraphDiff)

DbContext extension methods for Entity Framework Code First, that allow you to save an entire detached Model/Entity, with child Entities and Lists, to the database without writing the code to do it.

**This version is for EF Core**

## Features

 - Merge an entire graph of detached entities to the database using DbContext.UpdateGraph<T>();
 - Ensures concurrency is maintained for all child entities in the graph
 - Allows for different configuration mappings to ensure that only changes within the defined graph are persisted
 - Comprehensive testing suite to cover many (un/)common scenarios.

## Collaborators

Karthick Sundararajan

## License

See the LICENSE file for license rights and limitations (MIT).