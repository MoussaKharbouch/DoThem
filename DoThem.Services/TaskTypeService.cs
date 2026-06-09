using System;
using System.Collections.Generic;
using DoThem.Domain;
using DoThem.Infrastructure;

namespace DoThem.Services;

public class TaskTypeService : ITaskTypeService
{

    private readonly ITaskTypeRepository taskTypeRepository;

    public TaskTypeService(ITaskTypeRepository taskTypeRepository)
    {
        this.taskTypeRepository = taskTypeRepository;
    }

    private void ValidateTaskTypeFields(TaskType taskType)
    {

        // validate name
        if (string.IsNullOrWhiteSpace(taskType.Name))
            throw new ArgumentNullException("Name cannot be empty or null.");
        if (taskType.Name.Length > 30)
            throw new ArgumentException("Name cannot be longer than 30 characters.");

        // validate description
        if (taskType.Description.Length > 150)
            throw new ArgumentException("Description cannot be longer than 150 characters.");

        // validate creation date
        if (taskType.CreationDate > DateTime.Now)
            throw new ArgumentException("Creation date cannot be in the future.");

    }

    public TaskType? FindTaskType(int taskTypeID)
    {
        if (taskTypeID < 0)
            throw new ArgumentOutOfRangeException("Task type ID cannot be negative.");
        return taskTypeRepository.FindTaskType(taskTypeID);
    }

    public TaskType? FindTaskType(string name, int userID)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException("Name cannot be empty or null.");
        if (userID < 0)
            throw new ArgumentOutOfRangeException("User ID cannot be negative.");
        return taskTypeRepository.FindTaskType(name, userID);
    }

    public bool DoesTaskTypeExist(int taskTypeID)
    {
        if (taskTypeID < 0)
            throw new ArgumentOutOfRangeException("Task type ID cannot be negative.");
        return taskTypeRepository.DoesTaskTypeExist(taskTypeID);
    }

    public bool DoesTaskTypeExist(string name, int userID)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException("Name cannot be empty or null.");
        if (userID < 0)
            throw new ArgumentOutOfRangeException("User ID cannot be negative.");
        return taskTypeRepository.DoesTaskTypeExist(name, userID);
    }

    public int? AddTaskType(TaskType taskType)
    {

        if (taskType == null)
            throw new ArgumentNullException("Task type cannot be null.");

        if (taskType.UserID < 0)
            throw new ArgumentOutOfRangeException("User ID cannot be negative.");

        ValidateTaskTypeFields(taskType);

        // check if task type already exists with same name and user id
        if (taskTypeRepository.DoesTaskTypeExist(taskType.Name, taskType.UserID))
            throw new ArgumentException("Task type with the same name already exists for this user.");

        return taskTypeRepository.AddTaskType(taskType);

    }

    public bool UpdateTaskType(int taskTypeID, TaskType newTaskType)
    {

        if (taskTypeID < 0)
            throw new ArgumentOutOfRangeException("Task type ID cannot be negative.");

        if (newTaskType == null)
            throw new ArgumentNullException("Task type cannot be null.");

        if (newTaskType.UserID < 0)
            throw new ArgumentOutOfRangeException("User ID cannot be negative.");

        ValidateTaskTypeFields(newTaskType);

        // check if task type exists first
        TaskType? oldTaskType = FindTaskType(taskTypeID);
        if (oldTaskType == null)
            throw new ArgumentException("Task type with the given ID does not exist.");

        // check if another task type with same name already exists for this user
        if (oldTaskType.Name != newTaskType.Name || oldTaskType.UserID != newTaskType.UserID)
        {
            if (taskTypeRepository.DoesTaskTypeExist(newTaskType.Name, newTaskType.UserID))
                throw new ArgumentException("Task type with the same name already exists for this user.");
        }

        return taskTypeRepository.UpdateTaskType(taskTypeID, newTaskType);

    }

    public bool DeleteTaskType(int taskTypeID)
    {
        if (taskTypeID < 0)
            throw new ArgumentOutOfRangeException("Task type ID cannot be negative.");
        return taskTypeRepository.DeleteTaskType(taskTypeID);
    }

    public List<TaskType> GetTaskTypes()
    {
        return taskTypeRepository.GetTaskTypes();
    }

    public List<TaskType> GetTaskTypes(int userID)
    {
        if (userID < 0)
            throw new ArgumentOutOfRangeException("User ID cannot be negative.");
        return taskTypeRepository.GetTaskTypes(userID);
    }

}
