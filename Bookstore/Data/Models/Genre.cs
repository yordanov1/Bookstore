namespace Bookstore.Data.Models
{
    using System.Collections.Generic;

    public class Genre
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<Book> Books { get; set; } = new List<Book>();
    }
}
