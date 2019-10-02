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
    public class PlayersController : Controller
    {
        private Database1Entities db = new Database1Entities();

        // GET: Players
        public ActionResult Index(string searchString)
        {
            var players = db.Players.Include(p => p.Club).Include(b => b.Club.League);

            //filter by search string
            if (!String.IsNullOrEmpty(searchString))
            {


                players = players.Where(s => s.LastName.Contains(searchString) || s.Club.Team.Contains(searchString) || s.Club.League.LeagueName.Contains(searchString));
            }


            return View(players);

        }

        // GET: Players/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }

        // GET: Players/Create
        public ActionResult Create()
        {
            ViewBag.Clubid = new SelectList(db.Clubs, "Clubid", "Team");
            return View();
        }

        // POST: Players/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Playerid,FirstName,LastName,Nationality,dob,Clubid,Position,Picture")] Player player)
        {
            player.Likes = 0;
            player.Dislikes = 0;
            if (ModelState.IsValid)
            {
                db.Players.Add(player);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Clubid = new SelectList(db.Clubs, "Clubid", "Team", player.Clubid);
            return View(player);
        }

        // GET: Players/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            ViewBag.Clubid = new SelectList(db.Clubs, "Clubid", "Team", player.Clubid);
            return View(player);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Playerid,FirstName,LastName,Nationality,dob,Clubid,Position,Picture")] Player player)
        {
            if (ModelState.IsValid)
            {
                db.Entry(player).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Clubid = new SelectList(db.Clubs, "Clubid", "Team", player.Clubid);
            return View(player);
        }

        // GET: Players/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Player player = db.Players.Find(id);
            db.Players.Remove(player);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        public ActionResult Like(int id)

        {
            Player player = db.Players.Find(id);

            player.Likes++;

            db.SaveChanges();
            return RedirectToAction("index");


        }

        public ActionResult Dislike(int id)

        {
            Player player = db.Players.Find(id);

            player.Dislikes++;

            db.SaveChanges();
            return RedirectToAction("index");


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
