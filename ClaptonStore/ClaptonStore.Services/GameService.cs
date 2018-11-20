namespace ClaptonStore.Services
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using AutoMapper.QueryableExtensions;
    using Contracts;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Models.Enum;

    public class GameService : IGameService
    {
        private readonly ClaptonStoreContext context;

        public GameService(ClaptonStoreContext context) => this.context = context;

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
                Developer = newDeveloper,
                AddedOn = DateTime.UtcNow
            };

            this.context.Games.Add(game);
            await this.context.SaveChangesAsync();

            return game;
        }

        public async Task<bool> ExistsAsync(string title)
            => await this.context
                .Games
                .AnyAsync(u => u.Title == title);

        public async Task<TModel> Details<TModel>(int id)
            => await this.By<TModel>(x => x.Id == id)
                .FirstOrDefaultAsync();

        public IQueryable<TModel> All<TModel>() => this.By<TModel>();

        public IQueryable<TModel> Find<TModel>(string title) 
            => this.By<TModel>(g => g.Title.ToLower().Contains(title.ToLower()));

        private IQueryable<TModel> By<TModel>(Expression<Func<Game, bool>> predicate = null)
            => this.context
                .Games
                .AsQueryable()
                .Where(predicate ?? (i => true))
                .ProjectTo<TModel>();
    }
}