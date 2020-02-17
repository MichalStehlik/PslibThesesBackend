using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public class WorksController : ControllerBase
    {
        private readonly ThesesContext _context;

        private readonly Dictionary<WorkState, List<WorkState>> _stateTransitions = new Dictionary<WorkState, List<WorkState>>
        {
            { WorkState.InPreparation, new List<WorkState> {WorkState.WorkedOut} },
            { WorkState.WorkedOut, new List<WorkState> {WorkState.Completed, WorkState.Failed} },
            { WorkState.Completed, new List<WorkState> {WorkState.Undefended, WorkState.Succesful} },
            { WorkState.Failed, new List<WorkState> {} },
            { WorkState.Succesful, new List<WorkState> {} },
            { WorkState.Undefended, new List<WorkState> {} }
        };
        public WorksController(ThesesContext context)
        {
            _context = context;
        }
        // GET: Work
        [HttpGet]
        public ActionResult<IEnumerable<WorkListViewModel>> GetWorks(
            string search = null,
            string name = null,
            string subject = null,
            Guid? authorId = null,
            Guid? userId = null,
            string firstname = null,
            string lastname = null,
            int? setId = null,
            string setName = null,
            WorkState? state = null,
            string order = "name",
            int page = 0,
            int pagesize = 0)
        {
            IQueryable<Work> works = _context.Works
                .Include(i => i.User);
            int total = works.CountAsync().Result;
            if (!String.IsNullOrEmpty(search))
                works = works.Where(i => (i.Name.Contains(search)));
            if (!String.IsNullOrEmpty(name))
                works = works.Where(i => (i.Name.Contains(name)));
            if (!String.IsNullOrEmpty(subject))
                works = works.Where(i => (i.Subject.Contains(subject)));
            if (!String.IsNullOrEmpty(firstname))
                works = works.Where(i => (i.Author.FirstName.Contains(firstname)));
            if (!String.IsNullOrEmpty(lastname))
                works = works.Where(i => (i.Author.LastName.Contains(lastname)));
            if (userId != null)
                works = works.Where(i => (i.UserId == userId));
            if (authorId != null)
                works = works.Where(i => (i.AuthorId == authorId));
            if (setId != null)
                works = works.Where(i => (i.SetId == setId));
            if (!String.IsNullOrEmpty(setName))
                works = works.Where(i => (i.Set.Name.Contains(setName)));
            if (state != null)
                works = works.Where(i => (i.State == state));
            int filtered = works.CountAsync().Result;
            switch (order)
            {
                case "id":
                    works = works.OrderBy(t => t.Id);
                    break;
                case "id_desc":
                    works = works.OrderByDescending(t => t.Id);
                    break;
                case "firstname":
                    works = works.OrderBy(t => t.User.FirstName);
                    break;
                case "firstname_desc":
                    works = works.OrderByDescending(t => t.User.FirstName);
                    break;
                case "lastname":
                    works = works.OrderBy(t => t.User.LastName);
                    break;
                case "lastname_desc":
                    works = works.OrderByDescending(t => t.User.LastName);
                    break;
                case "state":
                    works = works.OrderBy(t => t.State);
                    break;
                case "state_desc":
                    works = works.OrderByDescending(t => t.State);
                    break;
                case "updated":
                    works = works.OrderBy(t => t.Updated);
                    break;
                case "updated_desc":
                    works = works.OrderByDescending(t => t.Updated);
                    break;
                case "name_desc":
                    works = works.OrderByDescending(t => t.Name);
                    break;
                case "name":
                default:
                    works = works.OrderBy(t => t.Name);
                    break;
            }
            if (pagesize != 0)
            {
                works = works.Skip(page * pagesize).Take(pagesize);
            }
            List<WorkListViewModel> worksVM = works.Select(i => new WorkListViewModel
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description,
                Subject = i.Subject,
                Resources = i.Resources,
                AuthorFirstName = i.Author.FirstName,
                AuthorLastName = i.Author.LastName,
                AuthorId = i.AuthorId,
                AuthorEmail = i.Author.Email,
                Updated = i.Updated,
                SetId = i.SetId,
                SetName = i.Set.Name,
                State = i.State,
                ClassName = i.ClassName
            }).ToList();
            int count = worksVM.Count();
            return Ok(new { total = total, filtered = filtered, count = count, page = page, pages = ((pagesize == 0) ? 0 : Math.Ceiling((double)filtered / pagesize)), data = worksVM });
        }

        // GET: Works/5
        /// <summary>
        /// Gets data of one work specified by his Id, returns pure data without goals, contents or user data.
        /// </summary>
        /// <param name="id">Work Id</param>
        /// <returns>Work</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkViewModel>> GetWork(int id)
        {
            var work = await _context.Works.FindAsync(id);

            if (work == null)
            {
                return NotFound();
            }

            return Ok(new WorkViewModel
            {
                Name = work.Name,
                Description = work.Description,
                Subject = work.Subject,
                Resources = work.Resources,
                UserId = work.UserId,
                Updated = work.Updated,
                Created = work.Created,
                ClassName = work.ClassName,
                DetailExpenditures = work.DetailExpenditures,
                MaterialCosts = work.MaterialCosts,
                MaterialCostsProvidedBySchool = work.MaterialCostsProvidedBySchool,
                ServicesCosts = work.ServicesCosts,
                ServicesCostsProvidedBySchool = work.ServicesCostsProvidedBySchool,
                AuthorId = work.AuthorId,
                ManagerId = work.ManagerId
            }
            );
        }

        [HttpGet("{id}/full")]
        public async Task<ActionResult<WorkViewModel>> GetFullWork(int id)
        {
            var work = await _context.Works
                .Where(i => i.Id == id)
                .Select(i => new {
                    i.Id,
                    i.Name,
                    i.User,
                    i.Author,
                    i.Manager,
                    i.Description,
                    i.Resources,
                    i.Subject,
                    i.Updated,
                    i.Created,
                    i.Goals,
                    i.Outlines,
                    i.State,
                    i.Set,
                    i.ClassName
                })
                .FirstOrDefaultAsync();
            if (work == null)
            {
                return NotFound();
            }

            return Ok(work);
        }

        // POST: Works
        [HttpPost]
        public async Task<ActionResult<Idea>> Post([FromBody] WorkInputModel input)
        {
            var user = _context.Users.FindAsync(input.UserId).Result;
            if (user == null)
            {
                return NotFound("User with Id equal to userId was not found");
            }
            var author = _context.Users.FindAsync(input.AuthorId).Result;
            if (author == null)
            {
                return NotFound("User with Id equal to authorId was not found");
            }

            var manager = _context.Users.FindAsync(input.ManagerId).Result;
            if (manager == null)
            {
                return NotFound("User with Id equal to managerId was not found");
            }

            var set = _context.Sets.FindAsync(input.SetId).Result;
            if (set == null)
            {
                return NotFound("Set not found");
            }

            if (manager.CanBeEvaluator == false)
            {
                return BadRequest("User with ManagerId cannot be assigned as manager.");
            }

            if (author.CanBeAuthor == false)
            {
                return BadRequest("User with AuthorId cannot be assigned as author.");
            }
            var work = new Work
            {
                Name = input.Name,
                Description = input.Description,
                Resources = input.Resources,
                Subject = input.Subject,
                Set = set,
                User = user,
                Created = DateTime.Now,
                Updated = DateTime.Now,
                MaterialCosts = input.MaterialCosts,
                MaterialCostsProvidedBySchool = input.MaterialCostsProvidedBySchool,
                ServicesCosts = input.ServicesCosts,
                ServicesCostsProvidedBySchool = input.ServicesCostsProvidedBySchool,
                State = WorkState.InPreparation,
                DetailExpenditures = input.DetailExpenditures,
                Author = author,
                Manager = manager,
                ClassName = input.ClassName
            };
            _context.Works.Add(work);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWork", new { id = work.Id }, work);
        }

        // PUT: Works/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] WorkInputModel input)
        {
            if (id != input.Id)
            {
                return BadRequest();
            }

            var work = await _context.Works.FindAsync(id);
            _context.Entry(work).State = EntityState.Modified;
            if (work == null)
            {
                return NotFound();
            }

            var user = _context.Users.FindAsync(input.UserId).Result;
            if (user == null)
            {
                return NotFound("User with Id equal to userId was not found"); // TODO Only owner or Manager or Author in InPreparation State can edit Work
            }

            var author = _context.Users.FindAsync(input.AuthorId).Result;
            if (author == null)
            {
                return NotFound("User with Id equal to authorId was not found"); 
            }

            var manager = _context.Users.FindAsync(input.ManagerId).Result;
            if (manager == null)
            {
                return NotFound("User with Id equal to managerId was not found");
            }

            var set = _context.Sets.FindAsync(input.SetId).Result;
            if (set == null)
            {
                return NotFound("Set not found");
            }

            if (work.ManagerId != input.ManagerId && manager.CanBeEvaluator == false)
            {
                return BadRequest("User with ManagerId cannot be assigned as manager.");
            }

            if (work.AuthorId != input.AuthorId && author.CanBeAuthor == false)
            {
                return BadRequest("User with AuthorId cannot be assigned as author.");
            }

            work.Name = input.Name;
            work.Description = input.Description;
            work.Resources = input.Resources;
            work.Subject = input.Subject;
            work.Set = set;
            work.MaterialCosts = input.MaterialCosts;
            work.MaterialCostsProvidedBySchool = input.MaterialCostsProvidedBySchool;
            work.ServicesCosts = input.ServicesCosts;
            work.ServicesCostsProvidedBySchool = input.ServicesCostsProvidedBySchool;
            work.State = input.State;
            work.DetailExpenditures = input.DetailExpenditures;
            work.User = user;
            work.Author = author;
            work.Manager = manager;
            work.Updated = DateTime.Now;
            work.ClassName = input.ClassName;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkExists(id))
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

        // DELETE: Works/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Idea>> Delete(int id)
        {
            var work = await _context.Works.FindAsync(id);
            if (work == null)
            {
                return NotFound();
            }

            _context.Works.Remove(work);
            await _context.SaveChangesAsync();

            return Ok(work);
        }

        private bool WorkExists(int id)
        {
            return _context.Works.Any(e => e.Id == id);
        }

        // --- goals
        // GET: Works/5/goals
        /// <summary>
        /// Fetch list of all goals for this work
        /// </summary>
        /// <param name="id">Work id</param>
        /// <returns>Array of goals ordered by value in Order field, HTTP 404</returns>
        [HttpGet("{id}/goals")]
        public async Task<ActionResult<IEnumerable<WorkGoal>>> GetWorkGoals(int id)
        {
            var work = await _context.Works.FindAsync(id);

            if (work == null)
            {
                return NotFound("work not found");
            }

            var goals = _context.WorkGoals
                .Where(wg => wg.Work == work)
                .Select(wg => new { WorkId = wg.WorkId, Order = wg.Order, Text = wg.Text })
                .OrderBy(wg => wg.Order)
                .AsNoTracking();
            return Ok(goals);
        }

        // GET: Works/5/goals/1
        /// <summary>
        /// Fetch specific goal of an work in certain order
        /// </summary>
        /// <param name="id">Work Id</param>
        /// <param name="order">Order (1+)</param>
        /// <returns>Goal, HTTP 404</returns>
        [HttpGet("{id}/goals/{order}")]
        public async Task<ActionResult<WorkGoal>> GetWorkGoal(int id, int order)
        {
            var work = await _context.Works.FindAsync(id);
            if (work == null)
            {
                return NotFound("work not found");
            }

            var goal = _context.WorkGoals
                .Where(wg => wg.Work == work && wg.Order == order)
                .Select(wg => new { WorkId = wg.WorkId, wg.Order, wg.Text })
                .FirstOrDefault();
            if (goal == null)
            {
                return NotFound("goal not found");
            }
            return Ok(goal);
        }

        // POST: Works/5/goals
        /// <summary>
        /// Creates and stores a new goal for a work
        /// </summary>
        /// <param name="id">Work id</param>
        /// <param name="goalText">Object containing text</param>
        /// <returns>New goal, HTTP 404</returns>
        [HttpPost("{id}/goals")]
        public async Task<ActionResult<WorkGoal>> PostWorkGoals(int id, [FromBody] WorkGoalInputModel goalText)
        {
            var work = await _context.Ideas.FindAsync(id);
            if (work == null)
            {
                return NotFound("work not found");
            }
            if (String.IsNullOrEmpty(goalText.Text))
            {
                return BadRequest("text of goal cannot be empty");
            }
            int maxGoalOrder;
            try
            {
                maxGoalOrder = _context.WorkGoals.Where(wg => wg.WorkId == id).Max(wg => wg.Order);
            }
            catch
            {
                maxGoalOrder = 0;
            }
            var newGoal = new WorkGoal { WorkId = id, Order = maxGoalOrder + 1, Text = goalText.Text };
            _context.WorkGoals.Add(newGoal);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetWorkGoal", new { id = newGoal.WorkId, order = newGoal.Order }, new { WorkId = id, Order = maxGoalOrder + 1, Text = goalText.Text });
        }

        // PUT: Works/5/goals/1
        /// <summary>
        /// Changes text of goal inside a work
        /// </summary>
        /// <param name="id">Work Id</param>
        /// <param name="order">Order of goal</param>
        /// <param name="goalText">New text of goal</param>
        /// <returns>HTTP 201, 404</returns>
        [HttpPut("{id}/goals/{order}")]
        public async Task<ActionResult<WorkGoal>> PutWorkGoalsOfOrder(int id, int order, [FromBody] WorkGoalInputModel goalText)
        {
            var work = await _context.Works.FindAsync(id);
            if (work == null)
            {
                return NotFound("work not found");
            }
            if (String.IsNullOrEmpty(goalText.Text))
            {
                return BadRequest("text of goal cannot be empty");
            }
            var goal = _context.WorkGoals.Where(wg => wg.Work == work && wg.Order == order).FirstOrDefault();
            if (goal == null)
            {
                return NotFound("goal not found");
            }

            work.Updated = DateTime.Now;
            goal.Text = goalText.Text;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // PUT: Works/5/goals/1/moveto/2
        /// <summary>
        /// Moves goal to a new position and shifts other goals to reorganize them in new order
        /// </summary>
        /// <param name="id">Work Id</param>
        /// <param name="order">Original position</param>
        /// <param name="newOrder">New position</param>
        /// <returns>HTTP 201, 404</returns>
        [HttpPut("{id}/goals/{order}/moveto/{newOrder}")]
        public async Task<ActionResult<WorkGoal>> PutWorkGoalsMove(int id, int order, int newOrder)
        {
            var work = await _context.Works.FindAsync(id);
            if (work == null)
            {
                return NotFound("work not found");
            }
            var goal = _context.WorkGoals.Where(wg => wg.Work == work && wg.Order == order).FirstOrDefault();
            if (goal == null)
            {
                return NotFound("goal not found");
            }

            int maxOrder;
            try
            {
                maxOrder = _context.WorkGoals.Where(wg => wg.WorkId == id).Max(wg => wg.Order);
            }
            catch
            {
                maxOrder = 0;
            }

            if (newOrder > maxOrder) newOrder = maxOrder;

            WorkGoal temp = new WorkGoal { WorkId = id, Order = newOrder, Text = goal.Text }; // future record
            _context.WorkGoals.Remove(goal); // remove old record, we made backup of it in temp
            _context.SaveChanges();

            if (order > newOrder) // moving down
            {
                for (int i = order - 1; i >= newOrder; i--)
                {
                    var item = _context.WorkGoals.Where(wg => wg.Work == work && wg.Order == i).FirstOrDefault();
                    if (item != null) item.Order = item.Order + 1;
                    _context.SaveChanges();
                }
            }
            else if (order < newOrder) // moving up
            {
                for (int i = order + 1; i <= newOrder; i++)
                {
                    var item = _context.WorkGoals.Where(wg => wg.Work == work && wg.Order == i).FirstOrDefault();
                    if (item != null) item.Order = item.Order - 1;
                    _context.SaveChanges();
                }
            }
            _context.WorkGoals.Add(temp);
            work.Updated = DateTime.Now;
            _context.SaveChanges();
            return NoContent();
        }

        // DELETE: Works/5/goals
        /// <summary>
        /// Removes all goals in specified work
        /// </summary>
        /// <param name="id">Work id</param>
        /// <returns>HTTP 201, 404</returns>
        [HttpDelete("{id}/goals")]
        public async Task<ActionResult<IdeaGoal>> DeleteAllWorkGoals(int id)
        {
            var work = await _context.Works.FindAsync(id);
            if (work == null)
            {
                return NotFound("work not found");
            }
            var goals = _context.WorkGoals.Where(ig => ig.Work == work).AsNoTracking().ToList();
            if (goals != null)
            {
                _context.WorkGoals.RemoveRange(goals);
                work.Updated = DateTime.Now;
                _context.SaveChanges();
            }
            return NoContent();
        }

        // DELETE: Works/5/goals/1
        /// <summary>
        /// Removes selected goal and shift all others to fill a hole
        /// </summary>
        /// <param name="id">Work Id</param>
        /// <param name="order">Order of removed goal</param>
        /// <returns>Removed goal, HTTP 404</returns>
        [HttpDelete("{id}/goals/{order}")]
        public async Task<ActionResult<WorkGoal>> DeleteWorkGoal(int id, int order)
        {
            var work = await _context.Works.FindAsync(id);
            if (work == null)
            {
                return NotFound("work not found");
            }
            var goal = _context.WorkGoals.Where(ig => ig.Work == work && ig.Order == order).AsNoTracking().FirstOrDefault();
            if (goal == null)
            {
                return NotFound("goal not found");
            }

            WorkGoal removedGoal = new WorkGoal { Id = goal.Id, WorkId = id, Order = order, Text = goal.Text };
            int maxGoalOrder;
            try
            {
                maxGoalOrder = _context.WorkGoals.Where(ig => ig.WorkId == id).Max(ig => ig.Order);
            }
            catch
            {
                maxGoalOrder = 0;
            }

            _context.WorkGoals.Remove(goal);
            work.Updated = DateTime.Now;
            _context.SaveChanges();

            for (int i = order; i <= maxGoalOrder; i++)
            {
                var row = _context.WorkGoals.Where(ig => ig.WorkId == id && ig.Order == i).FirstOrDefault();
                if (row != null)
                {
                    row.Order = i - 1;
                    _context.SaveChanges();
                }
            }

            return Ok(new { removedGoal.Id, removedGoal.WorkId, removedGoal.Order, removedGoal.Text });
        }
    }

    // InputModels
    public class WorkInputModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Resources { get; set; }
        public string Subject { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public Guid AuthorId { get; set; }
        public Guid ManagerId { get; set; }
        [Required]
        public int SetId { get; set; }
        public int MaterialCosts { get; set; } = 0;
        public int MaterialCostsProvidedBySchool { get; set; } = 0;
        public int ServicesCosts { get; set; } = 0;
        public int ServicesCostsProvidedBySchool { get; set; } = 0;
        public string DetailExpenditures { get; set; }
        public WorkState State { get; set; } = WorkState.InPreparation;
        public string ClassName { get; set; }
    }

    public class WorkGoalInputModel
    {
        public string Text { get; set; }
    }

    public class WorkOutlineInputModel
    {
        public string Text { get; set; }
    }
}
