﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
                    Year = s.Year
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
            var result = sets.ToList();
            int count = result.Count();

            return Ok(new { total = total, filtered = filtered, count = count, page = page, pages = ((pagesize == 0) ? 0 : Math.Ceiling((double)filtered / pagesize)), data = result });
        }

        // GET: Sets/5
        [Authorize(Policy = "Administrator")]
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
        [HttpPut("{id}")]
        [Authorize(Policy = "Administrator")]
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
        [HttpPost]
        [Authorize(Policy = "Administrator")]
        public async Task<ActionResult<Set>> PostSet(Set @set)
        {
            _context.Sets.Add(@set);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSet", new { id = @set.Id }, @set);
        }

        // DELETE: Sets/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "Administrator")]
        public async Task<ActionResult<Set>> DeleteSet(int id)
        {
            // TODO kontrola, zda v sadě nejsou práce
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
            return _context.Sets.Any(s => (s.Id == id));
        }

        [HttpGet("{id}/terms")]
        public ActionResult<IEnumerable<SetTerm>> GetSetTerms(int id)
        {
            if (!SetExists(id))
            {
                return NotFound("set not found");
            }

            var setTerms = _context.SetTerms
                .Where(st => st.SetId == id)
                .OrderBy(st => st.Date)
                .Select(st => new { Id = st.Id, Name = st.Name, Date = st.Date, WarningDate = st.WarningDate, QuestionsCount = st.Questions.Count })
                .AsNoTracking();
            return Ok(setTerms);
        }

        [HttpGet("{id}/terms/{termId}")]
        public async Task<ActionResult<SetTerm>> GetSetTerm(int id, int termId)
        {
            var @set = await _context.Sets.FindAsync(id);
            if (@set == null)
            {
                return NotFound("set not found");
            }
            var @term = await _context.SetTerms.FindAsync(termId);
            if (@term == null)
            {
                return NotFound("term not found");
            }
            var setTerm = _context.SetTerms.Where(st => (st.SetId == id && st.Id == termId)).FirstOrDefault();
            if (@setTerm == null)
            {
                return NotFound("term is not in this set");
            }
            return @term;
        }

        [HttpPut("{id}/terms/{termId}")]
        [Authorize(Policy = "Administrator")]
        public async Task<ActionResult<SetTerm>> PutSetTerms(int id, int termId, [FromBody] SetTermIdInputModel st)
        {
            var set = await _context.Sets.FindAsync(id);
            if (set == null)
            {
                return NotFound("set not found");
            }
            var @term = await _context.SetTerms.FindAsync(termId);
            if (@term == null)
            {
                return NotFound("term not found");
            }
            if (term.SetId != set.Id)
            {
                return BadRequest("term is not in this set");
            }
            /*
            var works = _context.Works.Where(w => (w.SetId == id)).Count();
            if (works != 0)
            {
                return BadRequest("it is not possible to edit any terms after set already contains at least one work");
            } 
            */
            if (st.WarningDate == null) st.WarningDate = st.Date.AddDays(-2);
            term.Name = st.Name;
            term.Date = st.Date;
            term.WarningDate = st.WarningDate;
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetSetTerm", new { id = term.SetId, termId = term.Id }, id);
        }

        [HttpDelete("{id}/terms/{termId}")]
        [Authorize(Policy = "Administrator")]
        public async Task<ActionResult<SetTerm>> DeleteSetTerms(int id, int termId)
        {
            var set = await _context.Sets.FindAsync(id);
            if (set == null)
            {
                return NotFound("set not found");
            }
            var @term = await _context.SetTerms.FindAsync(termId);
            if (@term == null)
            {
                return NotFound("term not found");
            }
            var setTerm = _context.SetTerms.Where(st => (st.SetId == id && st.Id == termId)).FirstOrDefault();
            if (@setTerm == null)
            {
                return NotFound("term is not in this set");
            }
            _context.SetTerms.Remove(term);
            await _context.SaveChangesAsync();
            return term;
        }

        [HttpPost("{id}/terms")]
        [Authorize(Policy = "Administrator")]
        public async Task<ActionResult<SetTerm>> PostSetTerms(int id, [FromBody] SetTermIdInputModel st)
        {
            var set = await _context.Sets.FindAsync(id);
            if (set == null)
            {
                return NotFound("set not found");
            }
            if (st.WarningDate == null) st.WarningDate = st.Date.AddDays(-2);
            var newTerm = new SetTerm { SetId = id, Name = st.Name, Date = st.Date, WarningDate = st.WarningDate };
            _context.SetTerms.Add(newTerm);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetSetTerm", new { id = newTerm.SetId, termId = newTerm.Id }, newTerm);
        }

        // --- roles
        /// <summary>
        /// Gets list of roles for given set
        /// </summary>
        /// <param name="id">Set ID</param>
        /// <returns></returns>
        [HttpGet("{id}/roles")]
        public ActionResult<IEnumerable<SetTerm>> GetSetRoles(int id)
        {
            if (!SetExists(id))
            {
                return NotFound("set not found");
            }

            var setTerms = _context.SetRoles
                .Where(st => st.SetId == id)
                .OrderBy(st => st.Name)
                .Select(st => new { Id = st.Id, Name = st.Name, st.ClassTeacher, st.Manager, st.PrintedInApplication, st.PrintedInReview, QuestionsCount = st.Questions.Count })
                .AsNoTracking();
            return Ok(setTerms);
        }

        /// <summary>
        /// Fetch role data
        /// </summary>
        /// <param name="id">set id</param>
        /// <param name="roleId">id of role</param>
        /// <returns>role</returns>
        [HttpGet("{id}/roles/{roleId}")]
        public async Task<ActionResult<SetRole>> GetSetRole(int id, int roleId)
        {
            var @set = await _context.Sets.FindAsync(id);
            if (@set == null)
            {
                return NotFound("set not found");
            }
            var @role = await _context.SetRoles.FindAsync(roleId);
            if (@role == null)
            {
                return NotFound("role not found");
            }
            var setRole = _context.SetRoles.Where(st => (st.SetId == id && st.Id == roleId)).FirstOrDefault();
            if (@setRole == null)
            {
                return NotFound("role is not in this set");
            }
            return @role;
        }

        /// <summary>
        /// Updates definition of role
        /// </summary>
        /// <param name="id">id of set</param>
        /// <param name="roleId">id of role</param>
        /// <param name="sr">role data</param>
        /// <returns>modified role</returns>
        [HttpPut("{id}/roles/{roleId}")]
        [Authorize(Policy = "Administrator")]
        public async Task<ActionResult<SetTerm>> PutSetRoles(int id, int roleId, [FromBody] SetRoleIdInputModel sr)
        {
            var set = await _context.Sets.FindAsync(id);
            if (set == null)
            {
                return NotFound("set not found");
            }
            var @role = await _context.SetRoles.FindAsync(roleId);
            if (@role == null)
            {
                return NotFound("role not found");
            }
            if (role.SetId == set.Id)
            {
                return BadRequest("role is not in this set");
            }
            var works = _context.Works.Where(w => (w.SetId == id)).Count();
            if (works != 0)
            {
                return BadRequest("it is not possible to edit any roles after set already contains at least one work");
            }
            role.Name = sr.Name;
            role.ClassTeacher = sr.ClassTeacher;
            role.Manager = sr.Manager;
            role.PrintedInApplication = sr.PrintedInApplication;
            role.PrintedInReview = sr.PrintedInReview;
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetSetRole", new { id = role.SetId, roleId = role.Id }, id);
        }

        /// <summary>
        /// Delete role from given set
        /// </summary>
        /// <param name="id">Set Id</param>
        /// <param name="roleId">Role Id (must be in set)</param>
        /// <returns>Deleted set</returns>
        [HttpDelete("{id}/roles/{roleId}")]
        [Authorize(Policy = "Administrator")]
        public async Task<ActionResult<SetRole>> DeleteSetRoles(int id, int roleId)
        {
            var set = await _context.Sets.FindAsync(id);
            if (set == null)
            {
                return NotFound("set not found");
            }
            var @role = await _context.SetRoles.FindAsync(roleId);
            if (@role == null)
            {
                return NotFound("role not found");
            }
            var setRole = _context.SetRoles.Where(st => (st.SetId == id && st.Id == roleId)).FirstOrDefault();
            if (@setRole == null)
            {
                return NotFound("role is not in this set");
            }
            _context.SetRoles.Remove(role);
            await _context.SaveChangesAsync();
            return role;
        }

        /// <summary>
        /// Adds a new role into given set
        /// </summary>
        /// <param name="id">Set ID</param>
        /// <param name="sr">Data of new role</param>
        /// <returns>new role</returns>
        [HttpPost("{id}/roles")]
        [Authorize(Policy = "Administrator")]
        public async Task<ActionResult<SetTerm>> PostSetRoles(int id, [FromBody] SetRoleIdInputModel sr)
        {
            var set = await _context.Sets.FindAsync(id);
            if (set == null)
            {
                return NotFound("set not found");
            }
            var newRole = new SetRole { 
                SetId = id, 
                Name = sr.Name, 
                ClassTeacher = sr.ClassTeacher, Manager = sr.Manager, 
                PrintedInApplication = sr.PrintedInApplication,
                PrintedInReview = sr.PrintedInReview,
            };
            _context.SetRoles.Add(newRole);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetSetRole", new { id = newRole.SetId, roleId = newRole.Id }, newRole);
        }

        // -- questions

        /// <summary>
        /// Gets collection of question in specific term and role (both should be in same set)
        /// </summary>
        /// <param name="setId">id of set (not used)</param>
        /// <param name="termId">id of term in set</param>
        /// <param name="roleId">id of role in set</param>
        /// <returns>question</returns>
        [HttpGet("{setId}/questions/{termId}/{roleId}")]
        public ActionResult<IEnumerable<SetQuestion>> GetSetQuestions(int setId, int termId, int roleId)
        {
            var setQuestions = _context.SetQuestions
                .Where(sq => sq.SetRoleId == roleId && sq.SetTermId == termId)
                .OrderBy(sq => sq.Order)
                .AsNoTracking();
            return Ok(setQuestions);
        }

        /// <summary>
        /// Gets one question by its id
        /// </summary>
        /// <param name="id">question internal id</param>
        /// <returns>question</returns>
        [HttpGet("{setId}/questions/{id}")]
        public async Task<ActionResult<SetRole>> GetSetQuestion(int id)
        {
            var @question = await _context.SetQuestions.FindAsync(id);
            if (@question == null)
            {
                return NotFound("question not found");
            }
            return Ok(question);
        }

        /// <summary>
        /// Gets one question from term, role and order in term+role combination
        /// </summary>
        /// <param name="termId">id of term</param>
        /// <param name="roleId">id of role</param>
        /// <param name="order">order</param>
        /// <returns></returns>
        [HttpGet("{setId}/questions/{termId}/{roleId}/{order}")]
        public async Task<ActionResult<SetRole>> GetSetQuestionOrder(int termId, int roleId, int order)
        {
            var question = await _context.SetQuestions.Where(sq => sq.SetRoleId == roleId && sq.SetTermId == termId && sq.Order == order).FirstOrDefaultAsync();
            if (@question == null)
            {
                return NotFound("question not found");
            }
            return Ok(question);
        }

        /// <summary>
        /// Creates a new question in role and term
        /// </summary>
        /// <param name="setId">id of set (not actually needed, just verified)</param>
        /// <param name="termId">id of term</param>
        /// <param name="roleId">id of role</param>
        /// <param name="item">question data</param>
        /// <returns>created question</returns>
        [HttpPost("{setId}/questions/{termId}/{roleId}")]
        [Authorize(Policy = "Administrator")]
        public async Task<ActionResult<SetQuestion>> PostSetQuestion(int setId, int termId, int roleId, [FromBody] SetQuestionInputModel item)
        {
            var set = await _context.Sets.FindAsync(setId);
            if (set == null)
            {
                return NotFound("set not found");
            }
            var @role = await _context.SetRoles.FindAsync(roleId);
            if (@role == null)
            {
                return NotFound("role not found");
            }
            var @term = await _context.SetTerms.FindAsync(termId);
            if (@term == null)
            {
                return NotFound("term not found");
            }

            if (term.SetId != set.Id && role.SetId != set.Id)
            {
                return BadRequest("role or term is outside this set");
            }

            int maxQuestionOrder;
            try
            {
                maxQuestionOrder = _context.SetQuestions.Where(sq => sq.SetRoleId == roleId && sq.SetTermId == termId).Max(sq => sq.Order) + 1;
            }
            catch
            {
                maxQuestionOrder = 0;
            }

            var newQuestion = new SetQuestion
            {
                SetTermId = termId,
                SetRoleId = roleId,
                Text = item.Text,
                Description = item.Description,
                Points = item.Points,
                Order = maxQuestionOrder
            };
            _context.SetQuestions.Add(newQuestion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSetQuestion", new { setId =  setId, id = newQuestion.Id}, newQuestion);
        }

        /// <summary>
        /// deletes question given by its internal id and reorganizes all remaining to maintain same order
        /// </summary>
        /// <param name="id">id of setquestion</param>
        /// <returns>deleted question</returns>
        [HttpDelete("{setId}/questions/{id}")]
        public async Task<ActionResult<SetQuestion>> DeleteSetQuestion(int id)
        {
            var question = await _context.SetQuestions.FindAsync(id);
            if (question == null)
            {
                return NotFound("question not found");
            }

            int maxQuestionOrder;
            try
            {
                maxQuestionOrder = _context.SetQuestions.Where(sq => 
                    sq.SetRoleId == question.SetRoleId 
                    &&
                    sq.SetTermId == question.SetTermId
                ).Max(sq => sq.Order);
            }
            catch
            {
                maxQuestionOrder = 0;
            }

            _context.SetQuestions.Remove(question);
            _context.SaveChanges();

            for (int i = question.Order; i <= maxQuestionOrder; i++)
            {
                var row = _context.SetQuestions.Where(sq => 
                    sq.SetRoleId == question.SetRoleId
                    &&
                    sq.SetTermId == question.SetTermId
                    &&
                    sq.Order == i).FirstOrDefault();
                if (row != null)
                {
                    row.Order = i - 1;
                    _context.SaveChanges();
                }
            }

            return Ok(question);
        }

        /// <summary>
        /// updates question by its id
        /// </summary>
        /// <param name="setId">id of set</param>
        /// <param name="questionId">id of question</param>
        /// <param name="item">question data</param>
        /// <returns>updated question</returns>
        [HttpPut("{setId}/questions/{questionId}")]
        [Authorize(Policy = "Administrator")]
        public async Task<ActionResult<SetQuestion>> PutSetQuestion(int setId, int questionId, [FromBody] SetQuestionInputModel item)
        {
            var set = await _context.Sets.FindAsync(setId);
            if (set == null)
            {
                return NotFound("set not found");
            }
            var @question = await _context.SetQuestions.FindAsync(questionId);
            if (@question == null)
            {
                return NotFound("question not found");
            }
            if (questionId != item.Id)
            {
                return BadRequest("inconsistent data");
            }
            question.Text = item.Text;
            question.Description = item.Description;
            question.Points = item.Points;
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetSetQuestion", new { setId = setId, id = question.Id }, question);
        }

        /// <summary>
        /// changes order of specific question and reorders questions to maintain order
        /// </summary>
        /// <param name="setId">id of set</param>
        /// <param name="termId">id of term</param>
        /// <param name="roleId">id of role</param>
        /// <param name="order">current order of question</param>
        /// <param name="newOrder">new order of question</param>
        /// <returns></returns>
        [HttpPut("{setId}/questions/{termId}/{roleId}/{order}/moveto/{newOrder}")]
        [Authorize(Policy = "Administrator")]
        public async Task<ActionResult<SetQuestion>> PutSetQuestionMove(int setId, int termId, int roleId, int order, int newOrder)
        {
            var set = await _context.Sets.FindAsync(setId);
            if (set == null)
            {
                return NotFound("set not found");
            }

            var question = _context.SetQuestions.Where(sq => sq.SetRoleId == roleId && sq.SetTermId == termId && sq.Order == order).FirstOrDefault();
            if (question == null)
            {
                return NotFound("question not found");
            }

            int maxOrder;
            try
            {
                maxOrder = _context.SetQuestions.Where(sq => sq.SetRoleId == roleId && sq.SetTermId == termId).Max(sq => sq.Order);
            }
            catch
            {
                maxOrder = 0;
            }

            if (newOrder > maxOrder) newOrder = maxOrder;

            if (order > newOrder) // moving down
            {
                for (int i = order - 1; i >= newOrder; i--)
                {
                    var item = _context.SetQuestions.Where(sq => sq.SetRoleId == roleId && sq.SetTermId == termId && sq.Order == i).FirstOrDefault();
                    if (item != null) item.Order = item.Order + 1;
                    _context.SaveChanges();
                }
            }
            else if (order < newOrder) // moving up
            {
                for (int i = order + 1; i <= newOrder; i++)
                {
                    var item = _context.SetQuestions.Where(sq => sq.SetRoleId == roleId && sq.SetTermId == termId && sq.Order == i).FirstOrDefault();
                    if (item != null) item.Order = item.Order - 1;
                    _context.SaveChanges();
                }
            }

            question.Order = newOrder;
            _context.SaveChanges();
            return NoContent();
        }

        // -- answers
    }

    class SetListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public bool Active { get; set; }
        public ApplicationTemplate Template { get; set; }
    }

    public class SetTermIdInputModel
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public DateTime WarningDate { get; set; }
    }

    public class SetRoleIdInputModel
    {
        public string Name { get; set; }
        public bool ClassTeacher { get; set; }
        public bool Manager { get; set; }
        public bool PrintedInApplication { get; set; }
        public bool PrintedInReview { get; set; }
    }

    public class SetQuestionInputModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public int Points { get; set; }
    }
}
