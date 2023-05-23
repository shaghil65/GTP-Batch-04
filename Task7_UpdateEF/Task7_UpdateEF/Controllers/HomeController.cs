﻿using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Task7_UpdateEF.Data;
using Task7_UpdateEF.Models;

namespace Task7_UpdateEF.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _db;
        //we can create obj of context class but its a legacy method(Tightly Coupled) instead we use implementation of 
        //context class because we registered a service in program class
        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View(_db.Persons.ToList());
        }
        public IActionResult Edit(int id)
        {
            if (id > 0)
            {
                var person = _db.Persons.Find(id);
                if (person != null)
                {
                    return View(person);
                }
            }
            return View();
        }

        [HttpPost, AutoValidateAntiforgeryToken]
        public IActionResult Edit(Person person)
        {
            if (ModelState.IsValid)
            {
                _db.Persons.Update(person);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}