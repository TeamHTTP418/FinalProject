using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReadyPlayerSite.Models;
using ReadyPlayerSite.ViewModels;
using WebMatrix.WebData;

namespace ReadyPlayerSite.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private PlayerDbContext db = new PlayerDbContext();

        //
        // GET: /Users/
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        //
        // GET: /Users/Details/5
        [Authorize]
        public ActionResult Details(int id = 0)
        {

            if (id != WebSecurity.CurrentUserId && !System.Web.HttpContext.Current.User.IsInRole("Administrator"))
            {
                return RedirectToAction("Details", new { id = WebSecurity.CurrentUserId });
            }

            Player player = db.Players.Find(id);

            if (player == null)
            {
                return HttpNotFound();
            }
            var lists = player.tasksCompleted.GroupBy(t => t.task.isMilestone).OrderBy(g => g.Key).Select(g => g.ToList()).ToArray();
            List<PlayerToTask> tasks = new List<PlayerToTask>();
            List<PlayerToTask> milestones = new List<PlayerToTask>();
            if (lists.Count() == 2)
            {
                tasks = lists[0].OrderByDescending(s => s.when).ToList();
                milestones = lists[1].OrderByDescending(s => s.when).ToList();
            }else if(lists.Count() == 1){
                if (lists[0].First().task.isMilestone)
                {
                    milestones = lists[0].OrderByDescending(s => s.when).ToList();
                }
                else
                {
                    tasks = lists[0].OrderByDescending(s => s.when).ToList();
                }
            }
            return View(new PlayerDetails {player = player, tasks = tasks, milestones = milestones });
        }

        //
        // GET: /Users/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Users/Create

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        //
        // GET: /Users/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id = 0)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /Users/Edit/5

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        //
        // GET: /Users/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id = 0)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /Users/Delete/5

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}