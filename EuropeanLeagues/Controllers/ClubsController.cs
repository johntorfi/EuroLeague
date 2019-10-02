using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EuropeanLeagues.Models;

namespace EuropeanLeagues.Controllers
{
    public class ClubsController : Controller
    {
        private Database1Entities db = new Database1Entities();

        // GET: Clubs
        public ActionResult Index(string searchString, string currentFilter)
        {
            var clubs = from c in db.Clubs
                        select c;

            //filter by search string
            if (!String.IsNullOrEmpty(searchString))
            {
                clubs = clubs.Where(s => s.Team.Contains(searchString));

            }

            return View(clubs);
        }
        // GET: Clubs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Club club = db.Clubs.Find(id);




            if (club == null)
            {
                return HttpNotFound();
            }
            return View(club);
        }

        // GET: Clubs/Create
        public ActionResult Create(string returnView)
        {
            ViewBag.Lid = new SelectList(db.Leagues, "Lid", "LeagueName");
            if (returnView != null)
            {
                TempData["returnView"] = returnView;
                return View();
            }
            return View();
        }

        // POST: Clubs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Clubid,Team,Manager,Venue,Lid,ImageUrl")] Club club)
        {
            if (club.ImageUrl == null)
            {
                club.ImageUrl = "~/Content/no-image-thumb.gif";
            }
            if (ModelState.IsValid)
            {
                db.Clubs.Add(club);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Lid = new SelectList(db.Leagues, "Lid", "Logo", club.Lid);
            return View(club);
        }

        // GET: Clubs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Club club = db.Clubs.Find(id);
            if (club == null)
            {
                return HttpNotFound();
            }
            ViewBag.Lid = new SelectList(db.Leagues, "Lid", "LeagueName", club.Lid);
            return View(club);
        }

        // POST: Clubs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Clubid,Team,Manager,Venue,Lid,ImageUrl")] Club club)
        {
            if (club.ImageUrl == null)
            {
                club.ImageUrl = "~/Content/no-image-thumb.gif";
            }
            if (ModelState.IsValid)
            {
                db.Entry(club).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Lid = new SelectList(db.Leagues, "Lid", "LeagueName", club.Lid);
            return View(club);
        }

        // GET: Clubs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Club club = db.Clubs.Find(id);
            if (club == null)
            {
                return HttpNotFound();
            }
            return View(club);
        }

        // POST: Clubs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            int AssociatedPlayerNumber = 0;

            Club club = db.Clubs.Find(id);
            AssociatedPlayerNumber = club.Players.Count();
            if (AssociatedPlayerNumber == 0)
            {
                db.Clubs.Remove(club);
                db.SaveChanges();
            }
            else
            {
                TempData["MyErrorMessage"] = "You cannot delete this club while there are players linked to them.";
                TempData["DeletionError"] = true;
                return RedirectToAction("Delete", new { id = id });
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
