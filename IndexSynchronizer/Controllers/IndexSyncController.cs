using Microsoft.AspNetCore.Mvc;

namespace IndexSynchronizer.Controllers
{
    public class IndexSyncController : Controller
    {
        public IndexSyncController()
        {
            
        }

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult IndexSync()
		{
			return View();
		}

		public IActionResult Updates()
        {
            return View();
        }
    }
}
