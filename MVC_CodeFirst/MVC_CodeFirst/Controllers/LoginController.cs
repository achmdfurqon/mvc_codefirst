using MVC_CodeFirst.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_CodeFirst.Controllers
{
    public class LoginController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login(string username, User u)
        {
            try
            {
                // TODO: Add login logic here
                var user = context.Users.Find(username);
                if (user!=null && user.Password==u.Password)
                {
                    return RedirectToAction("Home");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return View();
            }
        }
        // POST: Register 
        public ActionResult Register(User user)
        {
            try
            {
                // TODO: Add register logic here
                context.Users.Add(user);
                context.SaveChanges();
                return RedirectToAction("Register");
            }
            catch
            {
                return View();
            }
        }
    }
}