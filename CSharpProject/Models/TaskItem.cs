public class TaskItem
{
    public int Id { get; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Priority Priority { get; set; }
    public DateTime Deadline { get; set; }
    public WorkStatus Status { get; set; }

    public TaskItem(int id, string title, string description, Priority priority, DateTime deadline, WorkStatus status = WorkStatus.New)
    {
        Id = id;
        Title = title;
        Description = description;
        Priority = priority;
        Deadline = deadline;
        Status = status;
    }

    public bool IsOverdue => Status != WorkStatus.Done && Deadline < DateTime.Now;

    // Дедлайн хранится как начало следующего дня; для отображения возвращаем исходный день
    public DateTime DisplayDeadline => Deadline.AddDays(-1);

    public override string ToString() =>
        $"[{Id}] {Title} | {PriorityLabel(Priority)} | {StatusLabel(Status)} | до {DisplayDeadline:dd.MM.yyyy}";

    public static string PriorityLabel(Priority p) => p switch
    {
        Priority.Low => "Низкий",
        Priority.Medium => "Средний",
        Priority.High => "Высокий",
        _ => p.ToString()
    };

    public static string StatusLabel(WorkStatus s) => s switch
    {
        WorkStatus.New => "Новая",
        WorkStatus.InProgress => "В работе",
        WorkStatus.Done => "Выполнена",
        _ => s.ToString()
    };
}
