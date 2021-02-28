using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoMVC.Models;

namespace ToDoMVC.Controllers
{
    public class ToDoCapstoneController : Controller
    {
        private readonly ToDoCapstoneContext _context;

        public ToDoCapstoneController(ToDoCapstoneContext context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult ViewToDos()
        {
            //I would love some feedback on how to improve this method of getting the current logged-in user.
            //I'm aware this this is a pretty hacky and ugly solution. Unfortunately, this is the only way
            //I could get it to work properly
            List<AspNetUsers> loggedIn = _context.AspNetUsers.Include(x => x.ToDos).Where(x => x.UserName == User.Identity.Name).ToList();
            return View(loggedIn[0]);
        }

        [Authorize]
        public IActionResult AddToDo()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddToDo(ToDos t)
        {
            if (t.DueDate == null)
            {
                t.DueDate = DateTime.MaxValue;
            }
            t.Completed = false;
            List<AspNetUsers> loggedIn = _context.AspNetUsers.Where(x => x.UserName == User.Identity.Name).ToList();
            t.UserId = loggedIn[0].Id;
            _context.ToDos.Add(t);
            _context.SaveChanges();

            return RedirectToAction("ViewToDos");
        }

        [Authorize]
        public IActionResult UpdateCompletion(int Id)
        {
            ToDos toUpdate = _context.ToDos.Where(x => x.Id == Id).ToList()[0];
            toUpdate.Completed = !toUpdate.Completed;
            _context.SaveChanges();

            return RedirectToAction("AddToDo");
        }

        [Authorize]
        public IActionResult DeleteItem(int Id)
        {
            //Passes the appropraite ToDo into the view, as well as storing the Id in temp data
            ToDos toUpdate = _context.ToDos.Where(x => x.Id == Id).ToList()[0];
            TempData["Id"] = toUpdate.Id;

            return View(toUpdate);
        }

        [Authorize]
        [HttpPost]
        public IActionResult DeleteItem(string option)
        {
            if (option.Equals("yes"))
            {
                //Messy implementation, but this re-grabs the ToDo using the Id in temp data. Then, removes the ToDo and saves changes
                //Not sure if there is a cleaner way to cast the temp data Id to an int, but I'd love to know if there is.
                ToDos toDelete = _context.ToDos.Where(x => x.Id == int.Parse(TempData["Id"].ToString())).ToList()[0];
                _context.ToDos.Remove(toDelete);
                _context.SaveChanges();
            }

            return RedirectToAction("AddToDo");
        }
    }
}
