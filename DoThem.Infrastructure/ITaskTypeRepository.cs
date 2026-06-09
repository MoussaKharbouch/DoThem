using System;
using System.Collections.Generic;
using DoThem.Domain;

namespace DoThem.Infrastructure;

public interface ITaskTypeRepository
{

    /// <summary>
    /// find task type by task type id
    /// </summary>
    TaskType? FindTaskType(int taskTypeID);

    /// <summary>
    /// find task type by name and user id
    /// </summary>
    TaskType? FindTaskType(string name, int userID);

    /// <summary>
    /// check if task type exists by task type id
    /// </summary>
    bool DoesTaskTypeExist(int taskTypeID);

    /// <summary>
    /// check if task type exists by name and user id
    /// </summary>
    bool DoesTaskTypeExist(string name, int userID);

    /// <summary>
    /// add new task type
    /// </summary>
    /// <returns>
    /// the id of the new task type
    /// </returns>
    int? AddTaskType(TaskType taskType);

    /// <summary>
    /// update task type
    /// </summary>
    bool UpdateTaskType(int taskTypeID, TaskType newTaskType);

    /// <summary>
    /// delete task type
    /// </summary>
    bool DeleteTaskType(int taskTypeID);

    /// <summary>
    /// get task types with all fields
    /// </summary>
    List<TaskType> GetTaskTypes();

    /// <summary>
    /// get task types by user id
    /// </summary>
    List<TaskType> GetTaskTypes(int userID);

}
