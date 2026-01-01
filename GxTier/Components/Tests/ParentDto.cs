// Models/ParentDto.cs
public class ParentDto
{
    private static int _counter = 0;
    public ParentDto()
    {
        Id = ++_counter;
    }
    public int Id { get; private set; }
    public string Name { get; set; } = string.Empty;
    public List<ChildDto> Children { get; set; } = new();
}

// Models/ChildDto.cs
public class ChildDto
{
    public int Id { get; set; }
    public string Label { get; set; } = string.Empty;
    public int ParentId { get; set; }
}
