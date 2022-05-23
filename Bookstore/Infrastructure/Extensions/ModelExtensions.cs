namespace Bookstore.Infrastructure.Extensions
{
    using Bookstore.Services.Books.Models;

    public static class ModelExtensions
    {
        public static string GetInformation(this IBookModel book)
        => book.BookTitle + "-" + book.Author + "-" + book.PublishingHouse;
    }
}
