using System;
using System.Text;
using DoThem.Domain;
using DoThem.Infrastructure;

namespace DoThem.Services;

public class TaskItemService : ITaskItemService
{

    private readonly ITaskItemRepository taskItemRepository;

    public TaskItemService(ITaskItemRepository taskItemRepository)
    {
        this.taskItemRepository = taskItemRepository;
    }

    private void ValidateTaskFields(TaskItem task)
    {

        // validate title
        if (string.IsNullOrWhiteSpace(task.Title))
            throw new ArgumentNullException("Title cannot be empty or null.");
        if (task.Title.Length > 100)
            throw new ArgumentException("Title cannot be longer than 100 characters.");

        // validate description
        if (task.Description.Length > 300)
            throw new ArgumentException("Description cannot be longer than 300 characters.");

        // validate creation date
        if (task.CreationDate > DateTime.Now)
            throw new ArgumentException("Creation date cannot be in the future.");

        // validate due date
        if (task.DueDate < task.CreationDate)
            throw new ArgumentException("Due date cannot be before creation date.");

    }

    public TaskItem? FindTask(int taskID)
    {
        if (taskID < 0)
            throw new ArgumentOutOfRangeException("Task ID cannot be negative.");
        return taskItemRepository.FindTask(taskID);
    }

    public TaskItem? FindTask(string taskName, int userID, int taskTypeID)
    {
        if (string.IsNullOrWhiteSpace(taskName))
            throw new ArgumentNullException("Task name cannot be empty or null.");
        if (userID < 0)
            throw new ArgumentOutOfRangeException("User ID cannot be negative.");
        if (taskTypeID < 0)
            throw new ArgumentOutOfRangeException("Task type ID cannot be negative.");
        return taskItemRepository.FindTask(taskName, userID, taskTypeID);
    }

    public bool DoesTaskExist(int taskID)
    {
        if (taskID < 0)
            throw new ArgumentOutOfRangeException("Task ID cannot be negative.");
        return taskItemRepository.DoesTaskExist(taskID);
    }

    public bool DoesTaskExist(string taskName, int userID, int taskTypeID)
    {
        if (string.IsNullOrWhiteSpace(taskName))
            throw new ArgumentNullException("Task name cannot be empty or null.");
        if (userID < 0)
            throw new ArgumentOutOfRangeException("User ID cannot be negative.");
        if (taskTypeID < 0)
            throw new ArgumentOutOfRangeException("Task type ID cannot be negative.");
        return taskItemRepository.DoesTaskExist(taskName, userID, taskTypeID);
    }

    public int? AddTask(TaskItem task)
    {

        if (task == null)
            throw new ArgumentNullException("Task cannot be null.");

        if (task.UserID < 0)
            throw new ArgumentOutOfRangeException("User ID cannot be negative.");

        ValidateTaskFields(task);

        // check if task already exists with same title, user id, and task type id
        if (taskItemRepository.DoesTaskExist(task.Title, task.UserID, task.TaskTypeId))
            throw new ArgumentException("Task with the same name already exists for this user and task type.");

        return taskItemRepository.AddTask(task);

    }

    public bool UpdateTask(int taskID, TaskItem newTask)
    {

        if (taskID < 0)
            throw new ArgumentOutOfRangeException("Task ID cannot be negative.");

        if (newTask == null)
            throw new ArgumentNullException("Task cannot be null.");

        if (newTask.UserID < 0)
            throw new ArgumentOutOfRangeException("User ID cannot be negative.");

        ValidateTaskFields(newTask);

        // check if task exists first
        TaskItem? oldTask = FindTask(taskID);
        if (oldTask == null)
            throw new ArgumentException("Task with the given ID does not exist.");

        // check if another task with same name already exists for this user and task type
        if (oldTask.Title != newTask.Title || oldTask.UserID != newTask.UserID || oldTask.TaskTypeId != newTask.TaskTypeId)
        {
            if (taskItemRepository.DoesTaskExist(newTask.Title, newTask.UserID, newTask.TaskTypeId))
                throw new ArgumentException("Task with the same name already exists for this user and task type.");
        }

        return taskItemRepository.UpdateTask(taskID, newTask);

    }

    public bool DeleteTask(int taskID)
    {
        if (taskID < 0)
            throw new ArgumentOutOfRangeException("Task ID cannot be negative.");
        return taskItemRepository.DeleteTask(taskID);
    }

    public List<TaskItem> GetTasks()
    {
        return taskItemRepository.GetTasks();
    }

    public List<TaskItem> GetTasks(int userID)
    {
        if (userID < 0)
            throw new ArgumentOutOfRangeException("User ID cannot be negative.");
        return taskItemRepository.GetTasks(userID);
    }

    public List<TaskItem> GetTasks(int userID, int taskTypeID)
    {
        if (userID < 0)
            throw new ArgumentOutOfRangeException("User ID cannot be negative.");
        if (taskTypeID < 0)
            throw new ArgumentOutOfRangeException("Task type ID cannot be negative.");
        return taskItemRepository.GetTasks(userID, taskTypeID);
    }

}
