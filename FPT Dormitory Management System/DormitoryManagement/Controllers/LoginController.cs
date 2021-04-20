using DormitoryManagement.DAL;
using DormitoryManagement.Models;
using DormitoryManagement.Secure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DormitoryManagement.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        // Done
        public ActionResult Index() {
            if (Session["Account"] != null) {
                if ((Session["Account"] as Account).IsManager) { return Redirect("Manager"); }
                return Redirect("Student");
            }
            return View();
        }

        // POST: Login
        // Done
        [HttpPost]
        public ActionResult Index(string username, string password) {
            AccountDAO accountDao = new AccountDAO();
            Account account = accountDao.GetAccount(username, password);
            if(account is null) {
                ViewBag.Error = "Wrong username or password. Try again!";
                return View();
            }
            if (!account.IsActive) {
                ViewBag.Error = "Your Account is deactive. Contact to Admintrator to the detail ktx@fpt.edu.vn";
                return View();
            }

            Session["Account"] = account;
            if (account.IsManager) {
                return Redirect("Manager");
            }
            return Redirect("Student");
        }
    }
}