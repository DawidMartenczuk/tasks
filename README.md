# Tasks
Simple Task Management System which is exemplary implementation of **MVC Web Application** using **Microsoft ASP.NET Core 2.2 Framework** and **Entity Framework Core 2.2**.
Features included in this software are standard task managing. Into Task you can assign a assignee user to complete that Task. System allows to log task solving process,
and attach Tasks into milestones.

### Entity Framework Core
I Preffer to use in .NET Framework Projects a built-in ORM - Entity Framework, because it's well documented, and have a lot of features,
that simplify using Relational Database Model in my Apps. And of course it fits without any difficulties to my Programs. It's also fast and
optimized to work in .NET Framework Ecosystem.

### Identity
I used a standard built-in Identity Service, but it Was little extended, for example to use it in Relationships with my Entity Framework Models.
Usually I extend it to using a int as Primary Keys, but this time I tried to use standard Guids to do it like _Microsoft's right way_.

### Frontend
As Frontend part of my Project I used standard delivered with framework RazorViews including Bootstrap CSS Framework. It simplify a Development of project
by built in Helpers, for example my favorite among them are Form helpers with CSRF, XSS protection, and client-side validation using jQuery and before mentioned CSS framework.
It also works perfectly with server-side validation implemented in ASP.NET Core, and Entity Framework Core.

### Available options in app

* Use standard identity: Register and Login
* **C**reate, **R**ead, **U**pdate and **D**elete and filter Tasks
* **C**reate, **R**ead, **U**pdate and **D**elete Milestones
* Assign Task Assignees
* Closing/Opening Tasks
* Attach Tasks to Milestones
* Log Tasks changes, and write custom logs (messages) in Tasks

### Used skills

* C#
* ASP.NET Core Framework
* Entity Framework
* RazorViews
* Implementation of standard Identity built-in ASP.NET Core
* Using MVC architecture and writing CRUD controllers
* Bootstrap CSS Framework
