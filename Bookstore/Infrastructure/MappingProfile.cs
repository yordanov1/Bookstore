namespace Bookstore.Infrastructure
{
    using AutoMapper;
    using Bookstore.Data.Models;
    using Bookstore.Models.Books;
    using Bookstore.Models.Home;
    using Bookstore.Services.Books.Models;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Book, BookGenreServiceModel>();

            this.CreateMap<Book, LatestBooksServiceModel>();
            this.CreateMap<BookDetailsServiceModel, BookFormModel>();

            this.CreateMap<Book, BookServiceModel>()
                .ForMember(x => x.GenreName, cfg => cfg.MapFrom(x => x.Genre.Name));

            this.CreateMap<Book, BookDetailsServiceModel>()
                .ForMember(b => b.UserId, cfg => cfg.MapFrom(b => b.Moderator.UserId))
                .ForMember(b => b.GenreName, cfg => cfg.MapFrom(b => b.Genre.Name));
        } 
    }
}
