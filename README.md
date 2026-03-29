# DoThem 📝

A simple **Task Management System** that allows users to manage their personal tasks.

---

## 🚀 Project Overview

DoThem is a backend-focused project built using:

* **C#**
* **ADO.NET**
* **SQL Server**

The goal is to implement a clean and structured backend architecture without relying on ORMs like EF Core.

---

## ⭐ Key Highlights

- Clean layered architecture
- Built with pure ADO.NET (no ORM)
- Strong input validation in domain models
- Separation of concerns (Repository / Service)

---

## 🧠 Features

### 👤 User Management

* Register new account
* Login securely
* Update user information
* Delete account

### ✅ Task Management

* Create tasks
* View personal tasks
* Update tasks
* Delete tasks

### 🏷️ Task Types

* Users can define custom task types
* No restriction on number of types

### 🔄 Task Status

Each task can have one of the following:

* Pending
* In Progress
* Completed

---

## 🏗️ Architecture

The project follows a layered architecture:

```
Domain         → Core entities (User, TaskItem)
Infrastructure → Database access (ADO.NET)
Services       → Business logic
App            → Console testing (Program.cs)
```

---

## 🔐 System Rules

* Users can only access their own tasks
* No access to other users' data
* No roles (simple user system)
* No task priorities (for now)

---

## ⚙️ Current Status

🚧 **Under Development**

### ✅ Completed
- User module (Repository + Service)
- Password hashing & validation
- Console testing

### 🔄 In Progress
- Task module

### ⏳ Planned

* Task Types module
* REST API (possibly using ASP.NET)
* Frontend integration

---

## 💡 Why ADO.NET?

This project intentionally uses **ADO.NET** instead of EF Core to:

* Gain full control over SQL queries
* Better understand database interactions
* Build a strong backend foundation

---

## 🧪 Testing

Currently tested using a console application (`Program.cs`) with multiple scenarios.

---

## 📌 Future Improvements

* Add API layer (ASP.NET Core)
* Implement authentication (JWT)
* Improve error handling
* Add logging
* Write unit tests
* Build simple frontend

---

## 👨‍💻 Author

Developed by Moussa

---

## 📄 License

This project is open-source and available for learning purposes.
