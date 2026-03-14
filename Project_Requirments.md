# Task Management System – Project Requirements

## 1. Project Description
A simple task management system that allows users to create accounts and manage their personal tasks.  
Each user can create, view, update, and delete their own tasks. Users can only see tasks that belong to them.

---

## 2. Users

The system contains only **one type of user**: a normal user.

### User Data
Each user includes:

- Username
- Email
- Password
- Account creation date

### User Permissions
A user can:

- Create an account
- Log in
- View their tasks
- Add tasks
- Edit tasks
- Delete tasks

A user **cannot**:

- View tasks belonging to other users
- Modify or delete other users

---

## 3. Tasks

A task represents an activity or work item created by a user.

### Task Data
Each task includes:

- Title
- Description
- Type (user-defined, not limited)
- Status
- Creation date
- Owner (the user who created the task)

---

## 4. Task Types

Task types are **not restricted**.  
The user can define any type they want.

Examples:

- Study
- Work
- Personal
- Project
- Sport

There is **no limit** to task types.

---

## 5. Task Status

Each task must have one of the following statuses:

- Pending
- In Progress
- Completed

---

## 6. Functional Requirements

### Account Creation
The system must allow a user to:

- Create a new account
- Provide username
- Provide email
- Provide password

### Login
The system must allow a user to:

- Log in using email and password

### View Tasks
The user must be able to:

- See all their tasks
- View task title
- View task type
- View task status
- View creation date

### Add Task
The user must be able to:

- Create a new task
- Enter title
- Enter description
- Enter task type
- Set task status

### Edit Task
The user must be able to:

- Edit task title
- Edit task description
- Edit task type
- Change task status

### Delete Task
The user must be able to:

- Delete any task they own

---

## 7. System Constraints

The system must enforce the following rules:

- Users can only see their own tasks
- Users cannot access other users’ tasks
- Users cannot delete or modify other users
- The system does not include roles
- The system does not include task priorities