public class TaskHubApp : IDisposable
{
    private readonly Repository<TaskItem> _repository = new();
    private readonly TaskFileService _fileService = new();
    private readonly DeadlineChecker _checker;
    private readonly string _dataFile = "tasks.csv";
    private int _nextId = 1;
    private bool _disposed = false;

    public TaskHubApp()
    {
        _checker = new DeadlineChecker(_repository, OnOverdue);
    }

    public async Task RunAsync()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.Title = "TaskHub — Менеджер задач";

        while (true)
        {
            ConsoleMenu.PrintHeader("TaskHub — Менеджер задач");
            Console.WriteLine("  1. Создать задачу");
            Console.WriteLine("  2. Просмотр задач");
            Console.WriteLine("  3. Редактировать задачу");
            Console.WriteLine("  4. Удалить задачу");
            Console.WriteLine("  5. Поиск задач");
            Console.WriteLine("  6. Статистика");
            Console.WriteLine("  7. Сохранить в файл");
            Console.WriteLine("  8. Загрузить из файла");
            Console.WriteLine("  0. Выход");
            Console.WriteLine();

            var choice = ConsoleMenu.ReadChoice(0, 8);

            switch (choice)
            {
                case 1: CreateTask(); break;
                case 2: ViewTasks(); break;
                case 3: EditTask(); break;
                case 4: DeleteTask(); break;
                case 5: SearchTasks(); break;
                case 6: ShowStatistics(); break;
                case 7: await SaveTasksAsync(); break;
                case 8: await LoadTasksAsync(); break;
                case 0: return;
            }
        }
    }

    private void CreateTask()
    {
        ConsoleMenu.PrintHeader("Создание задачи");

        var title = ConsoleMenu.ReadNonEmpty("  Название: ");
        var description = ConsoleMenu.ReadLine("  Описание: ");
        var priority = ConsoleMenu.ReadPriority();
        var deadline = ConsoleMenu.ReadDeadline();

        var task = new TaskItem(_nextId++, title, description, priority, deadline);
        _repository.Add(task);

        Console.WriteLine($"\n  Задача #{task.Id} создана.");
        ConsoleMenu.Pause();
    }

    private void ViewTasks()
    {
        ConsoleMenu.PrintHeader("Просмотр задач");
        Console.WriteLine("  1. Все задачи");
        Console.WriteLine("  2. Выполненные");
        Console.WriteLine("  3. Невыполненные");
        Console.WriteLine("  4. Высокий приоритет");
        Console.WriteLine();

        var choice = ConsoleMenu.ReadChoice(1, 4);

        switch (choice)
        {
            case 1:
                ConsoleMenu.PrintTaskList(_repository.GetAll(), "Все задачи");
                break;
            case 2:
                ConsoleMenu.PrintTaskList(_repository.Find(t => t.Status == WorkStatus.Done), "Выполненные задачи");
                break;
            case 3:
                ConsoleMenu.PrintTaskList(_repository.Find(t => t.Status != WorkStatus.Done), "Невыполненные задачи");
                break;
            case 4:
                ConsoleMenu.PrintTaskList(_repository.Find(t => t.Priority == Priority.High), "Задачи с высоким приоритетом");
                break;
        }

        ConsoleMenu.Pause();
    }

    private void EditTask()
    {
        ConsoleMenu.PrintHeader("Редактирование задачи");

        var id = ReadTaskId();
        if (id == null) return;

        var task = _repository.GetById(id.Value);
        if (task == null)
        {
            Console.WriteLine($"  Задача #{id} не найдена.");
            ConsoleMenu.Pause();
            return;
        }

        ConsoleMenu.PrintTask(task);

        Console.WriteLine("  Что изменить?");
        Console.WriteLine("  1. Название");
        Console.WriteLine("  2. Описание");
        Console.WriteLine("  3. Приоритет");
        Console.WriteLine("  4. Статус");
        Console.WriteLine("  5. Дедлайн");
        Console.WriteLine();

        var choice = ConsoleMenu.ReadChoice(1, 5);

        switch (choice)
        {
            case 1:
                task.Title = ConsoleMenu.ReadNonEmpty("  Новое название: ");
                break;
            case 2:
                task.Description = ConsoleMenu.ReadLine("  Новое описание: ");
                break;
            case 3:
                task.Priority = ConsoleMenu.ReadPriority();
                break;
            case 4:
                task.Status = ConsoleMenu.ReadStatus();
                break;
            case 5:
                task.Deadline = ConsoleMenu.ReadDeadline();
                break;
        }

        Console.WriteLine("  Задача обновлена.");
        ConsoleMenu.Pause();
    }

    private void DeleteTask()
    {
        ConsoleMenu.PrintHeader("Удаление задачи");

        var id = ReadTaskId();
        if (id == null) return;

        try
        {
            _repository.Remove(id.Value);
            Console.WriteLine($"  Задача #{id} удалена.");
        }
        catch (KeyNotFoundException ex)
        {
            Console.WriteLine($"  Ошибка: {ex.Message}");
        }

        ConsoleMenu.Pause();
    }

    private void SearchTasks()
    {
        ConsoleMenu.PrintHeader("Поиск задач");
        Console.WriteLine("  1. По названию");
        Console.WriteLine("  2. По статусу");
        Console.WriteLine("  3. По приоритету");
        Console.WriteLine();

        var choice = ConsoleMenu.ReadChoice(1, 3);

        switch (choice)
        {
            case 1:
            {
                var query = ConsoleMenu.ReadNonEmpty("  Название (часть): ").ToLower();
                var results = _repository.Find(t => t.Title.ToLower().Contains(query));
                ConsoleMenu.PrintTaskList(results, $"Результаты поиска: \"{query}\"");
                break;
            }
            case 2:
            {
                var status = ConsoleMenu.ReadStatus();
                var results = _repository.Find(t => t.Status == status);
                ConsoleMenu.PrintTaskList(results, $"Задачи со статусом: {TaskItem.StatusLabel(status)}");
                break;
            }
            case 3:
            {
                var priority = ConsoleMenu.ReadPriority();
                var results = _repository.Find(t => t.Priority == priority);
                ConsoleMenu.PrintTaskList(results, $"Задачи с приоритетом: {TaskItem.PriorityLabel(priority)}");
                break;
            }
        }

        ConsoleMenu.Pause();
    }

    private void ShowStatistics()
    {
        var all = _repository.GetAll();

        ConsoleMenu.PrintHeader("Статистика");
        Console.WriteLine($"  Всего задач    : {StatisticsService.GetTotal(all)}");
        Console.WriteLine($"  Выполнено      : {StatisticsService.GetCompleted(all)}");
        Console.WriteLine($"  Просрочено     : {StatisticsService.GetOverdue(all)}");
        Console.WriteLine();
        Console.WriteLine("  По приоритетам:");

        var byPriority = StatisticsService.GetByPriority(all);
        foreach (var (priority, count) in byPriority)
            Console.WriteLine($"    {TaskItem.PriorityLabel(priority),-10}: {count}");

        ConsoleMenu.Pause();
    }

    private async Task SaveTasksAsync()
    {
        ConsoleMenu.PrintHeader("Сохранение задач");

        try
        {
            await _fileService.SaveAsync(_repository.GetAll(), _dataFile);
            Console.WriteLine($"  Задачи сохранены в файл: {_dataFile}");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"  Ошибка: {ex.Message}");
        }

        ConsoleMenu.Pause();
    }

    private async Task LoadTasksAsync()
    {
        ConsoleMenu.PrintHeader("Загрузка задач");

        try
        {
            var tasks = await _fileService.LoadAsync(_dataFile);
            _repository.Clear();
            foreach (var task in tasks)
                _repository.Add(task);

            _nextId = tasks.Count > 0 ? tasks.Max(t => t.Id) + 1 : 1;

            Console.WriteLine($"  Загружено задач: {tasks.Count}");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"  Ошибка: {ex.Message}");
        }

        ConsoleMenu.Pause();
    }

    private int? ReadTaskId()
    {
        Console.Write("  Введите Id задачи: ");
        var input = Console.ReadLine()?.Trim();
        if (int.TryParse(input, out int id))
            return id;
        Console.WriteLine("  Неверный Id.");
        ConsoleMenu.Pause();
        return null;
    }

    private void OnOverdue(TaskItem task)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n  [!] Просрочена задача: \"{task.Title}\" (дедлайн: {task.DisplayDeadline:dd.MM.yyyy})");
        Console.ResetColor();
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        _checker.Dispose();
        _fileService.Dispose();
        GC.SuppressFinalize(this);
    }
}
