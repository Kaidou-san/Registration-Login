using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LogRegJune.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace LogRegJune.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context;
        public HomeController(MyContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(User newUser)
        {
            if (ModelState.IsValid)
            {
                //Email must be unique!
                if (_context.Users.Any(e => e.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "Email Taken already");
                    return View("Index");
                }
                else
                {
                    // We have verification that all is well and we can add to the database
                    // WE NEED TO HASH OUR PW
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                    _context.Add(newUser);
                    _context.SaveChanges();
                    return RedirectToAction("Success");
                }
            }
            else
            {
                return View("Index");
            }
        }

        [HttpPost("login")]
        public IActionResult Login(LoginUser logUser)
        {
            if (ModelState.IsValid)
            {
                //VERIFY EMAIL GIVEN IS THE DATABASE
                User userInDb = _context.Users.FirstOrDefault(u => u.Email == logUser.LEmail);
                if (userInDb == null)
                {
                    ModelState.AddModelError("LEmail", "Invalid login attempt");
                    return View("Index");
                }
                //CHECK THE PW VS THE PW IN THE USER HAS IN THE DATABASE
                PasswordHasher<LoginUser> Hasher = new PasswordHasher<LoginUser>();
                PasswordVerificationResult result = Hasher.VerifyHashedPassword(logUser, userInDb.Password, logUser.LPassword);
                if (result == 0)
                {
                    //IF ITS 0 THAT MEANS WE FAILED TO VERIFY
                    ModelState.AddModelError("LEmail", "Invalid login attempt");
                    return View("Index");
                }
                else
                {
                    HttpContext.Session.SetInt32("loggedIn", userInDb.UserId);
                    //AFTER SUCCESSFULLY DO BOTH, WE HEAD TO THE SUCCESS PAGE
                    return RedirectToAction("Success");
                }
            }
            else
            {
                return View("Index");
            }
        }

        [HttpGet("Success")]
        public IActionResult Success()
        {
            int? loggedIn = HttpContext.Session.GetInt32("loggedIn");
            if (loggedIn != null)
            {
                ViewBag.User = _context.Users.FirstOrDefault(a => a.UserId == (int)loggedIn);
                return View();
            }else{
                ModelState.AddModelError("LEmail", "Invalid login attempt!");
                return View("Index");
            }
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }


    }
}
