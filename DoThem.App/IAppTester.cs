using System;
using DoThem.Domain;
using DoThem.Infrastructure;
using DoThem.Services;

namespace DoThem.App;

/// <summary>
/// Testing interface for all layers - provides comprehensive testing methods
/// for User, TaskItem, and TaskType modules
/// </summary>
public interface IAppTester
{

    #region User Testing

    /// <summary>
    /// Test user registration
    /// </summary>
    void TestUserRegistration();

    /// <summary>
    /// Test user login
    /// </summary>
    void TestUserLogin();

    /// <summary>
    /// Test finding user by ID
    /// </summary>
    void TestFindUserByID();

    /// <summary>
    /// Test user update
    /// </summary>
    void TestUpdateUser();

    /// <summary>
    /// Test user deletion
    /// </summary>
    void TestDeleteUser();

    /// <summary>
    /// Test getting all users
    /// </summary>
    void TestGetAllUsers();

    #endregion

    #region TaskItem Testing

    /// <summary>
    /// Test creating a new task
    /// </summary>
    void TestAddTask();

    /// <summary>
    /// Test finding task by ID
    /// </summary>
    void TestFindTaskByID();

    /// <summary>
    /// Test finding task by name, user ID, and task type ID
    /// </summary>
    void TestFindTaskByNameUserType();

    /// <summary>
    /// Test updating a task
    /// </summary>
    void TestUpdateTask();

    /// <summary>
    /// Test deleting a task
    /// </summary>
    void TestDeleteTask();

    /// <summary>
    /// Test getting all tasks
    /// </summary>
    void TestGetAllTasks();

    /// <summary>
    /// Test getting tasks by user ID
    /// </summary>
    void TestGetTasksByUserID();

    /// <summary>
    /// Test getting tasks by user ID and task type ID
    /// </summary>
    void TestGetTasksByUserAndType();

    #endregion

    #region TaskType Testing

    /// <summary>
    /// Test creating a new task type
    /// </summary>
    void TestAddTaskType();

    /// <summary>
    /// Test finding task type by ID
    /// </summary>
    void TestFindTaskTypeByID();

    /// <summary>
    /// Test finding task type by name and user ID
    /// </summary>
    void TestFindTaskTypeByNameAndUser();

    /// <summary>
    /// Test updating a task type
    /// </summary>
    void TestUpdateTaskType();

    /// <summary>
    /// Test deleting a task type
    /// </summary>
    void TestDeleteTaskType();

    /// <summary>
    /// Test getting all task types
    /// </summary>
    void TestGetAllTaskTypes();

    /// <summary>
    /// Test getting task types by user ID
    /// </summary>
    void TestGetTaskTypesByUserID();

    #endregion

    #region Integration Testing

    /// <summary>
    /// Test complete user workflow (create, find, update, delete)
    /// </summary>
    void TestCompleteUserWorkflow();

    /// <summary>
    /// Test complete task workflow (create, find, update, delete)
    /// </summary>
    void TestCompleteTaskWorkflow();

    /// <summary>
    /// Test complete task type workflow (create, find, update, delete)
    /// </summary>
    void TestCompleteTaskTypeWorkflow();

    /// <summary>
    /// Run all tests
    /// </summary>
    void RunAllTests();

    #endregion

}
