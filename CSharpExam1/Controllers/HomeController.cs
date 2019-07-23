using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using CSharpExam1.Models;
using Microsoft.EntityFrameworkCore;

namespace CSharpExam1.Controllers
{
    public class HomeController : Controller
    {
        private context _dbConnector;

        public HomeController(context dbConnector)
        {
            _dbConnector = dbConnector;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            System.Console.WriteLine("index view");
            return View();
        }
        [HttpPost("register")]
        public IActionResult Register(Users users)
        {
            System.Console.WriteLine("register path");
            if (ModelState.IsValid)
            {
                System.Console.WriteLine("passed is valid");
                if (_dbConnector.Users.Any(u => u.Email == users.Email))
                {
                    ModelState.AddModelError("email", "email is already in use");
                    System.Console.WriteLine("entered not valid email");
                    return View("Index");
                }
                PasswordHasher<Users> Hasher = new PasswordHasher<Users>();
                users.Password = Hasher.HashPassword(users, users.Password);

                _dbConnector.Add(users);
                _dbConnector.SaveChanges();
                HttpContext.Session.SetInt32("UserId", users.UserId);
                HttpContext.Session.SetString("Name", users.Name);
                HttpContext.Session.SetString("Alias", users.Alias);
                System.Console.WriteLine("SUCCESS!!");
                return RedirectToAction("Success");
            }
            else
            {
                System.Console.WriteLine("failed registration");
                return View("Index");
            }
        }

        [HttpGet("login")]
        public IActionResult LoginUser()
        {
            System.Console.WriteLine("login");
            return View("login");
        }

        [HttpPost("login")]
        public IActionResult Login(login login)
        {
            System.Console.WriteLine("register post path");
            if (ModelState.IsValid)
            {
                var usersInDb = _dbConnector.Users.FirstOrDefault(u => u.Email == login.emaill);

                if (usersInDb == null)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    System.Console.WriteLine("login route email not valid");
                    return View("Index");
                }

                var hasher = new PasswordHasher<login>();
                var result = hasher.VerifyHashedPassword(login, usersInDb.Password, login.passwordl);

                if (result == 0)
                {
                    ModelState.AddModelError("Password", "Password is already in use");
                    System.Console.WriteLine("login route");
                    return View("Index");
                }
                    HttpContext.Session.SetInt32("UserId", usersInDb.UserId);
                    HttpContext.Session.SetString("Name", usersInDb.Name);
                    HttpContext.Session.SetString("Alias", usersInDb.Alias);
                    return RedirectToAction("Success");
            }
                else
                {
                    return View("Index");
                }
        }

        [HttpGet("success")]
        public IActionResult Success()
        {
            if(HttpContext.Session.GetInt32("UserId") != null)
            {
            IndexView viewindex = new IndexView()
            {
                AllMessages = _dbConnector.Messsages
                    .Include(Message => Message.Creator)
                    .Include(Message => Message.Likes).ThenInclude(users => users.Users)
                    .ToList(),
                AllUsers = _dbConnector.Users.ToList()
            };
            ViewBag.Name = HttpContext.Session.GetString("Name");
            ViewBag.Alias = HttpContext.Session.GetString("Alias");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            return View("success", viewindex);
            }
            else
            {
                return View("Index");
            }
        }

        [HttpPost("success")]
        public IActionResult PostMessage(IndexView Data)
        {
            Messages NewMessage = Data.NewMessage;

            if (ModelState.IsValid)
            {
                _dbConnector.Add(NewMessage);
                _dbConnector.SaveChanges();

                System.Console.WriteLine("Message success");
                return RedirectToAction("success");
            }
            else
            {
                IndexView viewindex = new IndexView()
                {
                    AllMessages = _dbConnector.Messsages
                    .Include(Message => Message.Creator)
                    .Include(Message => Message.Likes).ThenInclude(users => users.Users)
                    .ToList(),
                    AllUsers = _dbConnector.Users.ToList()
                };
                ViewBag.Name = HttpContext.Session.GetString("Name");
                ViewBag.Alias = HttpContext.Session.GetString("Alias");
                ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
                return View("success", viewindex);
            }
        }

        [HttpGet("/success/addassociation/{MessageId}")]
        public IActionResult CreateUserAssociation(int MessageId)
        {
            int UserId = (int)HttpContext.Session.GetInt32("UserId");
            int mid = MessageId;
            Associations Likes = new Associations()
            {
                MessageId = MessageId,
                UserId = UserId
            };

            if (ModelState.IsValid)
            {
                _dbConnector.Add(Likes);
                _dbConnector.SaveChanges();
                System.Console.WriteLine("added");
                return RedirectToAction("Success");
            }
            else
            {
                IndexView viewindex = new IndexView()
                {
                    AllMessages = _dbConnector.Messsages
                    .Include(Message => Message.Creator)
                    .Include(Message => Message.Likes).ThenInclude(Associations => Associations.Users)
                    .ToList(),
                    AllUsers = _dbConnector.Users.ToList()
                };
                ViewBag.Name = HttpContext.Session.GetString("Name");
                ViewBag.Alias = HttpContext.Session.GetString("Alias");
                ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
                System.Console.WriteLine("notadded");
                return View("success", viewindex);
            }
        }

        [HttpGet("/profile/{id}")]
        public IActionResult UserProfile(int id)
        {
            Users ThisUser =_dbConnector.Users.Include(m => m.Messages).Include(l => l.Likes).FirstOrDefault(u => u.UserId == id);

            return View("profile", ThisUser);
        }

        [HttpGet("likes/{MessageId}")]
        public IActionResult ViewLikes(int MessageId)
        {
                    Messages usermessage = _dbConnector.Messsages
                    .Include(Message => Message.Creator)
                    .Include(Message => Message.Likes).ThenInclude(Likes => Likes.Users).FirstOrDefault(Message => Message.MessageId == MessageId);

                    return View("likes", usermessage);
        }

        [HttpGet("/deletemessage/{messageId}")]
        public IActionResult DeleteMessage(int messageId)
        {
            Messages retrievedmessage = _dbConnector.Messsages.SingleOrDefault(message => message.MessageId == messageId);
            _dbConnector.Messsages.Remove(retrievedmessage);
            _dbConnector.SaveChanges();

            return RedirectToAction("success");
        }

        [HttpGet]
        [Route("/logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index");
        }

    }
}