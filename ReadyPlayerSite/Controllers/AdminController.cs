using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReadyPlayerSite.Models;
using WebMatrix.WebData;

namespace ReadyPlayerSite.Controllers
{
    [Authorize(Roles="Administrator")]
    public class AdminController : Controller
    {
        private PlayerDbContext db = new PlayerDbContext();

        //
        // GET: /Admin/

        public ActionResult Index()
        {
            var adminActions = db.adminActions.Select(s => s).OrderByDescending(aa => aa.when).ThenBy(aa => aa.player.user.username);
           // String name = adminActions.First().user.username;
            return View(adminActions.ToList());
        }

        public ActionResult Details(int id = 0)
        {
            AdminAction action = db.adminActions.Find(id);

            if (action == null)
            {
                return HttpNotFound();
            }
            return View(action);
        }

        public ActionResult ManagePlayerByEid(string eid)
        {
            Player player = db.Players.Where(s => s.eid == eid).FirstOrDefault();
            if (player == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("ManagePlayer", new { id = player.ID });
        }

        public ActionResult ManagePlayer(int id = 0)
        {
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            ViewBag.playerID = player.ID;
            ViewBag.playerusername = player.user.username;
            ViewBag.adminID = WebSecurity.CurrentUserId;
            ViewBag.time = DateTime.Now;

            var actionTypeSelect = (from AdminActionType a in Enum.GetValues(typeof(AdminActionType))
                                   select new { Id = a, Name = a.ToString() }).ToList();
            if (player.freezeInfo == null)
            {
                actionTypeSelect.Remove(actionTypeSelect.Find(a => a.Id == AdminActionType.UnfreezeAccount));
            }
            else
            {
               actionTypeSelect.Remove(actionTypeSelect.Find(a => a.Id == AdminActionType.FreezeAccount));
            }
            ViewBag.actionSelect = new SelectList(actionTypeSelect, "Id", "Name");

            var scoreTypeSelect = from TaskType e in Enum.GetValues(typeof(TaskType))
                           select new { Id = e, Name = e.ToString() };
           
            ViewBag.typeSelect = new SelectList(scoreTypeSelect, "Id", "Name", null);
            
            return View();
        }

        [HttpPost]
        public ActionResult ManagePlayer(AdminAction aa)
        {
            
            if (ModelState.IsValid)
            {
                if (aa.type != AdminActionType.AddPoints && aa.type != AdminActionType.RemovePoints)
                {
                    aa.modifyTarget = null;
                    aa.value = null;
                }

                db.adminActions.Add(aa);
                aa.player = db.Players.Find(aa.playerID);
                aa.PerformAction(db);
                db.SaveChanges();
                
                return RedirectToAction("Index");
            }
            return View(aa.playerID);
        }

    }
}
