using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ERP.Models;

namespace ERP.Controllers
{
    public class PersonInfoController : Controller
    {
        private DataCollectionEntities db = new DataCollectionEntities();

        // GET: /PersonInfo/
        public ActionResult Index(int? id)
        {
            var personinfoes = db.PersonInfoes.Where(aa => aa.CompanyID == id).Include(p => p.CompanyInfo);
            return View(personinfoes.ToList());
        }

        // GET: /PersonInfo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonInfo personinfo = db.PersonInfoes.Find(id);
            if (personinfo == null)
            {
                return HttpNotFound();
            }
            return View(personinfo);
        }

        // GET: /PersonInfo/Create
        public ActionResult Create()
        {
            ViewBag.CompanyID = new SelectList(db.CompanyInfoes, "CID", "Name");
            return View();
        }

        // POST: /PersonInfo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="PID,Name,Designation,Department,Address,CompanyID,City,Country,EmailCompany,EmailPersonal,PhoneOffice,PhonePersonal,Linkedin,Tags,Remarks,CreateDate,UserID")] PersonInfo personinfo)
        {
            if (ModelState.IsValid)
            {
                int _userID = Convert.ToInt32(Session["LogedUserID"].ToString());
                personinfo.UserID = _userID;
                db.PersonInfoes.Add(personinfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CompanyID = new SelectList(db.CompanyInfoes, "CID", "Name", personinfo.CompanyID);
            return View(personinfo);
        }

        // GET: /PersonInfo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonInfo personinfo = db.PersonInfoes.Find(id);
            if (personinfo == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyID = new SelectList(db.CompanyInfoes, "CID", "Name", personinfo.CompanyID);
            return View(personinfo);
        }

        // POST: /PersonInfo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="PID,Name,Designation,Department,Address,CompanyID,City,Country,EmailCompany,EmailPersonal,PhoneOffice,PhonePersonal,Linkedin,Tags,Remarks,CreateDate,UserID")] PersonInfo personinfo)
        {
            if (ModelState.IsValid)
            {
                int _userID = Convert.ToInt32(Session["LogedUserID"].ToString());
                personinfo.UserID = _userID;
                db.Entry(personinfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyID = new SelectList(db.CompanyInfoes, "CID", "Name", personinfo.CompanyID);
            return View(personinfo);
        }

        // GET: /PersonInfo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonInfo personinfo = db.PersonInfoes.Find(id);
            if (personinfo == null)
            {
                return HttpNotFound();
            }
            return View(personinfo);
        }

        // POST: /PersonInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PersonInfo personinfo = db.PersonInfoes.Find(id);
            db.PersonInfoes.Remove(personinfo);
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
    }
}
