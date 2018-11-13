namespace ClaptonStore.Services
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Contracts;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Models.Enum;
    using Models.ViewModels;

    public class GameService : IGameService
    {
        private readonly ClaptonStoreContext context;

        public GameService(ClaptonStoreContext context)
        {
            this.context = context;
        }

        public async Task<Game> CreateGameAsync(
            string title,
            string description,
            string thumbnailUrl,
            decimal price,
            double size,
            DateTime releaseDate,
            GameGenreType gameType,
            string developer)
        {
            var newDeveloper = this.context
                .Developers
                .FirstOrDefault(d => d.Title == developer);

            if (newDeveloper is null)
            {
                newDeveloper = new Developer { Title = developer };

                this.context.Developers.Add(newDeveloper);
                await this.context.SaveChangesAsync();
            }

            var game = new Game
            {
                Title = title,
                Description = description,
                Price = price,
                Size = size,
                ThumbnailUrl = thumbnailUrl,
                ReleaseDate = releaseDate,
                GameGenreType = gameType,
                Developer = newDeveloper
            };

            this.context.Games.Add(game);
            await this.context.SaveChangesAsync();

            return game;
        }

        public async Task<bool> ExistsAsync(string title)
            => await this.context
                .Games
                .AnyAsync(u => u.Title == title);

        public async Task<GameDetailsViewModel> GetDetailsAsync(int id)
        {
            var gameDb = await this.context
                .Games
                .Include(d => d.Developer)
                .FirstOrDefaultAsync(g => g.Id == id);

            return new GameDetailsViewModel
            {
                Title = gameDb.Title,
                Developer = gameDb.Developer.Title,
                Description = gameDb.Description,
                Price = gameDb.Price,
                Size = gameDb.Size,
                ThumbnailUrl = gameDb.ThumbnailUrl,
                ReleaseDate = gameDb.ReleaseDate,
                Genre = gameDb.GameGenreType.GetDisplayName()
            };
        }
    }
}