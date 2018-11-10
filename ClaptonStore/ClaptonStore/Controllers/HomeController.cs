namespace ClaptonStore.Controllers
{
    using System.Diagnostics;
    using Microsoft.AspNetCore.Mvc;
    using Models.ViewModels;

    public class HomeController : Controller
    {
        public IActionResult Index() => this.View();

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
