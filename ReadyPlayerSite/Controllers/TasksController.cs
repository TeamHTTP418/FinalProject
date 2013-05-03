﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReadyPlayerSite.Controllers
{
    public class TasksController : Controller
    {
        //
        // GET: /Tasks/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Tasks/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Tasks/Create
        [Authorize(Roles="Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Tasks/Create

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
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
