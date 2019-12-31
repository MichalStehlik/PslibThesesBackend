using System;
using System.Collections.Generic;
using System.Drawing;
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
        /// <summary>
        /// Gets list of ideas satisfying given parameters.
        /// </summary>
        /// <param name="search">If Name of record contains this text, this record will be returned.</param>
        /// <param name="name">If Name of record contains this text, this record will be returned.</param>
        /// <param name="subject">If Subject of record contains this text, this record will be returned.</param>
        /// <param name="userId">If Id of author of this record is same as this value, this record will be returned.</param>
        /// <param name="firstname">If firstname of author of this record contains this text, this record will be returned.</param>
        /// <param name="lastname">If lastname of author of this record contains this text, this record will be returned.</param>
        /// <param name="target">Filters ideas for specific target group based on Id of Target.</param>
        /// <param name="offered">Filters ideas which are offered (or not) to students.</param>
        /// <param name="order">Sorting order of result - valid values are: name, name_desc, firstname, firstname_desc, lastname, lastname_desc, id, id_desc, updated, updated_desc,</param>
        /// <param name="page">Index of currently returned page of result. Starts with 0, which is default value.</param>
        /// <param name="pagesize">Size of returned page. Default is 0. In this case, no paging is performed.</param>
        /// <returns></returns>
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
            IQueryable<Idea> ideas = _context.Ideas
                .Include(i => i.User)
                .Include(i => i.IdeaTargets)
                .ThenInclude(it => it.Target);
            int total = ideas.CountAsync().Result;
            if (!String.IsNullOrEmpty(search))
                ideas = ideas.Where(i => (i.Name.Contains(search)));
            if (!String.IsNullOrEmpty(name))
                ideas = ideas.Where(i => (i.Name.Contains(name)));
            if (!String.IsNullOrEmpty(subject))
                ideas = ideas.Where(i => (i.Subject.Contains(subject)));
            if (!String.IsNullOrEmpty(firstname))
                ideas = ideas.Where(i => (i.User.FirstName.Contains(firstname)));
            if (!String.IsNullOrEmpty(lastname))
                ideas = ideas.Where(i => (i.User.LastName.Contains(lastname)));
            if (userId != null)
                ideas = ideas.Where(i => (i.UserId == userId));
            if (offered != null)
                ideas = ideas.Where(i => (i.Offered == offered));
            if (target != null)
                ideas = ideas.Where(i => (i.IdeaTargets.Where(it => it.TargetId == target).Count() > 0)); //evil
            int filtered = ideas.CountAsync().Result;
            switch (order)
            {
                case "id":
                    ideas = ideas.OrderBy(t => t.Id);
                    break;
                case "id_desc":
                    ideas = ideas.OrderByDescending(t => t.Id);
                    break;
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
            List<IdeaListViewModel> ideasVM = ideas.Select(i => new IdeaListViewModel
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description,
                Participants = i.Participants,
                Subject = i.Subject,
                Resources = i.Resources,
                UserFirstName = i.User.FirstName,
                UserLastName = i.User.LastName,
                UserId = i.UserId,
                UserEmail = i.User.Email,
                Offered = i.Offered,
                Updated = i.Updated,
                Targets = i.IdeaTargets.Select(it => it.Target)
            }).ToList();
            return Ok(new { total = total, filtered = filtered, count = count, page = page, pages = ((pagesize == 0) ? 0 : Math.Ceiling((double)filtered / pagesize)), data = ideasVM });
        }

        // GET: Ideas/5
        /// <summary>
        /// Gets data of one idea specified by his Id, returns pure data without targets, goals, contents or user data.
        /// </summary>
        /// <param name="id">Idea Id</param>
        /// <returns>Idea</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<IdeaViewModel>> GetIdea(int id)
        {
            var idea = await _context.Ideas.FindAsync(id);

            if (idea == null)
            {
                return NotFound();
            }

            return Ok(new IdeaViewModel
            {
                Name = idea.Name,
                Description = idea.Description,
                Subject = idea.Subject,
                Resources = idea.Resources,
                UserId = idea.UserId,
                Participants = idea.Participants,
                Updated = idea.Updated,
                Created = idea.Created,
                Offered = idea.Offered
            }
            );
        }

        // PUT: Ideas/5
        /// <summary>
        /// Overwrites basic data of an idea specified by his Id
        /// </summary>
        /// <param name="id">Idea Id</param>
        /// <param name="input">Idea data in body of request</param>
        /// <returns>HTTP 204, 404, 400</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIdea(int id, [FromBody] IdeaInputModel input)
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
                return NotFound("User with Id equal to userId was not found"); // TODO Only owner can edit Idea
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
        /// <summary>
        /// Sets this idea to be offered to students or not
        /// </summary>
        /// <param name="id">Idea Id</param>
        /// <param name="offered">New value (true/false) in body of request</param>
        /// <returns>HTTP 201, 404</returns>
        [HttpPut("{id}/offered")]
        public async Task<IActionResult> PutIdeaOffered(int id, [FromBody] bool offered)
        {
            var idea = await _context.Ideas.FindAsync(id);
            _context.Entry(idea).State = EntityState.Modified;
            if (idea == null)
            {
                return NotFound();
            }

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
        /// <summary>
        /// Creates and stores a new idea
        /// </summary>
        /// <param name="ideaIM">Data of an idea</param>
        /// <returns>HTTP 201</returns>
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
        /// <summary>
        /// Removes idea from database
        /// </summary>
        /// <param name="id">Id of idea</param>
        /// <returns>Removed idea, HTTP 404</returns>
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
        /// <summary>
        /// Fetch all target groups for this idea
        /// </summary>
        /// <param name="id">Idea Id</param>
        /// <returns>Array of idea targets</returns>
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
        /// <summary>
        /// Adds a new Target group to this idea
        /// </summary>
        /// <param name="id">Idea Id</param>
        /// <param name="targetId">New Target Id from requests body. If idea already contains this target, nothing happens.</param>
        /// <returns>HTTP 201, 202, 404</returns>
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
        /// <summary>
        /// Removes target group from an idea
        /// </summary>
        /// <param name="id">Idea Id</param>
        /// <param name="targetId">Target Id</param>
        /// <returns>Removed combination of idea and target keys, HTTP 404</returns>
        [HttpDelete("{id}/targets/{targetId}")]
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
            var ideaTarget = _context.IdeaTargets.Where(it => (it.Idea == idea && it.Target == target)).FirstOrDefault();
            if (ideaTarget != null)
            {
                _context.IdeaTargets.Remove(ideaTarget);
                _context.SaveChanges();
                return Ok(ideaTarget);
            }
            else
            {
                return NotFound("idea does not contain this specific target");
            }
        }

        // --- goals
        // GET: Ideas/5/goals
        /// <summary>
        /// Fetch list of all goals for this idea
        /// </summary>
        /// <param name="id">Idea id</param>
        /// <returns>Array of goals ordered by value in Order field, HTTP 404</returns>
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

        // GET: Ideas/5/goals/1
        [HttpGet("{id}/goals/{order}")]
        public async Task<ActionResult<IdeaGoal>> GetIdeaGoalsOfOrder(int id, int order)
        {
            var idea = await _context.Ideas.FindAsync(id);
            if (idea == null)
            {
                return NotFound("idea not found");
            }

            var goal = _context.IdeaGoals.Where(ig => ig.Idea == idea && ig.Order == order).FirstOrDefault();
            if (goal == null)
            {
                return NotFound("goal not found");
            }
            return Ok(goal);
        }

        // POST: Ideas/5/goals
        [HttpPost("{id}/goals")]
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
        [HttpPut("{id}/goals/{order}")]
        public async Task<ActionResult<IdeaGoal>> PutIdeaGoalsOfOrder(int id, int order, string text)
        {
            var idea = await _context.Ideas.FindAsync(id);
            if (idea == null)
            {
                return NotFound("idea not found");
            }
            var goal = _context.IdeaGoals.Where(ig => ig.Idea == idea && ig.Order == order).FirstOrDefault();
            if (goal == null)
            {
                return NotFound("goal not found");
            }

            idea.Updated = DateTime.Now;
            goal.Text = text;
         
            await _context.SaveChangesAsync();           
            return NoContent();
        }

        // PUT: Ideas/5/goals/1/moveto/2
        [HttpPut("{id}/goals/{order}/moveto/{newOrder}")]
        public async Task<ActionResult<IdeaGoal>> PutIdeaGoalsMove(int id, int order, int newOrder)
        {
            var idea = await _context.Ideas.FindAsync(id);
            if (idea == null)
            {
                return NotFound("idea not found");
            }
            var goal = _context.IdeaGoals.Where(ig => ig.Idea == idea && ig.Order == order).FirstOrDefault();
            if (goal == null)
            {
                return NotFound("goal not found");
            }
            var maxGoalOrder = _context.IdeaGoals.Where(ig => ig.Idea == idea).Max(i => i.Order);

            if (goal.Order > newOrder)
            {
                for (int i = newOrder; i < goal.Order; i++)
                {
                    var item = _context.IdeaGoals.Where(ig => ig.Idea == idea && ig.Order == i).FirstOrDefault();
                    if (item != null) item.Order = item.Order + 1;
                }
                goal.Order = newOrder;
            }
            else if (goal.Order < newOrder)
            {

            }
            _context.SaveChanges();
            return NoContent();
        }

        // DELETE: Ideas/5/goals/1
        [HttpDelete("{id}/goals/{order}")]
        public async Task<ActionResult<IdeaGoal>> DeleteIdeaGoal(int id, int order)
        {
            var idea = await _context.Ideas.FindAsync(id);
            if (idea == null)
            {
                return NotFound("idea not found");
            }
            var goal = _context.IdeaGoals.Where(ig => ig.Idea == idea && ig.Order == order).FirstOrDefault();
            if (goal == null)
            {
                return NotFound("goal not found");
            }
            else
            {
                _context.IdeaGoals.Remove(goal);
                _context.SaveChanges();
                return Ok(goal);
            }
        }

        // --- contents
        // GET: Ideas/5/contents

        // GET: Ideas/5/contents/1

        // POST: Ideas/5/contents

        // PUT: Ideas/5/contents/1

        // PUT: Ideas/5/contents/1/moveto/2

        // DELETE: Ideas/5/contents/1
    }
}
