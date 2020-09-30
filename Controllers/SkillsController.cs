using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Data;
using EmployeeManagement.Models;
using ClosedXML.Excel;

namespace EmployeeManagement.Controllers
{
    public class SkillsController : Controller
    {
        private readonly SkillContext _context;

        public SkillsController(SkillContext context)
        {
            _context = context;
        }

        //Method that exports the Skill Data Set to .xlsx file
        //When the scheduler clicks the button "Export to .xlsx file"
        //the following method will run and a Skills.xlsx file will be downloaded from the scheduler's browser
        //the file is saved by default in the "Downloads" folder
        public async Task<IActionResult> Excel()
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Skills");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Id";
                worksheet.Cell(currentRow, 2).Value = "Name";
                worksheet.Cell(currentRow, 3).Value = "Description";
                worksheet.Cell(currentRow, 4).Value = "Creation Date";
                foreach (var skill in _context.Skill)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = skill.ID;
                    worksheet.Cell(currentRow, 2).Value = skill.Name;
                    worksheet.Cell(currentRow, 3).Value = skill.Description;
                    worksheet.Cell(currentRow, 4).Value = skill.CreationDate;
                }

                using (var stream = new System.IO.MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();


                    HistoryLog newlog = new HistoryLog();

                    newlog.EventDescription = "Skills DataSet exported to Skills.xlsx file.";
                    newlog.EventDateTime = DateTime.Now;

                    _context.Add(newlog);
                    await _context.SaveChangesAsync();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Skills.xlsx");
                }
            }



        }

        // GET: Skills
        public async Task<IActionResult> Index()
        {
            return View(await _context.Skill.ToListAsync());
        }

        // GET: Skills/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skill = await _context.Skill
                .FirstOrDefaultAsync(m => m.ID == id);
            if (skill == null)
            {
                return NotFound();
            }

            return View(skill);
        }

        // GET: Skills/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Skills/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Description,CreationDate")] Skill skill)
        {
            if (ModelState.IsValid)
            {
                _context.Add(skill);
                await _context.SaveChangesAsync();


                //Create a record in the HistoryLog table about the creation of the current skill
                HistoryLog newlog = new HistoryLog();

                newlog.EventDescription = "Skill " + skill.Name + " created.";
                newlog.EventDateTime = DateTime.Now;

                _context.Add(newlog);
                await _context.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }
            return View(skill);
        }

        // GET: Skills/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skill = await _context.Skill.FindAsync(id);
            if (skill == null)
            {
                return NotFound();
            }
            return View(skill);
        }

        // POST: Skills/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description,CreationDate")] Skill skill)
        {
            if (id != skill.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(skill);
                    await _context.SaveChangesAsync();


                    //Create a record in the HistoryLog table about the data change of the current skill
                    HistoryLog newlog = new HistoryLog();

                    newlog.EventDescription = "Skill " + skill.Name + " data changed.";
                    newlog.EventDateTime = DateTime.Now;

                    _context.Add(newlog);
                    await _context.SaveChangesAsync();



                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SkillExists(skill.ID))
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
            return View(skill);
        }

        // GET: Skills/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skill = await _context.Skill
                .FirstOrDefaultAsync(m => m.ID == id);
            if (skill == null)
            {
                return NotFound();
            }

            return View(skill);
        }

        // POST: Skills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var skill = await _context.Skill.FindAsync(id);
            _context.Skill.Remove(skill);
            await _context.SaveChangesAsync();


            //Create a record in the HistoryLog table about the delete of the current skill
            HistoryLog newlog = new HistoryLog();

            newlog.EventDescription = "Skill " + skill.Name + " deleted.";
            newlog.EventDateTime = DateTime.Now;

            _context.Add(newlog);
            await _context.SaveChangesAsync();



            return RedirectToAction(nameof(Index));
        }

        private bool SkillExists(int id)
        {
            return _context.Skill.Any(e => e.ID == id);
        }
    }
}
