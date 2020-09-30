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
    public class EmployeesController : Controller
    {
        private readonly EmployeeContext _context;

        public EmployeesController(EmployeeContext context)
        {
            _context = context;
        }


        // GET: Employees
        /*public async Task<IActionResult> Index()
        {
            return View(await _context.Employee.ToListAsync());
        }*/

        //Index Methdod to display the Employee List
        public async Task<IActionResult> Index(string searchString, string sortOrder,string searchSkill)
        {
            ViewData["SurnameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "surname_asc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            var employees = from e in _context.Employee
                         select e;

            if (!String.IsNullOrEmpty(searchString))
            {
                //LINQ query for searching in Name or Surname fields
                employees = employees.Where(s => s.Name.Contains(searchString) || s.Surname.Contains(searchString));
            }
            else if (!String.IsNullOrEmpty(sortOrder))
            {
                //LINQ query for sorting by Surname Ascending, if the Column name is clicked
                if (sortOrder.Equals("surname_asc"))
                {
                    employees = employees.OrderBy(s => s.Surname);
                }
                //LINQ query for sorting by Hring Date Ascending, if the Column name is clicked
                else if (sortOrder.Equals("Date"))
                {
                    employees = employees.OrderBy(s => s.HiringDate);
                }
                
            }
            else if (!String.IsNullOrEmpty(searchSkill))
            {
                //LINQ query for searching in Skillset field
                employees = employees.Where(s => s.Skillset.Contains(searchSkill));
            }

                //In any way a View will be returned to Index Page filtered, sorted or unedited from the DataSet
                return View(await employees.ToListAsync());
        }


        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {


            //Get all the skills from the Skill DBSet
            var allskills = from e in _context.Skill
                            select e;

            String skillnames = "";

            //Saving all the skills to a string named skill names
            foreach (var skill in allskills)
            {
                skillnames += skill.Name + ",";

            }

            //Removing the last ","
            skillnames = skillnames.Remove(skillnames.Length - 1);

            //Via the ViewBag property, the string skillnames gets passed in the Create.cshtml file
            //and its displayed below the SkillSet field as a helping text
            ViewBag.SkillNames = skillnames;

            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Surname,Country,City,Address,HiringDate,Department,Skillset,JobTitle,MonthlySalary")] Employee employee)
        {

            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();



                //Create a record in the HistoryLog table about the creation of the current employee
                HistoryLog newlog = new HistoryLog();

                newlog.EventDescription = "Employee " + employee.Name + " " + employee.Surname + " created.";
                newlog.EventDateTime = DateTime.Now;

                _context.Add(newlog);
                await _context.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }


            //Get all the Skills from DBSet to a List named allskillslist
            var allskills = from e in _context.Skill
                            select e;


            List<string> allskillslist = new List<string>();

            foreach (var skill in allskills)
            {
                allskillslist.Add(skill.Name);
            }



            //Get the skillset of the current employee that is being edited
            //The id is stored as a parameter in the method, so the where clause
            //in the following linq query filters only the current employee that is being edited
            var currentemployee = from e in _context.Employee
                                 where e.Id == id 
                            select e;

            String employeeskills = "";

            foreach (var currentskill in currentemployee)
            {
                employeeskills += currentskill.Skillset;

            }

            //Spliting every skill with ',' delimeter and saving it to a List named employeeskillslist
            List<string> employeeskillslist = new List<string>();

            employeeskillslist = employeeskills.Split(',').ToList();


            //Declaration of a 3rd helping List for the final result named finalList
            List<string> finalList = new List<string>();

            //The final list purpose is to contain only the skills that the current employee does not have

            //This whole process is executed in order to inform the scheduler
            //about witch skills the current employee does not have

            //So the finalList contains the Excpetion of the two previous lists
            finalList = allskillslist.Except(employeeskillslist).ToList();

            //Convertion of the finalList to a string
            string finalstring = string.Join(",", finalList);

            //Checking if the current employee has all the available skills
            //If he has, it means that there are no other skills to add to his record
            if (!finalList.Any())
            {
                finalstring = "The current employee has already all the available skills!";
            }

            //Via the ViewBag property, the finalstring gets passed in the Edit.cshtml file
            //and its displayed below the SkillSet field as a helping text
            ViewBag.EmployeeAvailableSkills = finalstring;


            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,Country,City,Address,HiringDate,Department,Skillset,JobTitle,MonthlySalary")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();


                    //Create a record in the HistoryLog table about the data change of the current employee
                    HistoryLog newlog = new HistoryLog();

                    newlog.EventDescription = "Employee " + employee.Name + " " + employee.Surname + " data changed.";
                    newlog.EventDateTime = DateTime.Now;

                    _context.Add(newlog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
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
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employee.FindAsync(id);
            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();


            //Create a record in the HistoryLog table about the delete of the current employee
            HistoryLog newlog = new HistoryLog();

            newlog.EventDescription = "Employee " + employee.Name + " " + employee.Surname + " deleted.";
            newlog.EventDateTime = DateTime.Now;

            _context.Add(newlog);
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.Id == id);
        }
    }
}
