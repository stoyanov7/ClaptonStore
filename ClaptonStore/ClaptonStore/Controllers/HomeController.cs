namespace ClaptonStore.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Services.Contracts;

    public class HomeController : Controller
    {
        private readonly IGameService gameService;

        public HomeController(IGameService gameService) => this.gameService = gameService;

        public async Task<IActionResult> Index()
        {
            var model = await this.gameService.ListAllGamesAsync();

            return this.View(model);
        }

        public IActionResult About()
        {
            this.ViewData["Message"] = "Your application description page.";

            return this.View();
        }

        public IActionResult Contact()
        {
            this.ViewData["Message"] = "Your contact page.";

            return this.View();
        }

        public IActionResult Privacy() => this.View();

        [ResponseCache(Duration = 0, 
            Location = ResponseCacheLocation.None, 
            NoStore = true)]
        public IActionResult Error()
        {
            var errorViewMode = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier
            };

            return this.View(errorViewMode);
        }
    }
}
