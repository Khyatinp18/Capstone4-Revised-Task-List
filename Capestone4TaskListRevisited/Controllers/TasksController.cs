using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Capestone4TaskListRevisited.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Capestone4TaskListRevisited.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {

        private readonly IdentityTaskListDbContext _context;

        public TasksController(IdentityTaskListDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Tasks> thisUsersTasks = _context.Tasks.Where(x => x.TaskOwnerId == id).ToList();
            return View(thisUsersTasks);
        }

        [HttpGet]
        public IActionResult AddTasks()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddTasks(Tasks newTask)
        {
            newTask.TaskOwnerId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (ModelState.IsValid)
            {
                _context.Tasks.Add(newTask);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View();

            }


        }

        [HttpGet]
        public IActionResult EditTask(int id)
        {
            Tasks foundTask = _context.Tasks.Find(id);
            if (foundTask != null)
            {
                return View(foundTask);
            }
            return View();
        }

        [HttpPost]
        public IActionResult EditTask(Tasks updatedTask)
        {
            // updatedTask.TaskOwnerId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Tasks dbtask = _context.Tasks.Find(updatedTask.Id);

            if (ModelState.IsValid)
            {
                dbtask.TaskDescription = updatedTask.TaskDescription;
                dbtask.DueDate = updatedTask.DueDate;
                dbtask.Completed = updatedTask.Completed;

                //May need to delete
                _context.Entry(dbtask).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.Update(dbtask);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult DeleteTask(int id)
        {
            Tasks found = _context.Tasks.Find(id);

            if (found != null)
            {
                _context.Tasks.Remove(found);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        public IActionResult FindTask()
        {
           
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Tasks> thisUsersTasks = _context.Tasks.Where(x => x.TaskOwnerId == id).ToList();
              thisUsersTasks = thisUsersTasks.OrderBy(x => x.DueDate).ToList();
            return View(thisUsersTasks);
        } 



    }
}
