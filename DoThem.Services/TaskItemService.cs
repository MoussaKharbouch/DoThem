using System;
using System.Text;
using DoThem.Domain;
using DoThem.Infrastructure;

namespace DoThem.Services;

public class TaskItemService : ITaskItemService
{

    public TaskItem? FindTask(int taskID)
    {
        throw new NotImplementedException();
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
