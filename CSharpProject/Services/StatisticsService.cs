public static class StatisticsService
{
    public static int GetTotal(IReadOnlyList<TaskItem> tasks) => tasks.Count;

    public static int GetCompleted(IReadOnlyList<TaskItem> tasks) =>
        tasks.Count(t => t.Status == WorkStatus.Done);

    public static int GetOverdue(IReadOnlyList<TaskItem> tasks) =>
        tasks.Count(t => t.IsOverdue);

    public static Dictionary<Priority, int> GetByPriority(IReadOnlyList<TaskItem> tasks)
    {
        var result = new Dictionary<Priority, int>();
        foreach (Priority p in Enum.GetValues<Priority>())
            result[p] = tasks.Count(t => t.Priority == p);
        return result;
    }
}
