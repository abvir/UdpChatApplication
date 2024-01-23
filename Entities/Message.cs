namespace Entities;

public class Message : Entity
{
    public string Text { get; set; } = string.Empty;
    public Guid SenderId { get; set; }
    public Guid RecipientId { get; set; }
}
