﻿using ReadyPlayerSite.Models;
using ReadyPlayerSite.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data;
using System.Net;
using System.Security.Cryptography;

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

        public HttpStatusCodeResult AppSubmit(byte[] o = null)
        {
            RSACryptoServiceProvider rsa = getRSAProvider();
            byte[] incoming = rsa.Decrypt(o, false);
            string[] arguments = null;
            using (MemoryStream m = new MemoryStream(incoming))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    arguments = reader.ReadString().Split('|');
                }
            }
            if (arguments.Length == 2)
            {
                string token = arguments[0];
                string eid = arguments[1];
                Task t = db.Tasks.Where(s => s.token == token).FirstOrDefault();
                Player p = db.Players.Where(s => s.eid == eid).FirstOrDefault();
                if (t != null && p != null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                } 
            }
            return new HttpStatusCodeResult(HttpStatusCode.PreconditionFailed);
        }

        public ActionResult QRSubmit(object o = null)
        {

            return View();
        }

        public string getPublicKey()
        {
            return getRSAProvider().ToXmlString(false);
        }

        public RSACryptoServiceProvider getRSAProvider()
        {
            CspParameters cspParams = new CspParameters(1);
            cspParams.KeyContainerName = "ReadyPlayerSiteContainer";
            cspParams.Flags = CspProviderFlags.UseMachineKeyStore;
            cspParams.ProviderName = "Microsoft Strong Cryptographic";
            return new RSACryptoServiceProvider(cspParams);
        }

        //
        // GET: /Tasks/Create
        [Authorize(Roles = "Administrator")]
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
            if (task.isMilestone == false)
            {
                task.maxCompletedBonus = null;
                task.numberCompleted = null;
                task.iconName = null;
                task.bonusPoints = null;
            }
            if (ModelState.IsValid)
            {
                if (icon != null && icon.ContentLength > 0 && icon.ContentType.StartsWith("image/") && task.isMilestone)
                {
                    string iconName = Path.GetFileName(icon.FileName);
                    string path = Path.Combine(Server.MapPath("~/Content/icons"), iconName);
                    icon.SaveAs(path);
                    task.iconName = iconName;
                }
                task.token = Guid.NewGuid().ToString();
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

        //
        // GET: /Tasks/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id = 0)
        {
            Task task = db.Tasks.Find(id);

            var scoreTypeSelect = from TaskType e in Enum.GetValues(typeof(TaskType))
                                  select new { Id = e, Name = e.ToString() };
            ViewBag.typeSelect = new SelectList(scoreTypeSelect, "Id", "Name", task.type.ToString());

            DirectoryInfo iconDirectory = new DirectoryInfo(Server.MapPath(@"~/Content/icons"));
            var icons = from FileInfo f in iconDirectory.GetFiles()
                        select new { Id = Path.GetFileName(f.Name) };
            ViewBag.iconSelect = new SelectList(icons, "Id", "Id");

            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        //
        // POST: /Tasks/Edit/5

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(Task task, HttpPostedFileBase icon)
        {
            if (task.isMilestone == false)
            {
                task.maxCompletedBonus = null;
                task.numberCompleted = null;
                task.iconName = null;
                task.bonusPoints = null;
            }
            if (icon != null && icon.ContentLength > 0 && icon.ContentType.StartsWith("image/") && task.isMilestone)
            {
                string iconName = Path.GetFileName(icon.FileName);
                string path = Path.Combine(Server.MapPath("~/Content/icons"), iconName);
                icon.SaveAs(path);
                task.iconName = iconName;
            }
            if (ModelState.IsValid)
            {
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var scoreTypeSelect = from TaskType e in Enum.GetValues(typeof(TaskType))
                                  select new { Id = e, Name = e.ToString() };
            ViewBag.typeSelect = new SelectList(scoreTypeSelect, "Id", "Name", task.type.ToString());

            DirectoryInfo iconDirectory = new DirectoryInfo(Server.MapPath(@"~/Content/icons"));
            var icons = from FileInfo f in iconDirectory.GetFiles()
                        select new { Id = Path.GetFileName(f.Name) };
            ViewBag.iconSelect = new SelectList(icons, "Id", "Id");
            return View(task);
        }

        //
        // GET: /Tasks/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id = 0)
        {
            Task task = db.Tasks.Find(id);

            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        //
        // POST: /Tasks/Delete/5

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(Task task, bool removeIcon = false)
        {
            if (removeIcon)
            {
                DirectoryInfo iconDirectory = new DirectoryInfo(Server.MapPath(@"../Content/icons"));
                FileInfo icon = iconDirectory.EnumerateFiles().Where(s => s.Name == task.iconName).FirstOrDefault();
                if (icon != null)
                {
                    icon.Delete();
                }
            }
            db.Tasks.Remove(task);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
