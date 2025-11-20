// Models/ParentDto.cs
public class ParentDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<ChildDto> Children { get; set; } = new();
}

// Models/ChildDto.cs
public class ChildDto
{
    public Guid Id { get; set; }
    public string Label { get; set; } = string.Empty;
    public Guid ParentId { get; set; }
}
