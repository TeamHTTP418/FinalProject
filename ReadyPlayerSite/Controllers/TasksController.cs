using ReadyPlayerSite.Models;
using ReadyPlayerSite.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace ReadyPlayerSite.Controllers
{
    public class TasksController : Controller
    {
        private PlayerDbContext db = new PlayerDbContext();

        //
        // GET: /Tasks/

        public ActionResult Index()
        {
            var lists = db.Tasks.ToList().GroupBy(t => t.isMilestone).OrderBy(g => g.Key).Select(g => g.ToList()).ToArray();
            return View(new TasksAndMilestones { tasks = lists[0], milestones = lists[1] });
        }

        //
        // GET: /Tasks/Details/5

        public ActionResult Details(int id = 0)
        {
            Task t = db.Tasks.Find(id);
            if (t == null)
            {
                return HttpNotFound();
            }
            return View(t);
        }

        //
        // GET: /Tasks/Create
        [Authorize(Roles="Administrator")]
        public ActionResult Create()
        {
            var scoreTypeSelect = from TaskType e in Enum.GetValues(typeof(TaskType))
                                  select new { Id = e, Name = e.ToString() };
            ViewBag.typeSelect = new SelectList(scoreTypeSelect, "Id", "Name", null);

            DirectoryInfo iconDirectory = new DirectoryInfo(Server.MapPath(@"../Content/icons"));
            var icons = from FileInfo f in iconDirectory.GetFiles()
                        select new { Id = Path.GetFileName(f.Name) };
            ViewBag.iconSelect = new SelectList(icons, "Id", "Id");
            return View();
        }

        //
        // POST: /Tasks/Create

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create(Task task, HttpPostedFileBase icon)
        {
            task.numberCompleted = 0;
            if (ModelState.IsValid)
            {
                
                
                if (icon != null && icon.ContentLength > 0 && icon.ContentType.StartsWith("image/"))
                {
                    string iconName = Path.GetFileName(icon.FileName);
                    string path = Path.Combine(Server.MapPath("~/Content/icons"), iconName);
                    icon.SaveAs(path);
                    task.iconName = iconName;
                }
                db.Tasks.Add(task);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //Something was wrong, reshow the create view
            var scoreTypeSelect = from TaskType e in Enum.GetValues(typeof(TaskType))
                                  select new { Id = e, Name = e.ToString() };
            ViewBag.typeSelect = new SelectList(scoreTypeSelect, "Id", "Name", null);

            DirectoryInfo iconDirectory = new DirectoryInfo(Server.MapPath(@"../Content/icons"));
            var icons = from FileInfo f in iconDirectory.GetFiles()
                        select new { Id = Path.GetFileName(f.Name) };
            ViewBag.iconSelect = new SelectList(icons, "Id", "Id");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public void LoadIcon(HttpPostedFileBase file, int num = 0)
        {
            string fileName = Path.GetFileName(file.FileName);
            string path = Path.Combine(Server.MapPath("~/Content/icons"), fileName);
            file.SaveAs(path);
        }

        //
        // GET: /Tasks/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Tasks/Edit/5

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Tasks/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Tasks/Delete/5

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
