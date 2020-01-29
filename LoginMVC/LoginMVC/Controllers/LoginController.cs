using LoginMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BCrypt.Net;
using System.Net.Mail;
using System.Net;
using System.Configuration;

namespace LoginMVC.Controllers
{
    public class LoginController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        // GET: Roles
        public ActionResult Index()
        {
            var list = context.Accounts.ToList();
            return View(list);
        }

        // GET: Roles/Details/5
        public ActionResult Details(int id)
        {
            var list = context.Accounts.Find(id);
            return View(list);
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        [HttpPost]
        public ActionResult Create(Account account)
        {
            try
            {
                // TODO: Add insert logic here
                var pwd = BCrypt.Net.BCrypt.HashPassword(account.password, BCrypt.Net.BCrypt.GenerateSalt());
                account.password = pwd;
                context.Accounts.Add(account);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Roles/Edit/5
        public ActionResult Edit(int id)
        {
            var edit = context.Accounts.Find(id);
            return View(edit);
        }

        // POST: Roles/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Account account)
        {
            try
            {
                // TODO: Add update logic here
                var edit = context.Accounts.Find(id);
                edit.username = account.username;
                edit.email = account.email;
                var pwd = BCrypt.Net.BCrypt.HashPassword(account.password, BCrypt.Net.BCrypt.GenerateSalt());
                edit.password = pwd;
                edit.role = account.role;
                context.Entry(edit).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Roles/Delete/5
        public ActionResult Delete(int id)
        {
            var delete = context.Accounts.Find(id);
            return View(delete);
        }

        // POST: Roles/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Account account)
        {
            try
            {
                // TODO: Add delete logic here
                var delete = context.Accounts.Find(id);
                context.Accounts.Remove(delete);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Login
        [HttpPost]
        public ActionResult Login(Account account)
        {
            // TODO: Add login logic here
            //var user = context.Accounts.FirstOrDefault(model => model.email.Equals(account.email));
            var user = context.Accounts.Find(account.email);
            if (user != null && BCrypt.Net.BCrypt.Verify(account.password, user.password))
            {
                    //Session["id"] = user.id;
                    //Session.Add("username", user.username);
                    return RedirectToAction("Index","Login");                                              
            }
            else
            {
                return RedirectToAction("Login","Login");
            }
        }

        public ActionResult Logout()
        {
            Session.Remove("id");
            Session.Remove("username");
            return RedirectToAction("Index","Login");
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View("Register", new Account());
        }

        // POST: Register 
        [HttpPost]      
        public ActionResult Register(Account account)
        {
            // TODO: Add register logic here
            var passwd = account.password;
                account.password = BCrypt.Net.BCrypt.HashPassword(account.password); 
                context.Accounts.Add(account);
                context.SaveChanges();
            MailMessage mailMessage = new MailMessage("furqon2993@gmail.com",account.email);
            mailMessage.Subject = "[Password]" + DateTime.Now.ToString();
            mailMessage.Body = "Hi" + account.username + "\n This is Your New Password " + passwd;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;

            NetworkCredential network = new NetworkCredential(ConfigurationManager.AppSettings["mailAccount"], ConfigurationManager.AppSettings["mailPassword"]);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = network;
            smtp.Send(mailMessage);
            ViewBag.Message = "Password has been sent. Check your email";
                return RedirectToAction("Index","Login");
            
        }

        public ActionResult Dashboard()
        {
            return View();
        }
    }
}