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

## Docker Setup and SQLite Database Integration

To run the Project Management System in a Docker container and correctly mount the SQLite database, follow the steps below. The path to the SQLite database will depend on your local machine configuration.

### Prerequisites

- Docker must be installed on your machine.
- You need the `app.db` file, which is the SQLite database for the Project Management System. The location of this file will depend on your local system setup.

### Step 1: Build the Docker Image

Navigate to the root directory of the project, where the `Dockerfile` is located. Then, run the following command to build the Docker image:

````bash
docker build -t project-management-app .```

This command will create the Docker image named project-management-app.

## Step 2: Prepare the SQLite Database File
Make sure you have the `app.db` file, which is the SQLite database for the Project Management System. You should place this file on your local machine, and remember the path, as it will be needed in the next step.

## Step 3: Run the Docker Container with the SQLite Database
To run the Docker container and mount the SQLite database file from your local machine into the container, use the following command:

```bash
docker run -d -p 8080:80 --name project-management-container -v "<path_to_your_db>/app.db:/app/app.db" project-management-app ```


Important:
Replace <path_to_your_db> with the actual path where app.db is located on your local machine. For example, if your app.db file is located in C:/Users/Usuario/OneDrive/Escritorio/Projects/C#/Project-management Project/project-management-app/, the command would look like this:

bash
Copy code
docker run -d -p 8080:80 --name project-management-container -v "C:/Users/Usuario/OneDrive/Escritorio/Projects/C#/Project-management Project/project-management-app/app.db:/app/app.db" project-management-app
The command will:

Run the container in detached mode (-d).
Bind port 8080 of your machine to port 80 in the container, allowing access via http://localhost:8080.
Mount the app.db file from your local machine to the container's /app/app.db path, ensuring the container can access and use the existing SQLite database.
Step 4: Verify the Application
Once the container is running, you should be able to access the Project Management System API by navigating to:

bash
Copy code
http://localhost:8080
If everything is set up correctly, the application will be able to interact with the SQLite database that has been mounted from your local machine.

Step 5: Stopping the Container
To stop the running container, use the following command:

bash
Copy code
docker stop project-management-container
Step 6: Removing the Container
If you no longer need the container, you can remove it using:

bash
Copy code
docker rm project-management-container
Important Notes:
Make sure the path to the app.db file is correctly specified in the docker run command. If the path is incorrect or the database file is missing, the application may not function properly.
If you modify the app.db file on your local machine, you will need to restart the Docker container for the changes to take effect.
````
