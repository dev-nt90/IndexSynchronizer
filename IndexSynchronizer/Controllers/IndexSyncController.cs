using Microsoft.AspNetCore.Mvc;

namespace IndexSynchronizer.Controllers
{
    public class IndexSyncController : Controller
    {
        private readonly ILogger logger;
        public IndexSyncController(ILogger<IndexSyncController> logger)
        {
            this.logger = logger;
        }

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult IndexSync()
		{
            try
            {
                return View();
            }
            catch(Exception ex) 
            {
                logger.LogError(ex, "An unexpected exception occurred in the IndexSyncController");
                throw;
            }
		}

		public IActionResult Updates()
        {
            return View();
        }
    }
}
