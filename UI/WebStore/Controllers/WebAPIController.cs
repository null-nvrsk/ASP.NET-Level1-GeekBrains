using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.TestAPI;

namespace WebStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebAPIController : Controller
    {
        private readonly IValuesService _ValuesService;

        public WebAPIController(IValuesService valuesService) { _ValuesService = valuesService; }
        public IActionResult Index()
        {
            var values = _ValuesService.Get();
            return View(values);
        }
    }
}
