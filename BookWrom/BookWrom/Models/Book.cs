namespace BookWrom.Models;

public class Book : BaseModel
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public List<Review> Reviews { get; set; } = [];
}