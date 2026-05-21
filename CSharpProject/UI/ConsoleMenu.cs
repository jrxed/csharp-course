public static class ConsoleMenu
{
    public static void PrintHeader(string title)
    {
        Console.WriteLine();
        Console.WriteLine(new string('=', 50));
        Console.WriteLine($"  {title}");
        Console.WriteLine(new string('=', 50));
    }

    public static void PrintTask(TaskItem task)
    {
        var overdue = task.IsOverdue ? " [ПРОСРОЧЕНА]" : "";
        Console.ForegroundColor = task.Priority == Priority.High ? ConsoleColor.Red
            : task.Priority == Priority.Medium ? ConsoleColor.Yellow
            : ConsoleColor.Gray;
        Console.WriteLine($"  [{task.Id}] {task.Title}{overdue}");
        Console.ResetColor();
        Console.WriteLine($"      Описание : {task.Description}");
        Console.WriteLine($"      Приоритет: {TaskItem.PriorityLabel(task.Priority)}");
        Console.WriteLine($"      Статус   : {TaskItem.StatusLabel(task.Status)}");
        Console.WriteLine($"      Дедлайн  : {task.DisplayDeadline:dd.MM.yyyy}");
        Console.WriteLine();
    }

    public static void PrintTaskList(IReadOnlyList<TaskItem> tasks, string header)
    {
        PrintHeader(header);
        if (tasks.Count == 0)
        {
            Console.WriteLine("  Задачи не найдены.");
            return;
        }
        foreach (var task in tasks)
            PrintTask(task);
    }

    public static int ReadChoice(int min, int max)
    {
        while (true)
        {
            Console.Write($"Выберите [{min}-{max}]: ");
            var input = Console.ReadLine()?.Trim();
            if (int.TryParse(input, out int choice) && choice >= min && choice <= max)
                return choice;
            Console.WriteLine("  Неверный ввод. Попробуйте ещё раз.");
        }
    }

    public static string ReadNonEmpty(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(input))
                return input;
            Console.WriteLine("  Поле не может быть пустым.");
        }
    }

    public static string ReadLine(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine()?.Trim() ?? string.Empty;
    }

    public static Priority ReadPriority()
    {
        Console.WriteLine("  Приоритет: 1 - Низкий, 2 - Средний, 3 - Высокий");
        return ReadChoice(1, 3) switch
        {
            1 => Priority.Low,
            2 => Priority.Medium,
            3 => Priority.High,
            _ => Priority.Low
        };
    }

    public static WorkStatus ReadStatus()
    {
        Console.WriteLine("  Статус: 1 - Новая, 2 - В работе, 3 - Выполнена");
        return ReadChoice(1, 3) switch
        {
            1 => WorkStatus.New,
            2 => WorkStatus.InProgress,
            3 => WorkStatus.Done,
            _ => WorkStatus.New
        };
    }

    public static DateTime ReadDeadline()
    {
        while (true)
        {
            Console.Write("  Дедлайн (дд.мм.гггг): ");
            var input = Console.ReadLine()?.Trim();
            if (DateTime.TryParseExact(input, "dd.MM.yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out var date))
                return date.AddDays(1);
            Console.WriteLine("  Неверный формат даты. Используйте дд.мм.гггг");
        }
    }

    public static void Pause()
    {
        Console.WriteLine("\nНажмите Enter для продолжения...");
        Console.ReadLine();
    }
}
