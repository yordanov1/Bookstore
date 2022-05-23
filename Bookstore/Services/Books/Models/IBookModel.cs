namespace Bookstore.Services.Books.Models
{
    public interface IBookModel
    {
        string BookTitle { get; }

        string Author { get; }

        string PublishingHouse { get; }
    }
}
