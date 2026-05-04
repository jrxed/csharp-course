public class Product : IEntity
{
    public int Id { get; }
    public string Name { get; }
    public decimal Price { get; }

    public Product(int id, string name, decimal price)
    {
        Id = id;
        Name = name;
        Price = price;
    }

    public override string ToString() => $"Product(Id={Id}, Name={Name}, Price={Price})";
}

public class User : IEntity
{
    public int Id { get; }
    public string Username { get; }

    public User(int id, string username)
    {
        Id = id;
        Username = username;
    }

    public override string ToString() => $"User(Id={Id}, Username={Username})";
}
