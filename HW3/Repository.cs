public class Repository<T> where T : IEntity
{
    private readonly Dictionary<int, T> _store = new();

    public int Count => _store.Count;

    public void Add(T item)
    {
        if (_store.ContainsKey(item.Id))
            throw new InvalidOperationException($"Entity with Id={item.Id} already exists.");
        _store[item.Id] = item;
    }

    public bool Remove(int id) => _store.Remove(id);

    public T? GetById(int id) => _store.TryGetValue(id, out var item) ? item : default;

    public IReadOnlyList<T> GetAll() => _store.Values.ToList();

    public IReadOnlyList<T> Find(Predicate<T> predicate)
    {
        var result = new List<T>();
        foreach (var item in _store.Values)
            if (predicate(item))
                result.Add(item);
        return result;
    }
}
