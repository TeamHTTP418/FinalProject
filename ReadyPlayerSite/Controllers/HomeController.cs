using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReadyPlayerSite.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/


        public ActionResult Index()
        {
            ViewBag.Message = "Ready player one Home";

            return View();
        }

        public ActionResult LeaderBoard()
        {
            ViewBag.Message = "Ready Player One Leaderboard";

            return View();
        }

    }
}
