using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LonghornBank.Models;
using Microsoft.AspNet.Identity;

namespace LonghornBank.Controllers
{
    public class MembersController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Members
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Members.ToList());
        }

        // GET: Members/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // GET: Members/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MemberID,FirstName,LastName,Email,phoneNumber,OkToText,Major")] Member member)
        {
            if (ModelState.IsValid)
            {
                db.Members.Add(member);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(member);
        }

        // GET: Members/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            if (member.UserId != User.Identity.GetUserId() && !User.IsInRole("Admin"))
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.AllEvents = GetAllEvents(member);
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MemberID,FirstName,LastName,Email,phoneNumber,OkToText,Major")] Member member, int[] SelectedEvents)
        {
            if (ModelState.IsValid)
            {
                //Find associated member
                Member memberToChange = db.Members.Find(member.MemberID);

                if (memberToChange.UserId != User.Identity.GetUserId() && !User.IsInRole("Admin"))
                {
                    return RedirectToAction("Login", "Account");
                }

                //change events
                //remove any existing events
                memberToChange.Events.Clear();

                //if there are members to add, add them
                if (SelectedEvents != null)
                {
                    foreach (int eventID in SelectedEvents)
                    {
                        Event eventToAdd = db.Events.Find(eventID);
                        memberToChange.Events.Add(eventToAdd);
                    }
                }

                //update the rest of the fields
                memberToChange.FirstName = member.FirstName;
                memberToChange.LastName = member.LastName;
                memberToChange.Email = member.Email;
                memberToChange.phoneNumber = member.phoneNumber;
                memberToChange.OkToText = member.OkToText;
                memberToChange.Major = member.Major;

                db.Entry(memberToChange).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AllEvents = GetAllEvents(member);
            return View(member);
        }

        // GET: Members/Delete/5
        [Authorize(Roles ="Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Members/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Member member = db.Members.Find(id);
            db.Members.Remove(member);

            // Remove the user at the same time
            AppUser user = db.Users.Find(member.UserId);
            db.Users.Remove(user);

            db.SaveChanges();
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

        public MultiSelectList GetAllEvents(Member member)
        {
            //find the list of events
            var query = from e in db.Events
                        select e;
            //convert to list and execute query
            List<Event> allEvents = query.ToList();

            //create list of selected members
            List<Int32> SelectedEvents = new List<Int32>();

            //Loop through list of members and add MemberId
            foreach (Event e in member.Events)
            {
                SelectedEvents.Add(e.EventID);
            }

            //convert to multiselect
            MultiSelectList allEventsList = new MultiSelectList(allEvents, "EventID", "EventTitle", SelectedEvents);

            return allEventsList;
        }
    }
}
