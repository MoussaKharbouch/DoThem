# DoThem 📝

A simple **Task Management System** that allows users to manage their personal tasks with custom task types.

---

## 🚀 Project Overview

DoThem is a backend-focused project built using:

* **C#**
* **ADO.NET**
* **SQL Server**

The goal is to implement a clean and structured backend architecture without relying on ORMs like EF Core.

---

## ⭐ Key Highlights

- Clean layered architecture (Domain → Infrastructure → Services → App)
- Built with pure ADO.NET (no ORM)
- Strong input validation in domain models
- Separation of concerns (Repository / Service pattern)
- Comprehensive error handling and validation
- Parameterized SQL queries for security

---

## 🧠 Features

### 👤 User Management

* Register new account
* Login securely with password hashing (SHA256)
* Update user information
* Change user password
* Change user status (Active, Expired, Banned)
* Delete account

### ✅ Task Management

* Create tasks with title, description, and due date
* View personal tasks
* Update task details
* Delete tasks
* Track task status (NotStarted, InProgress, Completed)
* Filter tasks by user
* Filter tasks by user and task type

### 🏷️ Task Types

* Users can define custom task types
* Create unlimited task types per user
* Update task type details
* Delete task types
* View all task types

### 🔄 Task Status

Each task can have one of the following:

* **NotStarted** - Task not yet started
* **InProgress** - Task currently being worked on
* **Completed** - Task is done

---

## 🏗️ Architecture

The project follows a layered architecture:

```
┌─────────────────────────────────────┐
│           App Layer                 │
│   (Console Testing - IAppTester)    │
└─────────────────────────────────────┘
              ↓
┌─────────────────────────────────────┐
│        Services Layer               │
│  (Business Logic & Validation)      │
│  - UserService                      │
│  - TaskItemService                  │
│  - TaskTypeService                  │
└─────────────────────────────────────┘
              ↓
┌─────────────────────────────────────┐
│    Infrastructure Layer             │
│    (Data Access - ADO.NET)          │
│  - UserRepository                   │
│  - TaskItemRepository               │
│  - TaskTypeRepository               │
└─────────────────────────────────────┘
              ↓
┌─────────────────────────────────────┐
│        Domain Layer                 │
│     (Core Entities)                 │
│  - User                             │
│  - TaskItem                         │
│  - TaskType                         │
└─────────────────────────────────────┘
              ↓
┌─────────────────────────────────────┐
│       SQL Server Database           │
│  - Users Table                      │
│  - Tasks Table                      │
│  - TaskTypes Table                  │
└─────────────────────────────────────┘
```

---

## 🔐 System Rules

* Users can only access their own tasks
* Users can only create task types for themselves
* No access to other users' data
* No roles (simple user system)
* Password hashing using SHA256
* No task priorities (for now)

---

## ⚙️ Current Status

### ✅ Completed
- **User Module** (Repository + Service)
  - Registration with validation
  - Login with password hashing
  - User status management
  - Password change functionality
  - Full CRUD operations

- **TaskItem Module** (Repository + Service)
  - Full task management (CRUD)
  - Task filtering by user and type
  - Duplicate prevention
  - Comprehensive validation

- **TaskType Module** (Repository + Service)
  - Custom task type creation
  - Full CRUD operations
  - User-specific task types
  - Duplicate prevention

- **App Layer**
  - IAppTester interface for comprehensive testing
  - Console application for testing all modules

### 📋 Testing Interface (IAppTester)

Comprehensive testing interface that includes:
- Individual method testing for each module
- Complete workflow testing
- Integration testing
- Full test suite execution

---

## 💡 Why ADO.NET?

This project intentionally uses **ADO.NET** instead of EF Core to:

* Gain full control over SQL queries
* Better understand database interactions
* Build a strong backend foundation
* Learn query optimization techniques
* Understand connection pooling and resource management

---

## 🧪 Testing

The project includes a comprehensive testing interface (`IAppTester`) that tests:

* **User Operations** - Registration, login, updates, deletion
* **Task Operations** - CRUD operations, filtering, status management
* **Task Type Operations** - Creation, updates, deletion
* **Integration Workflows** - Complete end-to-end testing

Currently tested using a console application (`DoThem.App`) with multiple scenarios.

---

## 📦 Project Structure

```
DoThem/
├── DoThem.Domain/
│   ├── User.cs
│   ├── TaskItem.cs
│   └── TaskType.cs
│
├── DoThem.Infrastructure/
│   ├── IUserRepository.cs
│   ├── UserRepository.cs
│   ├── ITaskItemRepository.cs
│   ├── TaskItemRepository.cs
│   ├── ITaskTypeRepository.cs
│   └── TaskTypeRepository.cs
│
├── DoThem.Services/
│   ├── IUserService.cs
│   ├── UserService.cs
│   ├── ITaskItemService.cs
│   ├── TaskItemService.cs
│   ├── ITaskTypeService.cs
│   └── TaskTypeService.cs
│
└── DoThem.App/
    ├── IAppTester.cs
    └── Program.cs
```

---

## 📊 Database Schema

### Users Table
- UserID (PK, Identity)
- Username (Unique, Max 100 chars)
- Email (Unique, @gmail.com only, Max 100 chars)
- Password (Hashed, Max 50 chars)
- CreationDate (DateTime, not nullable)
- Status (Active/Expired/Banned)

### TaskTypes Table
- TaskTypeID (PK, Identity)
- UserID (FK to Users)
- Name (Max 30 chars)
- Description (Max 150 chars, nullable)
- CreationDate (DateTime)

### Tasks Table
- TaskID (PK, Identity)
- UserID (FK to Users)
- TaskTypeID (FK to TaskTypes)
- Title (Max 100 chars)
- Description (Max 300 chars, nullable)
- CreationDate (DateTime)
- DueDate (DateTime, nullable)
- Status (NotStarted/InProgress/Completed)

---

## 🔒 Security Features

* ✅ Parameterized SQL queries (prevents SQL injection)
* ✅ SHA256 password hashing
* ✅ Input validation at domain and service layers
* ✅ User data isolation
* ✅ Resource management with `using` statements

---

## 📌 Future Improvements

* Add API layer (ASP.NET Core REST API)
* Implement JWT authentication
* Add logging and error tracking
* Write comprehensive unit tests
* Add database transaction support
* Build frontend (React/Vue)
* Add task reminders and notifications
* Implement task priorities
* Add task categories/tags
* Task sharing between users

---

## 🎯 Validation Rules

### User
- Username: 1-100 chars, no spaces, unique
- Email: Must end with @gmail.com, unique, no spaces
- Password: 1-50 chars (hashed as SHA256)
- CreationDate: Cannot be in future

### TaskItem
- Title: 1-100 chars, required, unique per user/tasktype
- Description: 0-300 chars, optional
- DueDate: Must be >= CreationDate
- CreationDate: Cannot be in future
- Status: NotStarted, InProgress, or Completed

### TaskType
- Name: 1-30 chars, required, unique per user
- Description: 0-150 chars, optional
- CreationDate: Cannot be in future
- UserID: Must reference valid user

---

## 👨‍💻 Author

Developed by Moussa

---

## 📄 License

This project is open-source and available for learning purposes.

---

## 📝 Notes

This project demonstrates professional backend development practices:
- Clean code principles
- SOLID design patterns
- Proper error handling
- Comprehensive validation
- Resource management
- Security best practices
