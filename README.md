# Todo list application.

With an application like this, the first thing to decide is a storage DB. I picked SQL just because of the tight timeframe. If there was more time, I'd go with some document database.

MongoDB fits this task almost perfectly.
It is light, fast, and would let us store loosely typed documents presenting them as json objects. It could be useful to operate a tasks list with no relational restrictions. 

At the same time all the strength of relational db like joins, indexing, bulk operations, etc. simply not used here, as we have no hierarchy, no links between documents, and very basic workflow.

In my vision, the possible evolution of this app is directed to include various media to keep with a task,
and NoSQL db would be good here.

I went with probably the most traditional approach, storing the document as Header and Lines. Despite the slight cumbersomeness of the approach,
it gives enough flexibility for future evolution, yet it is fast in development and well understood by everyone.

Nowadays standard for .Net development is the use of EF as a repository, codefirst EF migrations for incremental db update,
native Dependency Injection with Service classes, that can be grouped and layered with the further evolution.
This idea works well for business applications with medium-to-fast changes in logic and allows to keep functionality decoupled, testable, clean, and meaningfully grouped at the same time.

Normally I separate the .Net solution in several projects. Aside from the main project with the web application, there are Core with general interfaces and enumerations, Data with database context and ORM, Models for all the models, viewmodels and DTOs, and Services for the service layer. Sometimes there appear some more parts, like Business Logic Layer in case it is useful to separate this logic from data and build a Domain Model on the next step.

In the case of a real application, I would expect to scale it up breaking the monolith app in separate services, and the first one would be the Task service and the Task table. That's the main payload and the SQL table is not the most flexible and productive storage. At the same time, the current architecture allows to move it relatively painlessly to the separate service and another stack.
