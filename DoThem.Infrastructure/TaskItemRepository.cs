using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Data.SqlClient;
using DoThem.Domain;

namespace DoThem.Infrastructure;

public class TaskItemRepository : ITaskItemRepository
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
