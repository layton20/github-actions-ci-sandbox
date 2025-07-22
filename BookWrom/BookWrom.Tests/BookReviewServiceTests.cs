using BookWrom.Models;
using BookWrom.Services;

namespace BookWrom.Tests;

public class BookReviewServiceTests
{
    [Fact]
    public void AddBook_ShouldAssignIdAndPublicId()
    {
        BookReviewService service = new();
        Book book = new() { Title = "Clean Code", Author = "Robert C. Martin" };

        Book result = service.AddBook(book);

        Assert.Equal(1, result.Id);
        Assert.NotEqual(Guid.Empty, result.Uid);
    }

    [Fact]
    public void AddBook_DuplicateTitle_ShouldReturnExistingBook()
    {
        BookReviewService service = new();

        Book first = service.AddBook(new Book { Title = "1984", Author = "Orwell" });
        Book second = service.AddBook(new Book { Title = "1984", Author = "Someone Else" });

        Assert.Equal(first.Id, second.Id);
        Assert.Single(service.GetAllBooks());
    }

    [Fact]
    public void AddBook_Invalid_ShouldThrow()
    {
        BookReviewService service = new();
        Book badBook = new() { Title = "", Author = "X" };

        Assert.Throws<ArgumentException>(() => service.AddBook(badBook));
    }

    [Fact]
    public void GetBookByPublicId_ShouldReturnBook()
    {
        BookReviewService service = new();
        Book book = service.AddBook(new Book { Title = "Dune", Author = "Frank Herbert" });

        Book? result = service.GetBookByPublicId(book.Uid);

        Assert.NotNull(result);
        Assert.Equal("Dune", result!.Title);
    }

    [Fact]
    public void AddReview_ShouldSucceed()
    {
        BookReviewService service = new();
        Book book = service.AddBook(new Book { Title = "The Hobbit", Author = "Tolkien" });

        Review review = new()
        {
            BookId = book.Id,
            Description = "An epic journey!"
        };

        service.AddReview(review);

        List<Review> reviews = service.GetReviewsForBook(book.Id).ToList();
        Assert.Single(reviews);
        Assert.Equal("An epic journey!", reviews[0].Description);
    }

    [Fact]
    public void AddReview_MissingBook_ShouldThrow()
    {
        BookReviewService service = new();
        Review review = new() { BookId = 99, Description = "Ghost book" };

        Assert.Throws<InvalidOperationException>(() => service.AddReview(review));
    }

    [Fact]
    public void AddReview_Invalid_ShouldThrow()
    {
        BookReviewService service = new();
        Book book = service.AddBook(new Book { Title = "Ego is the Enemy", Author = "Ryan Holiday" });

        Review review = new() { BookId = book.Id, Description = "" };

        Assert.Throws<ArgumentException>(() => service.AddReview(review));
    }
}