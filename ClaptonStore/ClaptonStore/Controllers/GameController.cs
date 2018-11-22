namespace ClaptonStore.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Models.BindingModels;
    using Models.ViewModels;
    using Services.Contracts;

    public class GameController : Controller
    {
        private readonly IGameService gameService;
        private readonly ILogger logger;

        public GameController(IGameService gameService, ILogger<GameController> logger)
        {
            this.gameService = gameService;
            this.logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Administrators")]
        public IActionResult Add() => this.View();

        [HttpPost]
        [Authorize(Roles = "Administrators")]
        public async Task<IActionResult> Add(GameBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                this.logger.LogError($"Invalid {nameof(GameBindingModel)}!");
                return this.RedirectToAction("Index", "Home");
            }

            var gameExists = await this.gameService.ExistsAsync(model.Title);

            if (gameExists)
            {
                this.logger.LogError($"{model.Title} exist!");
                return this.RedirectToAction("Index", "Home");
            }

            var game = await this.gameService.CreateGameAsync(
                model.Title,
                model.Description,
                model.ThumbnailUrl,
                model.Price,
                model.Size,
                model.ReleaseDate,
                model.GameType,
                model.Developer);

            this.logger.LogInformation("Admin add a new game!");

            return this.RedirectToAction("Details", new { id = game.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await this.gameService.Details<GameDetailsViewModel>(id);

            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var model = await this.gameService
                .All<AllGamesViewModel>()
                .ToListAsync();

            return this.View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var game = await this.gameService.ById<GameBindingModel>(id);

            if (game == null)
            {
                return this.NotFound();
            }

            return this.View(game);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GameBindingModel model)
        {
            if (id != model.Id)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    await this.gameService.Edit(model.Id, model.Title);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await this.gameService.ExistsAsync(model.Id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return this.RedirectToAction("Index", "Home");
            }
            return this.View(model);
        }
    }
}