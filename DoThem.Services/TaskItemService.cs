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

    bool ValidateTask(TaskItem task)
    {

        // we can add more validation rules here if needed
        if (string.IsNullOrEmpty(task.Title))
            return false;
        if (task.Title.Length > 100)
            return false;
        if (task.Description.Length > 300)
            return false;
        if (task.CreationDate > DateTime.Now)
            return false;
        if (task.DueDate < task.CreationDate)
            return false;

        return true;

    }

    public TaskItem? FindTask(int taskID)
    {
        if (taskID < 0)
            throw new ArgumentException("Task ID cannot be negative.");
        return taskItemRepository.FindTask(taskID);
    }

    public TaskItem? FindTask(string taskName, int userID, int taskTypeID)
    {
        throw new NotImplementedException();
    }

    public bool DoesTaskExist(int taskID)
    {
        throw new NotImplementedException();
    }

    public bool DoesTaskExist(string taskName, int userID, int taskTypeID)
    {
        throw new NotImplementedException();
    }

    public int? AddTask(TaskItem task)
    {
        throw new NotImplementedException();
    }

    public bool UpdateTask(int taskID, TaskItem newTask)
    {
        throw new NotImplementedException();
    }

    public bool DeleteTask(int taskID)
    {
        throw new NotImplementedException();
    }

    public List<TaskItem> GetTasks()
    {
        throw new NotImplementedException();
    }

    public List<TaskItem> GetTasks(int userID)
    {
        throw new NotImplementedException();
    }

    public List<TaskItem> GetTasks(int userID, int taskTypeID)
    {
        throw new NotImplementedException();
    }

}
