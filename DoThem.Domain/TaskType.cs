using System;
using System.Linq;

namespace DoThem.Domain;

/// <summary>
/// The TaskType class represents a task type in the system.
/// It has properties such as TaskTypeID, Name, Description, CreationDate, and UserID.
/// The Name property is required and cannot be empty or longer than 30 characters.
/// The Description property is optional but cannot be longer than 150 characters.
/// The CreationDate property is a DateTime that represents the date and time when the task type was created.
/// It cannot be in the future.
/// The UserID property is an integer that references the user who created this task type.
/// </summary>
public class TaskType
{

    public int TaskTypeID { get; set; }

    public int UserID { get; set; }

    private string _Name = string.Empty;
    public string Name
    {

        get { return _Name; }

        set
        {

            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("Name cannot be empty or null.");
            if (value.Length > 30)
                throw new ArgumentException("Name cannot be longer than 30 characters.");

            _Name = value;

        }

    }

    private string _Description = string.Empty;
    public string Description
    {

        get { return _Description; }

        set
        {

            if (value.Length > 150)
                throw new ArgumentException("Description cannot be longer than 150 characters.");

            _Description = value;

        }

    }

    private DateTime _CreationDate;

    public DateTime CreationDate
    {

        get { return _CreationDate; }

        private set
        {

            if (value > DateTime.UtcNow)
                throw new ArgumentException("Creation date cannot be in the future.");

            _CreationDate = value;

        }

    }

    public TaskType(int TaskTypeID, int UserID, string Name, string Description, DateTime CreationDate)
    {
        this.TaskTypeID = TaskTypeID;
        this.UserID = UserID;
        this.Name = Name;
        this.Description = Description;
        this.CreationDate = CreationDate;
    }

}
