public class DeadlineChecker : IDisposable
{
    private readonly Repository<TaskItem> _repository;
    private readonly Action<TaskItem> _onOverdue;
    private readonly Timer _timer;
    private bool _disposed = false;

    public DeadlineChecker(Repository<TaskItem> repository, Action<TaskItem> onOverdue, int intervalSeconds = 5)
    {
        _repository = repository;
        _onOverdue = onOverdue;
        _timer = new Timer(Check, null, TimeSpan.FromSeconds(intervalSeconds), TimeSpan.FromSeconds(intervalSeconds));
    }

    private void Check(object? state)
    {
        if (_disposed) return;

        var overdue = _repository.Find(t => t.IsOverdue);
        foreach (var task in overdue)
            _onOverdue(task);
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        _timer.Dispose();
        GC.SuppressFinalize(this);
    }
}
