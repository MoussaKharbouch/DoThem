using System;
using DoThem.Domain;

namespace DoThem.Infrastructure;

public interface ITaskItemRepository
{

    /// <summary>
    /// find task by task id
    /// </summary>
    TaskItem? FindTask(int taskID);

    /// <summary>
    /// find task by task name and user id and task type id
    /// </summary>
    TaskItem? FindTask(string taskName, int userID, int taskTypeID);

    /// <summary>
    /// check if task exists by task id
    /// </summary>
    bool DoesTaskExist(int taskID);

    /// <summary>
    /// check if task exists by task name and user id and task type id
    /// </summary>
    bool DoesTaskExist(string taskName, int userID, int taskTypeID);

    /// <summary>
    /// add new task
    /// </summary>
    /// <returns>
    /// the id of the new task
    /// </returns>
    int? AddTask(TaskItem task);

    /// <summary>
    /// update task
    /// </summary>
    bool UpdateTask(int taskID, TaskItem newTask);

    /// <summary>
    /// delete task
    /// </summary>
    bool DeleteTask(int taskID);

    /// <summary>
    /// get tasks with all fields
    /// </summary>
    List<TaskItem> GetTasks();

    /// <summary>
    /// get tasks by user id
    /// </summary>
    List<TaskItem> GetTasks(int userID);

    /// <summary>
    /// get tasks by user id and task type id
    /// </summary>
    List<TaskItem> GetTasks(int userID, int taskTypeID);

}
