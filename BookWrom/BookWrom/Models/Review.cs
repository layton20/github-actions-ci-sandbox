namespace BookWrom.Models;

public class Review : BaseModel
{
    public int Rating { get; set; }
    public string Description { get; set; } = string.Empty;
    public string ReviewedBy { get; set; } = string.Empty;
    public Book Book { get; set; }
    public int BookId { get; set; }
}