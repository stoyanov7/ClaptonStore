namespace ClaptonStore.Services.Contracts
{
    using System;
    using System.Threading.Tasks;
    using Models;
    using Models.Enum;

    public interface IGameService
    {
        Task<Game> CreateGameAsync(
            string title,
            string description,
            string thumbnailUrl,
            decimal price,
            double size,
            DateTime releaseDate,
            GameGenreType gameType,
            string developer);

        Task<bool> ExistsAsync(string title);
    }
}