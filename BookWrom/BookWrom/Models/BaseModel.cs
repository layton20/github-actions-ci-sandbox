namespace BookWrom.Models;

public abstract class BaseModel
{
    public int Id { get; set; }
    public Guid Uid { get; set; }
}