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
    public class UsersController : ControllerBase
    {
        private readonly ThesesContext _context;

        public UsersController(ThesesContext context)
        {
            _context = context;
        }

        // GET: /Users
        /// <summary>
        /// Gets list of users satisfying parameters.
        /// </summary>
        /// <param name="search">Firstname, lastname and email fields will be searched if this text is contained (even partially) within.</param>
        /// <param name="firstname">If FirstName of record contains this text, this record will be returned.</param>
        /// <param name="lastname">If LastName of record contains this text, this record will be returned.</param>
        /// <param name="email">If Email of record contains this text, this record will be returned.</param>
        /// <param name="author">If user can be author of work, this record will be returned.</param>
        /// <param name="evaluator">If user can be evaluator of works, this record will be returned.</param>
        /// <param name="order">Sorting order of result - valid values are: firstname, firstname_desc, lastname (default), lastname_desc, email, email_desc, id, id_desc</param>
        /// <param name="page">Index of currently returned page of result. Starts with 0, which is default value.</param>
        /// <param name="pagesize">Size of returned page. Default is 0. If 0, no paging is performed.</param>
        /// <returns>
        /// anonymous object: {
        /// total = amount of all users,
        /// filtered = number of records after filter was applied,
        /// count = number of records after filter and paging was applied,
        /// page = index of returned page,
        /// pages = count of pages for applied filter, returns 0 if no paging was requested,
        /// data = list of users
        /// }
        /// </returns>
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers(string search = null, string firstname = null, string lastname = null, string email = null, bool? author = null, bool? evaluator = null, string order = "lastname", int page = 0, int pagesize = 0)
        {
            IQueryable<User> users = _context.Users;
            int total = users.CountAsync().Result;
            if (!String.IsNullOrEmpty(search))
                users = users.Where(u => (u.FirstName.Contains(search) || u.LastName.Contains(search) || u.Email.Contains(search)));
            if (!String.IsNullOrEmpty(firstname))
                users = users.Where(u => (u.FirstName.Contains(firstname)));
            if (!String.IsNullOrEmpty(lastname))
                users = users.Where(u => (u.LastName.Contains(lastname)));
            if (!String.IsNullOrEmpty(email))
                users = users.Where(u => (u.FirstName.Contains(email)));
            if (author != null)
                users = users.Where(u => (u.CanBeAuthor == author));
            if (evaluator != null)
                users = users.Where(u => (u.CanBeEvaluator == evaluator));
            int filtered = users.CountAsync().Result;
            switch (order)
            {
                case "firstname":
                    users = users.OrderBy(u => u.FirstName);
                    break;
                case "firstname_desc":
                    users = users.OrderByDescending(u => u.FirstName);
                    break;
                case "lastname":
                    users = users.OrderBy(u => u.LastName);
                    break;
                case "lastname_desc":
                    users = users.OrderByDescending(u => u.LastName);
                    break;
                case "email":
                    users = users.OrderBy(u => u.Email);
                    break;
                case "email_desc":
                    users = users.OrderByDescending(u => u.Email);
                    break;
                case "id_desc":
                    users = users.OrderByDescending(u => u.Id);
                    break;
                case "id":
                default:
                    users = users.OrderBy(u => u.Id);
                    break;
            }
            if (pagesize != 0 )
            {
                users = users.Skip(page * pagesize).Take(pagesize);
            }
            var count = users.CountAsync().Result;
            return Ok(new { total = total, filtered = filtered, count = count, page = page, pages = ((pagesize == 0) ? 0 : Math.Ceiling((double)filtered / pagesize)), data = users.AsNoTracking() });
        }

        // GET: /Users/aaa
        /// <summary>
        /// Gets data of one user specified by his Id
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns>User</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        // PUT: /Users/aaa
        /// <summary>
        /// Overwrites data of user specified by his Id
        /// </summary>
        /// <param name="id">User Id</param>
        /// <param name="user">New User data</param>
        /// <returns>HTTP 204,400,404</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: /Users
        /// <summary>
        /// Creates and stores a new user, unless user with this Id already exists
        /// </summary>
        /// <param name="user">User data</param>
        /// <returns>HTTP 201, 200, 400</returns>
        [HttpPost]
        public async Task<ActionResult<User>> PostUser([FromBody] User user)
        {
            var existingUser = await _context.Users.FindAsync(user.Id);
            if (existingUser == null)
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetUser", new { id = user.Id }, user);
            }
            else
            {
                return Ok(existingUser);
            }
        }

        // DELETE: /Users/aaa
        /// <summary>
        /// Deletes one user specified by his Id
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns>User data if success, HTTP 404 if not found</returns>
        [HttpDelete("{id}")] 
        public async Task<ActionResult<User>> DeleteUser(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }
        /// <summary>
        /// Check if user with Id exists.
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns>boolean</returns>
        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
