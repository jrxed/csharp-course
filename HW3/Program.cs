// Task 1: Generic Repository
Console.WriteLine("=== Task 1: Repository<T> ===\n");

var products = new Repository<Product>();
products.Add(new Product(1, "Laptop", 75000));
products.Add(new Product(2, "Mouse", 800));
products.Add(new Product(3, "Monitor", 25000));
products.Add(new Product(4, "Keyboard", 1500));

Console.WriteLine($"Products count: {products.Count}");
Console.WriteLine($"GetById(2): {products.GetById(2)}");
Console.WriteLine($"GetById(99): {products.GetById(99)}");

var expensive = products.Find(p => p.Price > 1000);
Console.WriteLine("\nProducts with Price > 1000:");
foreach (var p in expensive)
    Console.WriteLine($"  {p}");

Console.WriteLine("\nAll products:");
foreach (var p in products.GetAll())
    Console.WriteLine($"  {p}");

products.Remove(2);
Console.WriteLine($"\nAfter removing Id=2, count: {products.Count}");

try
{
    products.Add(new Product(1, "Duplicate", 0));
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"\nDuplicate exception caught: {ex.Message}");
}

var users = new Repository<User>();
users.Add(new User(1, "alice"));
users.Add(new User(2, "bob"));
Console.WriteLine($"\nUsers count: {users.Count}");
Console.WriteLine($"GetById(1): {users.GetById(1)}");
var usersWithA = users.Find(u => u.Username.Contains('a'));
Console.WriteLine("Users with 'a' in name:");
foreach (var u in usersWithA)
    Console.WriteLine($"  {u}");

// Task 2: CollectionUtils
Console.WriteLine("\n=== Task 2: CollectionUtils ===\n");

// Distinct
var ints = new List<int> { 3, 1, 4, 1, 5, 9, 2, 6, 5, 3, 5 };
Console.WriteLine($"Distinct ints: [{string.Join(", ", CollectionUtils.Distinct(ints))}]");

var strings = new List<string> { "apple", "banana", "apple", "cherry", "banana" };
Console.WriteLine($"Distinct strings: [{string.Join(", ", CollectionUtils.Distinct(strings))}]");

// GroupBy
var words = new List<string> { "cat", "dog", "ant", "bear", "fish", "ox", "eagle" };
var byLength = CollectionUtils.GroupBy(words, w => w.Length);
Console.WriteLine("\nWords grouped by length:");
foreach (var (len, group) in byLength)
    Console.WriteLine($"  {len}: [{string.Join(", ", group)}]");

// Merge
var counters1 = new Dictionary<string, int> { ["apple"] = 3, ["banana"] = 2, ["cherry"] = 5 };
var counters2 = new Dictionary<string, int> { ["banana"] = 4, ["cherry"] = 1, ["date"] = 7 };
var merged = CollectionUtils.Merge(counters1, counters2, (a, b) => a + b);
Console.WriteLine("\nMerged word counters:");
foreach (var (word, count) in merged)
    Console.WriteLine($"  {word}: {count}");

// MaxBy
var productList = new List<Product>
{
    new(1, "Laptop", 75000),
    new(2, "Mouse", 800),
    new(3, "Monitor", 25000),
};
var mostExpensive = CollectionUtils.MaxBy(productList, p => p.Price);
Console.WriteLine($"\nMost expensive product: {mostExpensive}");

try
{
    CollectionUtils.MaxBy(new List<Product>(), p => p.Price);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Empty MaxBy exception: {ex.Message}");
}
