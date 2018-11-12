namespace ClaptonStore.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.BindingModels;
    using Services.Contracts;

    public class GameController : Controller
    {
        private readonly IGameService gameService;

        public GameController(IGameService gameService)
        {
            this.gameService = gameService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Add() => this.View();

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(AddGameBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Index", "Home");
            }

            var gameExists = await this.gameService.ExistsAsync(model.Title);

            if (gameExists)
            {
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

            return this.RedirectToAction("Details", new { id = game.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await this.gameService.GetDetailsAsync(id);

            return this.View(model);
        }
    }
}