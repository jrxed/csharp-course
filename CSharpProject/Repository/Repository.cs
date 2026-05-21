public class Repository<T> : IRepository<T> where T : TaskItem
{
    private readonly Dictionary<int, T> _store = new();

    public int Count => _store.Count;

    public void Add(T item)
    {
        if (_store.ContainsKey(item.Id))
            throw new InvalidOperationException($"Задача с Id={item.Id} уже существует.");
        _store[item.Id] = item;
    }

    public void Remove(int id)
    {
        if (!_store.Remove(id))
            throw new KeyNotFoundException($"Задача с Id={id} не найдена.");
    }

    public T? GetById(int id) =>
        _store.TryGetValue(id, out var item) ? item : null;

    public IReadOnlyList<T> GetAll() => _store.Values.OrderBy(t => t.Id).ToList();

    public IReadOnlyList<T> Find(Predicate<T> predicate)
    {
        var result = new List<T>();
        foreach (var item in _store.Values)
            if (predicate(item))
                result.Add(item);
        return result;
    }

    public void Clear() => _store.Clear();
}
