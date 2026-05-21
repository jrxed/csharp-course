public interface IRepository<T> where T : TaskItem
{
    void Add(T item);
    void Remove(int id);
    T? GetById(int id);
    IReadOnlyList<T> GetAll();
    IReadOnlyList<T> Find(Predicate<T> predicate);
    int Count { get; }
}
