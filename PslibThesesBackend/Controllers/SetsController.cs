using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PslibThesesBackend.Models;

namespace PslibThesesBackend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SetsController : ControllerBase
    {
        private readonly ThesesContext _context;

        public SetsController(ThesesContext context)
        {
            _context = context;
        }

        // GET: Sets
        [HttpGet]
        public ActionResult<IEnumerable<Set>> GetSets(
            string search = null,
            string name = null,
            bool? active = null,
            int? year = null,
            string order = "year_desc",           
            int page = 0,
            int pagesize = 0)
        {
            IQueryable<SetListViewModel> sets = _context.Sets.Select(s =>
                new SetListViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Active = s.Active,
                    Template = s.Template,
                    Year = s.Year,
                    MaxGrade = s.MaxGrade
                }
            );
            int total = sets.CountAsync().Result;
            if (!String.IsNullOrEmpty(search))
                sets = sets.Where(t => (t.Name.Contains(search)));
            if (!String.IsNullOrEmpty(name))
                sets = sets.Where(t => (t.Name.Contains(name)));
            if (active != null)
                sets = sets.Where(t => (t.Active == active));
            if (year != null)
                sets = sets.Where(t => (t.Year == year));
            int filtered = sets.CountAsync().Result;

            switch (order)
            {
                case "name":
                    sets = sets.OrderBy(t => t.Name);
                    break;
                case "name_desc":
                    sets = sets.OrderByDescending(s => s.Name);
                    break;
                case "id":
                    sets = sets.OrderBy(s => s.Id);
                    break;
                case "year_desc":
                    sets = sets.OrderByDescending(s => s.Year);
                    break;
                case "year":
                default:
                    sets = sets.OrderBy(s => s.Year);
                    break;
            }

            if (pagesize != 0)
            {
                sets = sets.Skip(page * pagesize).Take(pagesize);
            }
            var count = sets.CountAsync().Result;

            return Ok(new { total = total, filtered = filtered, count = count, page = page, pages = ((pagesize == 0) ? 0 : Math.Ceiling((double)filtered / pagesize)), data = sets.AsNoTracking() });
        }

        // GET: Sets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Set>> GetSet(int id)
        {
            var @set = await _context.Sets.FindAsync(id);

            if (@set == null)
            {
                return NotFound();
            }

            return @set;
        }

        // PUT: Sets/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSet(int id, Set @set)
        {
            if (id != @set.Id)
            {
                return BadRequest();
            }

            _context.Entry(@set).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SetExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: Sets
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Set>> PostSet(Set @set)
        {
            _context.Sets.Add(@set);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSet", new { id = @set.Id }, @set);
        }

        // DELETE: Sets/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Set>> DeleteSet(int id)
        {
            var @set = await _context.Sets.FindAsync(id);
            if (@set == null)
            {
                return NotFound();
            }

            _context.Sets.Remove(@set);
            await _context.SaveChangesAsync();

            return @set;
        }

        private bool SetExists(int id)
        {
            return _context.Sets.Any(e => e.Id == id);
        }
    }

    class SetListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public bool Active { get; set; }
        public ApplicationTemplate Template { get; set; }
        public int MaxGrade { get; set; }
    }
}
