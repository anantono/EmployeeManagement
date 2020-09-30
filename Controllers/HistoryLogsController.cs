using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Data;
using EmployeeManagement.Models;

namespace EmployeeManagement.Controllers
{
    public class HistoryLogsController : Controller
    {
        private readonly HistoryLogContext _context;

        public HistoryLogsController(HistoryLogContext context)
        {
            _context = context;
        }

        // GET: HistoryLogs
        public async Task<IActionResult> Index()
        {

            //Displaying the HistoryLog ordering with descending DateTine
            //So the newest event will be shown first
            var logs = from e in _context.HistoryLog
                       orderby e.EventDateTime descending
                       select e;

            return View(await logs.ToListAsync());

            //return View(await _context.HistoryLog.ToListAsync());
        }

        // GET: HistoryLogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historyLog = await _context.HistoryLog
                .FirstOrDefaultAsync(m => m.Id == id);
            if (historyLog == null)
            {
                return NotFound();
            }

            return View(historyLog);
        }

        // GET: HistoryLogs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HistoryLogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EventDescription,EventDateTime")] HistoryLog historyLog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(historyLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(historyLog);
        }

        // GET: HistoryLogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historyLog = await _context.HistoryLog.FindAsync(id);
            if (historyLog == null)
            {
                return NotFound();
            }
            return View(historyLog);
        }

        // POST: HistoryLogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EventDescription,EventDateTime")] HistoryLog historyLog)
        {
            if (id != historyLog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(historyLog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HistoryLogExists(historyLog.Id))
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
            return View(historyLog);
        }

        // GET: HistoryLogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historyLog = await _context.HistoryLog
                .FirstOrDefaultAsync(m => m.Id == id);
            if (historyLog == null)
            {
                return NotFound();
            }

            return View(historyLog);
        }

        // POST: HistoryLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var historyLog = await _context.HistoryLog.FindAsync(id);
            _context.HistoryLog.Remove(historyLog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HistoryLogExists(int id)
        {
            return _context.HistoryLog.Any(e => e.Id == id);
        }
    }
}
