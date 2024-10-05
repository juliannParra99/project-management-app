# Project Management System 

The Project Management System is an API-based application designed to facilitate efficient project and task management within an organization. The system allows users to create projects, assign tasks, and track their progress. It offers robust features for managing multiple projects, each with its own set of tasks, while ensuring security and controlled access using JWT (JSON Web Token) for authentication.

## Key Features:
- Project Management: Users can create, update, and delete projects. Each project can have a set of tasks associated with it, making it easier to manage deliverables and deadlines.

- Task Management: Within each project, tasks can be created, updated, and marked as completed. The tasks are assigned to specific projects, allowing for structured organization.

- JWT-based Authentication: The system uses JWT to authenticate users, ensuring that only authorized users can access project and task data. Once a user is authenticated, they receive a token that is required for all further API interactions.

- User Roles and Permissions: The system supports user management, allowing different permissions and roles, which provide controlled access to features based on user privileges.

## Technologies Used:
- .NET 8: The backend of the system is built using the latest version of .NET, providing a scalable and efficient environment for API development.

- Entity Framework Core: This ORM (Object-Relational Mapping) tool is used to manage the database, enabling seamless interactions with the underlying SQLite or SQL Server database.

- SQLite/SQL Server: The system uses a database to store information about projects, tasks, and users, with the flexibility to choose between SQLite for lightweight storage or SQL Server for larger-scale implementations.

- Identity Framework: The system leverages ASP.NET Identity for managing user authentication and authorization, including the ability to assign roles to users.

This project is designed to be scalable and secure, providing essential features for managing both simple and complex projects, while ensuring that only authorized users can interact with the system's resources. The use of modern technologies like .NET 8 and JWT ensures that the system is robust and reliable for real-world project management needs.
