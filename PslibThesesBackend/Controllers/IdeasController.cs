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
        public ActionResult<IEnumerable<IdeaListViewModel>> GetIdeas(
            string search = null, 
            string name = null, 
            string subject = null, 
            string userId = null, 
            string firstname = null, 
            string lastname = null, 
            int? target = null,
            bool? offered = null,
            string order = "name", 
            int page = 0, 
            int pagesize = 0)
        {
            IQueryable<Idea> ideas = _context.Ideas.Include(i => i.User).Include(i => i.IdeaTargets).ThenInclude(it => it.Target);
            int total = ideas.CountAsync().Result;
            if (!String.IsNullOrEmpty(search))
                ideas = ideas.Where(t => (t.Name.Contains(search)));
            if (!String.IsNullOrEmpty(name))
                ideas = ideas.Where(t => (t.Name.Contains(name)));
            if (!String.IsNullOrEmpty(subject))
                ideas = ideas.Where(t => (t.Subject.Contains(subject)));
            if (!String.IsNullOrEmpty(firstname))
                ideas = ideas.Where(t => (t.User.FirstName.Contains(firstname)));
            if (!String.IsNullOrEmpty(lastname))
                ideas = ideas.Where(t => (t.User.FirstName.Contains(lastname)));
            if (userId != null)
                ideas = ideas.Where(t => (t.UserId == userId));
            if (offered != null)
                ideas = ideas.Where(t => (t.Offered == offered));
            if (target != null)
                ideas = ideas.Where(t => (t.IdeaTargets.Contains(new IdeaTarget { TargetId = (int)target })));
            int filtered = ideas.CountAsync().Result;
            switch (order)
            {
                case "firstname":
                    ideas = ideas.OrderBy(t => t.User.FirstName);
                    break;
                case "firstname_desc":
                    ideas = ideas.OrderByDescending(t => t.User.FirstName);
                    break;
                case "lastname":
                    ideas = ideas.OrderBy(t => t.User.LastName);
                    break;
                case "lastname_desc":
                    ideas = ideas.OrderByDescending(t => t.User.LastName);
                    break;
                case "updated":
                    ideas = ideas.OrderBy(t => t.Updated);
                    break;
                case "updated_desc":
                    ideas = ideas.OrderByDescending(t => t.Updated);
                    break;
                case "name_desc":
                    ideas = ideas.OrderByDescending(t => t.Name);
                    break;
                case "name":
                default:
                    ideas = ideas.OrderBy(t => t.Name);
                    break;
            }
            if (pagesize != 0)
            {
                ideas = ideas.Skip(page * pagesize).Take(pagesize);
            }
            var count = ideas.CountAsync().Result;
            return Ok(new { total = total, filtered = filtered, count = count, page = page, pages = ((pagesize == 0) ? 0 : Math.Ceiling((double)filtered / pagesize)), data = ideas.AsNoTracking() });
        }

        // GET: Ideas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IdeaViewModel>> GetIdea(int id)
        {
            var idea = await _context.Ideas.FindAsync(id);

            if (idea == null)
            {
                return NotFound();
            }

            return new IdeaViewModel {
                Name = idea.Name,
                Description = idea.Description,
                Subject = idea.Subject,
                Resources = idea.Resources,
                UserId = idea.UserId,
                Participants = idea.Participants,
                Updated = idea.Updated,
                Created = idea.Created,
                Offered = idea.Offered
            };
        }

        // PUT: Ideas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIdea(int id, IdeaInputModel input)
        {
            if (id != input.Id)
            {
                return BadRequest();
            }

            var idea = await _context.Ideas.FindAsync(id);
            _context.Entry(idea).State = EntityState.Modified;
            if (idea == null)
            {
                return NotFound();
            }

            var user = _context.Users.FindAsync(input.UserId).Result;
            if (user == null)
            {
                return NotFound("User with Id equal to userId was not found");
            }

            idea.Name = input.Name;
            idea.Description = input.Description;
            idea.Resources = input.Resources;
            idea.Subject = input.Subject;
            idea.Participants = input.Participants;
            idea.User = user;
            idea.Updated = DateTime.Now;
            idea.Offered = input.Offered;

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

        // PUT: Ideas/5/offered
        [HttpPut("{id}/offered")]
        public async Task<IActionResult> PutIdeaOffered(int id, bool offered)
        {
            var idea = await _context.Ideas.FindAsync(id);
            _context.Entry(idea).State = EntityState.Modified;
            if (idea == null)
            {
                return NotFound();
            }

            idea.Updated = DateTime.Now;
            idea.Offered = offered;

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
                Updated = DateTime.Now,
                Offered = ideaIM.Offered
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

            return Ok(idea);
        }

        private bool IdeaExists(int id)
        {
            return _context.Ideas.Any(e => e.Id == id);
        }

        // --- targets
        // GET: Ideas/5/targets
        [HttpGet("{id}/targets")]
        public async Task<ActionResult<IEnumerable<Target>>> GetIdeaTargets(int id)
        {
            var idea = await _context.Ideas.FindAsync(id);
            if (idea == null)
            {
                return NotFound("idea not found");
            }

            var ideaTargets = _context.IdeaTargets
                .Where(it => it.Idea == idea)
                .OrderBy(it => it.Target.Text)
                .Select(it => 
                new Target { Text = it.Target.Text, Color = it.Target.Color, Id = it.Target.Id }
            ).ToList();
            return Ok(ideaTargets);
        }

        // POST: Ideas/5/targets
        [HttpPost("{id}/targets")]
        public async Task<ActionResult<IdeaTarget>> PostIdeaTarget(int id, [FromBody] int targetId)
        {
            var idea = await _context.Ideas.FindAsync(id);
            if (idea == null)
            {
                return NotFound("idea not found");
            }
            var target = await _context.Targets.FindAsync(targetId);
            if (target == null)
            {
                return NotFound("target not found");
            }
            var ideaTarget = _context.IdeaTargets.Where(it => it.Idea == idea && it.Target == target).FirstOrDefault();
            if (ideaTarget == null)
            {
                var newIdeaTarget = new IdeaTarget
                {
                    Idea = idea,
                    Target = target
                };
                _context.IdeaTargets.Add(newIdeaTarget);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetIdeaTargets", new { id = idea.Id });
            }
            else
            {
                return NoContent();
            }
        }

        // DELETE: Ideas/5/targets/3
        [HttpGet("{id}/targets/{targetId}")]
        public async Task<ActionResult<IdeaTarget>> DeleteIdeaTarget(int id, int targetId)
        {
            var idea = await _context.Ideas.FindAsync(id);
            if (idea == null)
            {
                return NotFound("idea not found");
            }
            var target = await _context.Targets.FindAsync(targetId);
            if (target == null)
            {
                return NotFound("target not found");
            }
            var ideaTarget = _context.IdeaTargets.Where(it => it.Idea == idea && it.Target == target).FirstOrDefault();
            if (ideaTarget == null)
            {
                _context.IdeaTargets.Remove(ideaTarget);
                return Ok(ideaTarget);
            }
            else
            {
                return NotFound("idea does not contain this specific target");
            }
        }

        // --- goals
        // GET: Ideas/5/goals
        [HttpGet("{id}/goals")]
        public async Task<ActionResult<IEnumerable<IdeaGoal>>> GetIdeaGoals(int id)
        {
            var idea = await _context.Ideas.FindAsync(id);

            if (idea == null)
            {
                return NotFound("idea not found");
            }

            var goals = _context.IdeaGoals.Where(ig => ig.Idea == idea).OrderBy(ig => ig.Order).ToList();
            return Ok(goals);
        }

        // POST: Ideas/5/goals
        [HttpGet("{id}/goals")]
        public async Task<ActionResult<IdeaGoal>> PostIdeaGoals(int id, string newGoalText)
        {
            var idea = await _context.Ideas.FindAsync(id);
            if (idea == null)
            {
                return NotFound("idea not found");
            }
            var maxGoalOrder = _context.IdeaGoals.Where(ig => ig.Idea == idea).Max(i => i.Order);
            _context.IdeaGoals.Add(new IdeaGoal { IdeaId = id, Order = maxGoalOrder + 1, Text = newGoalText});
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetIdeaGoal", new { id = idea.Id });
        }

        // PUT: Ideas/5/goals/1

        // PUT: Ideas/5/goals/1/moveto/2

        // DELETE: Ideas/5/goals/1

        // --- contents
        // GET: Ideas/5/contents

        // POST: Ideas/5/contents

        // PUT: Ideas/5/contents/1

        // PUT: Ideas/5/contents/1/moveto/2

        // DELETE: Ideas/5/contents/1
    }
}
