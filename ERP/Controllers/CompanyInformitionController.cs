using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ERP.Models;
using PagedList;
namespace ERP.Controllers
{
    public class CompanyInformitionController : Controller
    {
        private DataCollectionEntities db = new DataCollectionEntities();

        // GET: /CompanyInformition/
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DesigSortParm = sortOrder == "designation" ? "designation_desc" : "designation";
            ViewBag.SectionSortParm = sortOrder == "section" ? "section_desc" : "section";
            //List<EmpView> emps = new List<EmpView>();
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            User LoggedInUser = Session["MUser"] as User;
            List<CompanyInfo> com = new List<CompanyInfo>();
            com = db.CompanyInfoes.Where(aa => aa.UserID == LoggedInUser.UID).ToList();
            // List<EmpView> emps = db.EmpViews.ToList();
            com = com.OrderByDescending(aa => aa.CID).ToList();
            ViewBag.CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                try
                {

                    com = com.Where(s => s.Name.ToUpper().Contains(searchString.ToUpper())
                        || s.Name.ToUpper().Contains(searchString.ToUpper()) || s.Website.ToString().Contains(searchString)
                    ).OrderByDescending(aa => aa.Email).ToList();
                }
                catch (Exception ex)
                {

                }
            }

            switch (sortOrder)
            {
                case "name_desc":
                    com = com.OrderByDescending(s => s.Name).ToList();
                    break;
                case "designation_desc":
                    com = com.OrderByDescending(s => s.Address).ToList();
                    break;
                case "designation":
                    com = com.OrderBy(s => s.Website).ToList();
                    break;
                case "section_desc":
                    com = com.OrderByDescending(s => s.Employee).ToList();
                    break;
                case "section":
                    com = com.OrderBy(s => s.Year).ToList();
                    break;
                case "esection":
                    com = com.OrderBy(s => s.Email).ToList();
                    break;
                case "desection":
                    com = com.OrderBy(s => s.Phone).ToList();
                    break;
                default:
                    com = com.OrderBy(s => s.Name).ToList();
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(com.ToPagedList(pageNumber, pageSize));       
        }

        // GET: /CompanyInformition/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyInfo companyinfo = db.CompanyInfoes.Find(id);
            if (companyinfo == null)
            {
                return HttpNotFound();
            }
            return View(companyinfo);
        }

        // GET: /CompanyInformition/Create
        public ActionResult Create()
        {
            CompanyInfo cc = new CompanyInfo();
            cc.City = Session["City"].ToString();
            cc.Country = Session["Country"].ToString();
            cc.Tags = Session["Tags"].ToString();
            return View(cc);
        }

        // POST: /CompanyInformition/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CID,Name,Address,City,Country,Website,Phone,Fax,Tags,Remarks,CreateDate,UserID,Email,Year,Employee")] CompanyInfo companyinfo)
        {         
                if (ModelState.IsValid)
                {
                    
                    if (companyinfo.Name != null && companyinfo.Website != null)
                    {
                        
                            if (db.CompanyInfoes.Where(aa => aa.Name == companyinfo.Name && aa.Website == companyinfo.Website).Count() == 0)
                            {
                                Session["City"] = companyinfo.City;
                                Session["Country"] = companyinfo.Country;
                                Session["Tags"] = companyinfo.Tags;
                                int _userID = Convert.ToInt32(Session["LogedUserID"].ToString());
                                companyinfo.UserID = _userID;
                                db.CompanyInfoes.Add(companyinfo);
                                db.SaveChanges();
                                string pname = Request.Form["pname"].ToString();
                                string designation = Request.Form["designation"].ToString();
                                string department = Request.Form["department"].ToString();
                                string eemail = Request.Form["eemail"].ToString();
                                string pphone = Request.Form["pphone"].ToString();
                                string linkedin = Request.Form["linkedin"].ToString();


                                if (pname!="" || eemail!="")
                                {
                                    PersonInfo person = new PersonInfo();
                                    person.Name = pname;
                                    person.Designation = designation;
                                    person.Department = department;
                                    person.EmailPersonal = eemail;
                                    person.PhonePersonal = pphone;
                                    person.CompanyID = companyinfo.CID;
                                    person.Linkedin = linkedin;
                                    db.PersonInfoes.Add(person);
                                    db.SaveChanges(); 
                                }
                                return RedirectToAction("Create");
                            }                          
                        
                        else
                            ViewBag.ErrorMessage = "Company already exist";
                    }
                    else
                        ViewBag.ErrorMessage = "Please Fill All Fields";
                }

                return View(companyinfo);
                       
        }

        // GET: /CompanyInformition/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyInfo companyinfo = db.CompanyInfoes.Find(id);
            if (companyinfo == null)
            {
                return HttpNotFound();
            }
            return View(companyinfo);
        }

        // POST: /CompanyInformition/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CID,Name,Address,City,Country,Website,Phone,Fax,Tags,Remarks,CreateDate,UserID,Email,Year,Employee")] CompanyInfo companyinfo)
        {
            if (ModelState.IsValid)
            {
                int _userID = Convert.ToInt32(Session["LogedUserID"].ToString());
                companyinfo.UserID = _userID;
                db.Entry(companyinfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(companyinfo);
        }

        // GET: /CompanyInformition/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyInfo companyinfo = db.CompanyInfoes.Find(id);
            if (companyinfo == null)
            {
                return HttpNotFound();
            }
            return View(companyinfo);
        }

        // POST: /CompanyInformition/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CompanyInfo companyinfo = db.CompanyInfoes.Find(id);
            db.CompanyInfoes.Remove(companyinfo);
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
