﻿namespace Bookstore.Services.Books
{
    public class BookDetailsServiceModel : BookServiceModel
    {
        public int ModeratorId { get; set; }

        public string ModeratorName { get; set; }

        public int GenreId { get; set; }

        public string UserId { get; set; }
    }
}
