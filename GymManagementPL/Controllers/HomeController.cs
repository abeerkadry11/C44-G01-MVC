using GymManagementBLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class HomeController : Controller
    {
        // Don't forget to register the AnalyticsService in the Program.cs 
        private readonly IAnalyticsService analyticsService;

        public HomeController(IAnalyticsService _analyticsService)
        {
            analyticsService = _analyticsService;
        }

        public ViewResult Index()
        {
            var Data = analyticsService.GetAnalyticsData();

            //return View(); // return Default View Of Action [View With Action Name]

            return View(Data); // return Default View Of Action with Passing Model 

            //return View("ViewName");// return Another View With Specific Name
            //return View("Hamada", Data); // return Another View With Specific Name with Passing Model
        }

    }
}
