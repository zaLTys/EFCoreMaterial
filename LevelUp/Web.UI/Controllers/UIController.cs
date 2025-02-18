using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc; 
using Web.UI.ViewModels;

namespace Web.UI.Controllers
{
    //[Authorize]
    public class UIController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<UIController> _logger;

        public UIController(IHttpClientFactory httpClientFactory,
            ILogger<UIController> logger)
        {
            _httpClientFactory = httpClientFactory ??
                throw new ArgumentNullException(nameof(httpClientFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IActionResult> Index()
        {
            
            
            return View(new IndexViewModel());
        }

        
    }
}
