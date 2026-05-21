public class TaskFileService : IDisposable
{
    private bool _disposed = false;

    public async Task SaveAsync(IReadOnlyList<TaskItem> tasks, string path)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        try
        {
            await using var writer = new StreamWriter(path, append: false);
            foreach (var task in tasks)
                await writer.WriteLineAsync(
                    $"{task.Id}|{task.Title}|{task.Description}|{task.Priority}|{task.Deadline:O}|{task.Status}");
        }
        catch (IOException ex)
        {
            throw new IOException($"Ошибка сохранения файла: {ex.Message}", ex);
        }
    }

    public async Task<List<TaskItem>> LoadAsync(string path)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        var tasks = new List<TaskItem>();

        if (!File.Exists(path))
            return tasks;

        try
        {
            using var reader = new StreamReader(path);
            string? line;

            while ((line = await reader.ReadLineAsync()) != null)
            {
                var parts = line.Split('|');
                if (parts.Length < 6) continue;

                if (!int.TryParse(parts[0], out int id)) continue;
                if (!Enum.TryParse<Priority>(parts[3], out var priority)) continue;
                if (!DateTime.TryParse(parts[4], out var deadline)) continue;
                if (!Enum.TryParse<WorkStatus>(parts[5], out var status)) continue;

                tasks.Add(new TaskItem(id, parts[1], parts[2], priority, deadline, status));
            }
        }
        catch (IOException ex)
        {
            throw new IOException($"Ошибка загрузки файла: {ex.Message}", ex);
        }

        return tasks;
    }

    public void Dispose()
    {
        _disposed = true;
        GC.SuppressFinalize(this);
    }
}
