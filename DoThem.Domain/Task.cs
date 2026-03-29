using System;

namespace DoThem.Domain;

/// <summary>
/// The Task class represents a task in the system.
/// It has properties such as Title, Description, TaskTypeId, CreationDate, DueDate, and TaskStatus.
/// The Title property is required and cannot be empty or longer than 100 characters.
/// The Description property is optional but cannot be longer than 300 characters.
/// The TaskTypeId property is an integer that represents the type of the task.
/// The CreationDate property is a DateTime that represents the date and time when the task was created. It cannot be in the future.
/// The DueDate property is a DateTime that represents the date and time when the task is due. It cannot be before the CreationDate.
/// The TaskStatus property is an enum that represents the status of the task (NotStarted, InProgress, Completed).
/// </summary>
public class Task
{

    public int TaskId { get; set; }

    private string _Title = string.Empty;
    public string Title
    {

        get
        {
            return _Title;
        }
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Title cannot be empty");
            if (value.Length > 100)
                throw new ArgumentException("Title cannot be longer than 100 characters");
            _Title = value;
        }

    }

    private string _Description = string.Empty;
    public string Description
    {

        get { return _Description; }

        set
        {
            if (value.Length > 300)
                throw new ArgumentException("Description cannot be longer than 300 characters");
            _Description = value;
        }

    }

    public int TaskTypeId { get; set; }

    private DateTime _CreationDate;
    public DateTime CreationDate
    {

        get { return _CreationDate; }

        set
        {
            if (value > DateTime.Now)
                throw new ArgumentException("Creation date cannot be in the future.");
            _CreationDate = value;
        }
        
    }

    private DateTime _DueDate;
    public DateTime DueDate
    {

        get { return _DueDate; }

        set
        {
            if (value < CreationDate)
                throw new ArgumentException("Due date cannot be before creation date.");
            _DueDate = value;
        }

    }

    public enum TaskStatus { NotStarted = 1, InProgress = 2, Completed = 3 }

    public Task(int TaskId, string Title, string Description, int TaskTypeId, DateTime CreationDate, DateTime DueDate)
    {
        this.TaskId = TaskId;
        this.Title = Title;
        this.Description = Description;
        this.TaskTypeId = TaskTypeId;
        this.CreationDate = CreationDate;
        this.DueDate = DueDate;
    }

}
