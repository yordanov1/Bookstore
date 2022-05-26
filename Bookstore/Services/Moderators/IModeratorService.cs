namespace Bookstore.Services.Moderators
{
    public interface IModeratorService
    {
        public bool IsModerator(string userId);

        public int IdByUser(string userId);
    }
}
