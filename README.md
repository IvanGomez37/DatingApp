# Dating App

Welcome to **Dating App** – a dating web application inspired by Tinder, built with Angular and .NET Core. This application provides a complete dating experience, including profiles, matches, messages, and more.

## Table of Contents
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Running the Application](#running-the-application)
- [Folder Structure](#folder-structure)
- [API Endpoints](#api-endpoints)
- [Screenshots](#screenshots)
- [Contributing](#contributing)

## Features
- **User Profiles**: Create and manage your personal dating profile with photos and a bio.
- **Matching**: Find users with similar interests and swipe to match.
- **Messaging**: Chat with your matches in real-time.
- **User Authentication**: Secure login and registration using JWT authentication.
- **Real-time Updates**: Get notified instantly of new messages and matches.

## Tech Stack
- **Frontend**: Angular
- **Backend**: .NET Core Web API
- **Database**: SQL Server
- **Authentication**: JWT (JSON Web Tokens)

## Getting Started

### Prerequisites
Make sure you have the following installed on your system:
- **Node.js** and **npm**: [Download here](https://nodejs.org/)
- **Angular CLI**: Install via `npm install -g @angular/cli`
- **.NET Core SDK**: [Download here](https://dotnet.microsoft.com/download)
- **SQL Server**: Install locally or use a cloud service

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/your-username/dating-app.git
   cd dating-app
   ```

2. **Install frontend dependencies**
   ```bash
   cd client
   npm install
   ```

3. **Install backend dependencies**
   ```bash
   cd ../API
   dotnet restore
   ```

4. **Database setup**
   - Update the connection string in `API/appsettings.json` to point to your SQL Server instance.
   - Run the following commands to apply migrations and create the database:
     ```bash
     cd API
     dotnet ef database update
     ```

### Running the Application

1. **Start the .NET Core API**
   ```bash
   cd API
   dotnet run
   ```

2. **Start the Angular application**
   ```bash
   cd ../client
   ng serve
   ```
   
3. Open your browser and go to `http://localhost:4200` to access the app.

## Folder Structure

```
dating-app/
├── API/              # .NET Core Web API project
│   ├── Controllers/   # API controllers for handling requests
│   ├── Data/          # Database context and migration files
│   ├── Models/        # Entity models
│   ├── Services/      # Service layer for business logic
│   └── appsettings.json
└── client/           # Angular application
    ├── src/
    │   ├── app/       # Angular components, services, modules, etc.
    │   ├── assets/    # Images and other static files
    │   └── environments/
    └── angular.json
```

## API Endpoints

- **Authentication**
  - `POST /api/auth/register` - Register a new user
  - `POST /api/auth/login` - Log in a user

- **Profiles**
  - `GET /api/users` - Get a list of users
  - `GET /api/users/{id}` - Get user details by ID

- **Matches**
  - `POST /api/matches/{userId}` - Like or match with a user

- **Messages**
  - `POST /api/messages` - Send a message
  - `GET /api/messages/{userId}` - Retrieve message history with a user

## Screenshots

...

## Contributing

If you’d like to contribute to this project, please fork the repository and submit a pull request. Issues and suggestions are also welcome!