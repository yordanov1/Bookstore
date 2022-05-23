namespace Bookstore.Services.Books
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Bookstore.Data;
    using Bookstore.Data.Models;
    using Bookstore.Models;
    using Bookstore.Services.Books.Models;
    using System.Collections.Generic;
    using System.Linq;

    public class BookService : IBookService
    {
        private readonly BookstoreDbContext data;
        private readonly IConfigurationProvider mapper;

        public BookService(
            BookstoreDbContext data, 
            IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
        }

        public BookQueryServiceModel All(
            string author, 
            string searchTerm,
            BookSorting sorting,
            int currentPage,
            int booksPerPage)
        {
            var booksQuery = this.data.Books
                .Where(x => x.IsPublic);


            if (!string.IsNullOrWhiteSpace(author))
            {
                booksQuery = booksQuery.Where(x => x.Author == author);
            }


            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                booksQuery = booksQuery.Where(x =>
                       (x.BookTitle + " " + x.Author).ToLower().Contains(searchTerm.ToLower())
                     || x.Description.ToLower().Contains(searchTerm.ToLower()));
            }


            booksQuery = sorting switch
            {
                BookSorting.Rating => booksQuery.OrderByDescending(x => x.Rating),
                BookSorting.Author => booksQuery.OrderByDescending(x => x.Author),
                _ => booksQuery.OrderByDescending(x => x.Id) // ако не се парснат 
            };


            var totalBooks = booksQuery.Count();


            var books = GetBooks(booksQuery
                .Skip((currentPage - 1) * booksPerPage)
                .Take(booksPerPage));
                

            return new BookQueryServiceModel
            {
                TotalBooks = totalBooks,
                CurrentPage = currentPage,
                BooksPerPage = booksPerPage,
                Books = books
            };
        }


        public IEnumerable<LatestBooksServiceModel> Latest()
          => this.data
           .Books
           .Where(x => x.IsPublic)
           .OrderByDescending(x => x.Id)
           .ProjectTo<LatestBooksServiceModel>(this.mapper) // .ConfigurationProvider
           .Take(3)
           .ToList();

        public BookDetailsServiceModel Details(int id)
          => this.data.
            Books.
            Where(b => b.Id == id).
            ProjectTo<BookDetailsServiceModel>(this.mapper). 
            FirstOrDefault();
        /*
        public BookDetailsServiceModel Details(int id)
          => this.data.
            Books.
            Where(b => b.Id == id).
            Select(b => new BookDetailsServiceModel
            {
                Id = b.Id,
                BookTitle = b.BookTitle,
                Author = b.Author,
                ImageUrl = b.ImageUrl,
                PublishingHouse = b.PublishingHouse,
                Rating = b.Rating,
                Description = b.Description,
                GenreId = b.GenreId,
                GenreName = b.Genre.Name,
                ModeratorId = b.ModeratorId,
                ModeratorName = b.Moderator.Name,
                UserId = b.Moderator.UserId
            })
            .FirstOrDefault();
        */


        public int Create(
            string bookTitle, 
            string author, 
            string imageUrl, 
            string publishingHouse, 
            int? rating, 
            string description, 
            int genreId, 
            int moderatorId)
        {
            var newBook = new Book
            {
                BookTitle = bookTitle,
                Author = author,
                ImageUrl = imageUrl,
                PublishingHouse = publishingHouse,
                Rating = rating,
                Description = description,
                GenreId = genreId,
                ModeratorId = moderatorId,
                IsPublic = false
            };

            this.data.Books.Add(newBook);
            this.data.SaveChanges();

            return newBook.Id;
        }

        public bool Edit(
            int id, 
            string bookTitle, 
            string author, 
            string imageUrl, 
            string publishingHouse, 
            int? rating, 
            string description, 
            int genreId)
        {
            var bookData = this.data.Books.Find(id);

            if (bookData == null)
            {
                return false;
            }

            bookData.BookTitle = bookTitle;
            bookData.Author = author;   
            bookData.ImageUrl = imageUrl;
            bookData.PublishingHouse = publishingHouse;
            bookData.Rating = rating;
            bookData.Description = description;
            bookData.GenreId = genreId;
            bookData.IsPublic = false;

            
            this.data.SaveChanges();

            return true;

        }

        public IEnumerable<BookServiceModel> ByUser(string userId)
        => GetBooks(this.data
            .Books
            .Where(b => b.Moderator.UserId == userId));

        public bool IsByModerator(int bookId, int moderatorId)
      => this.data.
            Books.
            Any(b => b.Id == bookId && b.ModeratorId == moderatorId);

        public IEnumerable<string> AllBookAuthors()
        {
            return this.data
                .Books
                .Select(x => x.Author)
                .Distinct()
                .OrderBy(a => a)
                .ToList();
        }

        public IEnumerable<BookGenreServiceModel> AllBookGenres()
            => this.data
           .Genres
           .Select(x => new BookGenreServiceModel
           {
               Id = x.Id,
               Name = x.Name
           })
           .ToList();

        public bool GenreExist(int genreId)
        => this.data.
            Genres.
            Any(x => x.Id == genreId);

        private static IEnumerable<BookServiceModel> GetBooks(IQueryable<Book> bookQuery)
            => bookQuery.Select(book => new BookServiceModel
            {
                Id = book.Id,
                BookTitle = book.BookTitle,
                Author = book.Author,
                ImageUrl = book.ImageUrl,
                PublishingHouse = book.PublishingHouse,
                Rating = book.Rating,
                Description = book.Description,
                GenreName = book.Genre.Name
            })
            .ToList();

      
    }
}
