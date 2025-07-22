using BookWrom.Models;

namespace BookWrom.Services;

public class BookReviewService
{
    private readonly List<Review> __Reviews = [];
    private readonly List<Book> _Books = [];
    private int __NextBookId = 1;
    private int __NextReviewId = 1;

    public IEnumerable<Book> GetAllBooks()
    {
        return _Books;
    }

    public Book? GetBookByPublicId(Guid publicId)
    {
        return _Books.FirstOrDefault(b => b.Uid == publicId);
    }

    public Book AddBook(Book book)
    {
        if (string.IsNullOrWhiteSpace(book.Title) || string.IsNullOrWhiteSpace(book.Author))
        {
            throw new ArgumentException("Book must have a title and author.");
        }

        if (_Books.Any(b => b.Title.Equals(book.Title, StringComparison.OrdinalIgnoreCase)))
        {
            return _Books.First(b => b.Title.Equals(book.Title, StringComparison.OrdinalIgnoreCase));
        }

        book.Id = __NextBookId++;
        book.Uid = Guid.NewGuid();

        _Books.Add(book);
        return book;
    }

    public IEnumerable<Review> GetReviewsForBook(int bookId)
    {
        return __Reviews.Where(r => r.BookId == bookId);
    }

    public void AddReview(Review review)
    {
        if (review.BookId == 0 || string.IsNullOrWhiteSpace(review.Description))
        {
            throw new ArgumentException("Review must have a valid book ID and non-empty text.");
        }

        if (_Books.All(b => b.Id != review.BookId))
        {
            throw new InvalidOperationException("Book does not exist.");
        }

        review.Id = __NextReviewId++;

        __Reviews.Add(review);
    }
}