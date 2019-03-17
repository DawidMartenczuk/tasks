using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tasks.Data;
using Tasks.Models;

namespace Tasks.Controllers
{
    public class TaskLogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TaskLogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tasks
        public async Task<IActionResult> Index(int id)
        {
            var applicationDbContext = _context.Task.Include(t => t.Author).Include("Logs.User").FirstOrDefaultAsync(t => t.Id == id);
            return View(await applicationDbContext);
        }

        // GET: TaskLogs/Create/5
        public IActionResult Create(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            return View();
        }

        // POST: TaskLogs/Create/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("Title,Description")] TaskLog taskLog)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            taskLog.Created = DateTime.Now;
            taskLog.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            taskLog.TaskId = id;
            if (ModelState.IsValid)
            {
                _context.Add(taskLog);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Tasks", new { id = taskLog.TaskId });
            }
            return View(taskLog);
        }

        // GET: TaskLogs/Edit/5
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

            var taskLog = await _context.TaskLog.FindAsync(id);
            if (taskLog == null)
            {
                return NotFound();
            }
            return View(taskLog);
        }

        // POST: TaskLogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,TaskId,UserId,Created")] TaskLog taskLog)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            if (id != taskLog.Id)
            {
                return NotFound();
            }

            taskLog.Updated = DateTime.Now;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taskLog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskLogExists(taskLog.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Tasks", new { id = taskLog.TaskId });
            }
            return View(taskLog);
        }

        // GET: TaskLogs/Delete/5
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

            var taskLog = await _context.TaskLog
                .Include(t => t.Task)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskLog == null)
            {
                return NotFound();
            }

            return View(taskLog);
        }

        // POST: TaskLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            var taskLog = await _context.TaskLog.FindAsync(id);
            var taskId = taskLog.TaskId;
            _context.TaskLog.Remove(taskLog);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Tasks", new { id = taskId });
        }

        private bool TaskLogExists(int id)
        {
            return _context.TaskLog.Any(e => e.Id == id);
        }
    }
}
