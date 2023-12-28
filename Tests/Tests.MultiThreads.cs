namespace Tests;

public partial class Tests
{
    [Test]
    public void MultiThreads()
    {
        var tasks = new List<Task<List<Type>>>();
        var typesCount = 500;

        for (int i = 0; i < 3; i++)
            tasks.Add(Task.Run(() =>
            {
                return Enumerable.Range(0, typesCount)
                    .Select(x => DynamicFactory.CreateType(($"Prop{x}", typeof(int))))
                    .ToList();
            }));

        Task.WaitAll(tasks.ToArray());

        var unique = new HashSet<Type>(tasks.SelectMany(x => x.Result));

        Assert.That(unique.Count, Is.EqualTo(typesCount));
    }
}