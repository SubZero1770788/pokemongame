using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class Fallback : Controller
    {
        public ActionResult Index()
        {
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory()
            , "wwwroot", "index.html"), "text/HTML");
        }
    }
}