# 📝 DoThem - Task Management System

> A professional-grade **Task Management System** built with pure ADO.NET and layered architecture. No ORM, full control, production-ready backend.

[![C#](https://img.shields.io/badge/C%23-9.0%2B-blue?logo=c-sharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![.NET](https://img.shields.io/badge/.NET-10.0-purple?logo=.net)](https://dotnet.microsoft.com/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2019%2B-red?logo=microsoft-sql-server)](https://www.microsoft.com/sql-server)
[![License](https://img.shields.io/badge/license-MIT-green)](LICENSE)
[![Status](https://img.shields.io/badge/status-Active-brightgreen)](https://github.com)

---

## 🎯 Project Vision

**DoThem** is a backend-focused project that demonstrates professional software engineering practices. Built without Entity Framework Core, it showcases:

- Clean Architecture principles
- SOLID design patterns  
- Professional error handling
- Security best practices
- Advanced ADO.NET techniques

Perfect for learning or as a foundation for a production REST API.

---

## 🚀 Features

### 👤 **User Management**
- ✅ User registration with SHA256 password hashing
- ✅ Secure login with credential validation
- ✅ User profile management
- ✅ Password change functionality
- ✅ User status management (Active/Expired/Banned)
- ✅ Account deletion

### 📌 **Task Management**
- ✅ Create, Read, Update, Delete tasks
- ✅ Rich task metadata (title, description, due dates)
- ✅ Task status tracking (NotStarted/InProgress/Completed)
- ✅ Filter tasks by user and task type
- ✅ Duplicate prevention
- ✅ Comprehensive validation

### 🏷️ **Task Types**
- ✅ User-defined task types (unlimited)
- ✅ Task type CRUD operations
- ✅ Organizational flexibility
- ✅ Type-based task filtering

### 🔒 **Security**
- ✅ Parameterized SQL queries (SQL injection prevention)
- ✅ SHA256 password hashing
- ✅ User data isolation
- ✅ Input validation at all layers
- ✅ Resource management with using statements

---

## 🏗️ Architecture

### Layered Architecture Pattern

```
┌──────────────────────────────────────────────┐
│                  APP LAYER                   │
│          (Console Testing Interface)         │
│                 IAppTester                   │
└──────────────────────────────────────────────┘
                       ↓
┌──────────────────────────────────────────────┐
│               SERVICES LAYER                 │
│        (Business Logic & Validation)         │
│  • UserService          • TaskTypeService    │
│  • TaskItemService                           │
└──────────────────────────────────────────────┘
                       ↓
┌──────────────────────────────────────────────┐
│            INFRASTRUCTURE LAYER              │
│          (Data Access with ADO.NET)          │
│  • UserRepository       • TaskTypeRepository │
│  • TaskItemRepository                        │
└──────────────────────────────────────────────┘
                       ↓
┌──────────────────────────────────────────────┐
│               DOMAIN LAYER                   │
│              (Core Entities)                 │
│  • User • TaskItem • TaskType                │
└──────────────────────────────────────────────┘
                       ↓
┌──────────────────────────────────────────────┐
│         SQL SERVER DATABASE                  │
│  • Users • Tasks • TaskTypes                 │
└──────────────────────────────────────────────┘
```

### Design Patterns Used

| Pattern | Implementation | Purpose |
|---------|----------------|---------|
| **Repository** | `IUserRepository`, `ITaskItemRepository`, `ITaskTypeRepository` | Abstraction of data access |
| **Service** | `IUserService`, `ITaskItemService`, `ITaskTypeService` | Business logic layer |
| **Dependency Injection** | Constructor-based DI | Loose coupling, testability |
| **Factory** | Object creation in services | Complex object initialization |
| **Validation** | Domain and Service layers | Consistent validation rules |

---

## 📦 Project Structure

```
DoThem/
│
├── DoThem.Domain/                          # Core Domain Models
│   ├── User.cs                             # User entity with validation
│   ├── TaskItem.cs                         # Task entity with business rules
│   └── TaskType.cs                         # TaskType entity
│
├── DoThem.Infrastructure/                  # Data Access Layer (ADO.NET)
│   ├── IUserRepository.cs                  # User repository contract
│   ├── UserRepository.cs                   # User ADO.NET implementation
│   ├── ITaskItemRepository.cs              # Task repository contract
│   ├── TaskItemRepository.cs               # Task ADO.NET implementation
│   ├── ITaskTypeRepository.cs              # TaskType repository contract
│   └── TaskTypeRepository.cs               # TaskType ADO.NET implementation
│
├── DoThem.Services/                        # Business Logic Layer
│   ├── IUserService.cs                     # User service contract
│   ├── UserService.cs                      # User business logic
│   ├── ITaskItemService.cs                 # Task service contract
│   ├── TaskItemService.cs                  # Task business logic
│   ├── ITaskTypeService.cs                 # TaskType service contract
│   └── TaskTypeService.cs                  # TaskType business logic
│
├── DoThem.App/                             # Presentation/Testing Layer
│   ├── IAppTester.cs                       # Comprehensive testing interface
│   └── Program.cs                          # Console application entry point
│
└── README.md                               # Project documentation
```

---

## 🗄️ Database Schema

### Users Table
```sql
CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(100) NOT NULL UNIQUE,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Password NVARCHAR(64) NOT NULL,          -- SHA256 hashed
    CreationDate DATETIME NOT NULL DEFAULT GETDATE(),
    Status INT NOT NULL DEFAULT 1            -- 1=Active, 2=Expired, 3=Banned
)
```

### TaskTypes Table
```sql
CREATE TABLE TaskTypes (
    TaskTypeID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT NOT NULL FOREIGN KEY REFERENCES Users(UserID),
    Name NVARCHAR(30) NOT NULL,
    Description NVARCHAR(150),
    CreationDate DATETIME NOT NULL DEFAULT GETDATE()
    CONSTRAINT UQ_TaskType_Name_User UNIQUE(Name, UserID)
)
```

### Tasks Table
```sql
CREATE TABLE Tasks (
    TaskID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT NOT NULL FOREIGN KEY REFERENCES Users(UserID),
    TaskTypeID INT NOT NULL FOREIGN KEY REFERENCES TaskTypes(TaskTypeID),
    Title NVARCHAR(100) NOT NULL,
    Description NVARCHAR(300),
    CreationDate DATETIME NOT NULL,
    DueDate DATETIME,
    Status INT NOT NULL DEFAULT 1             -- 1=NotStarted, 2=InProgress, 3=Completed
    CONSTRAINT UQ_Task_Title_User_Type UNIQUE(Title, UserID, TaskTypeID)
)
```

---

## 🎯 Validation Rules

### User Entity
| Field | Rules | Example |
|-------|-------|---------|
| **Username** | 1-100 chars, no spaces, unique | `john_doe` |
| **Email** | Must end with @gmail.com, unique, no spaces | `john@gmail.com` |
| **Password** | 1-50 chars, hashed as SHA256 | `SecurePass123` |
| **CreationDate** | Cannot be in future | Auto: Today |

### TaskItem Entity
| Field | Rules | Example |
|-------|-------|---------|
| **Title** | 1-100 chars, unique per user/type | `Complete Report` |
| **Description** | 0-300 chars, optional | `Detailed description` |
| **DueDate** | Must be >= CreationDate | `2026-06-30` |
| **CreationDate** | Cannot be in future | Auto: Today |
| **Status** | 1=NotStarted, 2=InProgress, 3=Completed | `1` |

### TaskType Entity
| Field | Rules | Example |
|-------|-------|---------|
| **Name** | 1-30 chars, unique per user | `Work` |
| **Description** | 0-150 chars, optional | `Work-related tasks` |
| **CreationDate** | Cannot be in future | Auto: Today |

---

## 🔐 Security Features

### Authentication & Data Protection
```csharp
// Password Hashing Example
string hashed = HashPassword("MyPassword123"); 
// Uses SHA256.Create() for cryptographic hashing

// Stored in database as Base64 encoded SHA256 hash
// Never stored as plain text
```

### SQL Injection Prevention
```csharp
// ✅ SAFE: Parameterized Query
command.Parameters.AddWithValue("@Username", username);
string query = "SELECT * FROM Users WHERE Username = @Username";

// ❌ UNSAFE: String Concatenation (Not used in project)
string query = $"SELECT * FROM Users WHERE Username = '{username}'";
```

### Input Validation Example
```csharp
// Multi-layer validation
if (string.IsNullOrWhiteSpace(username))
    throw new ArgumentNullException("Username cannot be empty");
if (username.Length > 100)
    throw new ArgumentException("Username too long");
if (username.Any(char.IsWhiteSpace))
    throw new ArgumentException("Username cannot have spaces");
```

---

## 🧪 Testing Interface (IAppTester)

The project includes a comprehensive testing interface for validating all functionality:

### Test Categories

**User Operations** (7 tests)
```csharp
void TestUserRegistration();        // Create user
void TestUserLogin();               // Authenticate
void TestFindUserByID();            // Retrieve user
void TestUpdateUser();              // Modify user
void TestDeleteUser();              // Remove user
void TestGetAllUsers();             // List all users
void TestCompleteUserWorkflow();    // End-to-end test
```

**Task Operations** (8 tests)
```csharp
void TestAddTask();                        // Create task
void TestFindTaskByID();                   // Retrieve task
void TestUpdateTask();                     // Modify task
void TestDeleteTask();                     // Remove task
void TestGetAllTasks();                    // List all tasks
void TestGetTasksByUserID();               // Filter by user
void TestGetTasksByUserAndType();          // Filter by user & type
void TestCompleteTaskWorkflow();           // End-to-end test
```

**TaskType Operations** (7 tests)
```csharp
void TestAddTaskType();              // Create type
void TestFindTaskTypeByID();         // Retrieve type
void TestUpdateTaskType();           // Modify type
void TestDeleteTaskType();           // Remove type
void TestGetAllTaskTypes();          // List all types
void TestGetTaskTypesByUserID();     // Filter by user
void TestCompleteTaskTypeWorkflow(); // End-to-end test
```

**Full Suite**
```csharp
void RunAllTests();  // Execute complete test suite
```

---

## 🚀 Quick Start

### Prerequisites
- .NET 10.0+
- SQL Server 2019+
- Visual Studio 2022+ (optional)

### Installation

1. **Clone Repository**
```bash
git clone https://github.com/yourusername/DoThem.git
cd DoThem
```

2. **Update Connection String**
Edit the connection string in your app configuration:
```csharp
string connectionString = "Server=YOUR_SERVER;Database=DoThem;Integrated Security=true;";
```

3. **Create Database**
Execute the SQL scripts to create tables:
```sql
-- Run all CREATE TABLE statements from Database Schema section
```

4. **Build Project**
```bash
dotnet build
```

5. **Run Tests**
```bash
dotnet run --project DoThem.App
```

---

## 💡 Why ADO.NET Instead of Entity Framework?

| Aspect | ADO.NET | EF Core |
|--------|---------|---------|
| **Query Control** | ✅ Full control | Limited to LINQ |
| **Performance** | ✅ Optimized | Overhead |
| **Learning Value** | ✅ Deep understanding | Abstraction hides details |
| **SQL Expertise** | ✅ Required | Not necessary |
| **Simple Projects** | ✅ Perfect | Overkill |

**Decision:** This project prioritizes learning and control over convenience.

---

## 📊 Code Metrics

| Metric | Value | Notes |
|--------|-------|-------|
| **Lines of Code** | ~3,500 | Core business logic |
| **Classes** | 12 | 6 repositories, 6 services |
| **Interfaces** | 6 | Abstraction layer |
| **Methods** | 50+ | Comprehensive coverage |
| **Validation Points** | 100+ | Multi-layer validation |

---

## 🎓 Learning Outcomes

This project teaches:

1. **ADO.NET Mastery**
   - Connection pooling
   - Command execution
   - Data readers
   - Parameter binding
   - Transaction management

2. **Clean Architecture**
   - Layer separation
   - Dependency injection
   - SOLID principles
   - Design patterns

3. **Security Best Practices**
   - SQL injection prevention
   - Password hashing
   - Input validation
   - Data isolation

4. **Error Handling**
   - Custom exceptions
   - Validation errors
   - Database exceptions
   - Resource cleanup

5. **Database Design**
   - Schema design
   - Relationships
   - Constraints
   - Indexing

## 🤝 Contributing

Contributions are welcome! Please follow these guidelines:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add AmazingFeature'`)
4. Push to branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Code Standards
- Follow C# naming conventions
- Use meaningful variable names
- Add XML documentation for public methods
- Write clean, readable code
- Maintain layer separation

---

## 📝 Usage Example

### Creating a Task
```csharp
// Create repository and service
var repository = new TaskItemRepository(connectionString);
var service = new TaskItemService(repository);

// Create task
var task = new TaskItem(
    TaskID: 0,
    UserID: 1,
    Title: "Complete Report",
    Description: "Quarterly business report",
    TaskTypeId: 1,
    CreationDate: DateTime.Now,
    DueDate: DateTime.Now.AddDays(7),
    Status: TaskItem.TaskStatus.NotStarted
);

// Add task (validates and stores in database)
int? taskId = service.AddTask(task);
```

### Retrieving Tasks
```csharp
// Get all tasks for user
var userTasks = service.GetTasks(userId: 1);

// Get tasks by user and type
var workTasks = service.GetTasks(userId: 1, taskTypeID: 1);

// Find specific task
var task = service.FindTask(taskId: 5);
```

---

## 🐛 Troubleshooting

### Connection Issues
- Verify SQL Server is running
- Check connection string
- Confirm database exists
- Verify user permissions

### Validation Errors
- Check email format (@gmail.com)
- Verify string lengths
- Ensure dates are valid
- Check for duplicate entries

### Null Reference Exceptions
- Use proper null checking
- Verify IDs exist before operations
- Check return values from Find methods

## 📄 License

This project is licensed under the **MIT License** - see the LICENSE file for details.

```
MIT License

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
```

---

## 👨‍💻 Author

**Moussa Kharbouch**
- GitHub: [@MoussaKharbouch](https://github.com/MoussaKharbouch)
- Email: moussa5arbouch@gmail.com

## 📊 Project Statistics

![GitHub stars](https://img.shields.io/github/stars/MoussaKharbouch/DoThem?style=social)
![GitHub forks](https://img.shields.io/github/forks/MoussaKharbouch/DoThem?style=social)
![GitHub watchers](https://img.shields.io/github/watchers/MoussaKharbouch/DoThem?style=social)

**Last Updated:** June 9, 2026  
**Version:** 1.0.0  
**Status:** Active Development

---

<div align="center">

### ⭐ If you find this project helpful, please give it a star!

**Happy Coding! 🚀**

</div>
