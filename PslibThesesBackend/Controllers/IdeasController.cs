using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PslibThesesBackend.Models;
using PslibThesesBackend.ViewModels;

namespace PslibThesesBackend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class IdeasController : ControllerBase
    {
        private readonly ThesesContext _context;

        public IdeasController(ThesesContext context)
        {
            _context = context;
        }

        // GET: Ideas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IdeaViewModel>>> GetIdeas(
            string search = null, 
            string name = null, 
            string subject = null, 
            string user = null, 
            string firstname = null, 
            string lastname = null, 
            int? target = null, 
            string order = "name", 
            int page = 0, 
            int pagesize = 0)
        {
            IQueryable<IdeaViewModel> ideas = _context.Ideas.Select(i =>
                new IdeaViewModel
                {
                    Id = i.Id,
                    Name = i.Name,
                    Description = i.Description,
                    Subject = i.Subject,
                    Resources = i.Resources,
                    UserId = i.UserId,
                    Participants = i.Participants,
                    UserFirstName = i.User.FirstName,
                    UserLastName = i.User.LastName,
                    UserMiddleName = i.User.MiddleName,
                    UserEmail = i.User.Email
                }
            );
            return Ok(ideas.AsNoTracking());
        }

        // GET: Ideas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Idea>> GetIdea(int id)
        {
            var idea = await _context.Ideas.FindAsync(id);

            if (idea == null)
            {
                return NotFound();
            }

            return idea;
        }

        // PUT: Ideas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIdea(int id, Idea idea)
        {
            if (id != idea.Id)
            {
                return BadRequest();
            }

            _context.Entry(idea).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IdeaExists(id))
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

        // POST: Ideas
        [HttpPost]
        public async Task<ActionResult<Idea>> PostIdea(IdeaInputModel ideaIM)
        {
            var user = _context.Users.FindAsync(ideaIM.UserId).Result;
            if (user == null)
            {
                return NotFound("User with Id equal to userId was not found");
            }
            var idea = new Idea {
                Name = ideaIM.Name,
                Description = ideaIM.Description,
                Resources = ideaIM.Resources,
                Subject = ideaIM.Subject,
                Participants = ideaIM.Participants,
                User = user,
                Created = DateTime.Now,
                Updated = DateTime.Now
            };
            _context.Ideas.Add(idea);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIdea", new { id = idea.Id }, idea);
        }

        // DELETE: Ideas/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Idea>> DeleteIdea(int id)
        {
            var idea = await _context.Ideas.FindAsync(id);
            if (idea == null)
            {
                return NotFound();
            }

            _context.Ideas.Remove(idea);
            await _context.SaveChangesAsync();

            return idea;
        }

        private bool IdeaExists(int id)
        {
            return _context.Ideas.Any(e => e.Id == id);
        }
    }
}
