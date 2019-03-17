using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tasks.Data;
using Tasks.Models;
using System.Security.Claims;

namespace Tasks.Controllers
{
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tasks
        public async Task<IActionResult> Index(string q, int? milestone, int? closed, string assignee, string author)
        {
            var tasks = from t in _context.Task.Include(t => t.Assignee).Include(t => t.Milestone).Include(t => t.Author)
                        select t;

            if (!string.IsNullOrEmpty(q))
            {
                tasks = tasks.Where(s => s.Title.Contains(q));
            }
            if (milestone.HasValue)
            {
                tasks = tasks.Where(s => s.MilestoneId.Equals(milestone));
            }
            if (!string.IsNullOrEmpty(assignee))
            {
                tasks = tasks.Where(s => s.AssigneeId.Equals(assignee));
            }
            if (!string.IsNullOrEmpty(author))
            {
                tasks = tasks.Where(s => s.AuthorId.Equals(author));
            }
            if (!closed.HasValue)
            {
                tasks = tasks.Where(s => s.Closed.Equals(false));
            }

            ViewData["Assignees"] = new SelectList(_context.Users, "Id", "UserName", !string.IsNullOrEmpty(assignee) ? assignee : "");
            ViewData["Authors"] = new SelectList(_context.Users, "Id", "UserName", !string.IsNullOrEmpty(author) ? author : "");
            ViewData["q"] = q;
            ViewData["closed"] = closed.HasValue ? true : false;
            return View(await tasks.ToListAsync());
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Task
                .Include(t => t.Assignee)
                .Include(t => t.Author)
                .Include(t => t.Milestone)
                .Include("Logs.User")
                .FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // GET: Tasks/Create
        public IActionResult Create(int? milestone, string assignee)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            ViewData["AssigneeId"] = new SelectList(_context.Users, "Id", "UserName", !string.IsNullOrEmpty(assignee) ? assignee : "");
            ViewData["MilestoneId"] = new SelectList(_context.Milestone, "Id", "Title", milestone.HasValue ? milestone : 0);
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,AssigneeId,MilestoneId")] Models.Task task)
        {
            if(!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            task.Created = DateTime.Now;
            task.AuthorId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (ModelState.IsValid)
            {
                _context.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssigneeId"] = new SelectList(_context.Users, "Id", "UserName", task.AssigneeId);
            ViewData["MilestoneId"] = new SelectList(_context.Milestone, "Id", "Title", task.MilestoneId);
            return View(task);
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Task.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            if (!string.Equals(task.AuthorId, User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            ViewData["AssigneeId"] = new SelectList(_context.Users, "Id", "UserName", task.AssigneeId);
            ViewData["MilestoneId"] = new SelectList(_context.Milestone, "Id", "Title", task.MilestoneId);
            return View(task);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,AssigneeId,AuthorId,MilestoneId,Closed,Created")] Models.Task task)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            if (id != task.Id)
            {
                return NotFound();
            }

            if (!string.Equals(task.AuthorId, User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            task.Updated = DateTime.Now;

            if (ModelState.IsValid)
            {
                var log = new TaskLog()
                {
                    Title = "Updated",
                    Description = "Updated a task details",
                    Created = DateTime.Now,
                    TaskId = task.Id,
                    UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value
                };
                try
                {
                    _context.Update(task);
                    _context.Add(log);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(task.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssigneeId"] = new SelectList(_context.Users, "Id", "UserName", task.AssigneeId);
            ViewData["MilestoneId"] = new SelectList(_context.Milestone, "Id", "Title", task.MilestoneId);
            return View(task);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Task
                .Include(t => t.Assignee)
                .Include(t => t.Milestone)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            if (!string.Equals(task.AuthorId, User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            var task = await _context.Task.FindAsync(id);

            if (!string.Equals(task.AuthorId, User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            _context.Task.Remove(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Tasks/Toggle/5
        [HttpGet]
        public async Task<IActionResult> Toggle(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            var task = await _context.Task.FindAsync(id);

            if (id != task.Id)
            {
                return NotFound();
            }

            if (!string.Equals(task.AuthorId, User.FindFirst(ClaimTypes.NameIdentifier).Value) && !string.Equals(task.AssigneeId, User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            task.Closed = !task.Closed;
            var log = new TaskLog()
            {
                Title = task.Closed ? "Closed" : "Opened",
                Description = $"{(task.Closed ? "Closed" : "Opened")} a task",
                Created = DateTime.Now,
                TaskId = task.Id,
                UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value
            };
            _context.Update(task);
            _context.Add(log);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = task.Id });
        }

        private bool TaskExists(int id)
        {
            return _context.Task.Any(e => e.Id == id);
        }
    }
}
