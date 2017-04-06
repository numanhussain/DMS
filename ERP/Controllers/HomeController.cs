using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERP.Models;

namespace ERP.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            try
             {
                if (Session["LogedUserID"] == null)
                {
                    Session["LogedUserID"] = "";
                    Session["MUser"] = "0";
                    Session["City"] = "Islamabad";
                    Session["Country"] = "Pakistan";
                    Session["Tags"] = "Textile";
                    return View();
                }
                else if (Session["LogedUserID"].ToString() == "")
                {
                    return View();
                }
                else
                {
                    return View("AfterLogin");
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(ERP.Models.User u)
        {
            try
            {
                if (ModelState.IsValid) // this is check validity
                {
                    using (ERP.Models.DataCollectionEntities dc = new ERP.Models.DataCollectionEntities())
                    {
                        List<User> users = new List<User>();
                        users = dc.Users.Where(aa => aa.Name == u.Name && aa.Password == u.Password).ToList();
                        //login for emplioyee
                        if (users.Count>0)
                        {

                            Session["MUser"] = users.FirstOrDefault();
                            Session["LogedUserID"] = users.FirstOrDefault().UID;
                            return RedirectToAction("AfterLogin");
                        }
                    }
                }
                return RedirectToAction("index");

            }
            catch (Exception ex)
            {
                ViewBag.Message = "There seems to be a problem with the network. Please contact your network administrator";
                return RedirectToAction("Index");
            }
        }
        public ActionResult AfterLogin()
        {
            try
            {
                if (Session["LogedUserID"] != null)
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                return View("Index");
            }
        }
        public ActionResult Logout()
        {
            try
            {
                Session["LogedUserID"] = null;
                Session["MUser"] = null;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("Index");
            }
        }
	}
}