using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PslibThesesBackend.Constants;
using PslibThesesBackend.Models;

namespace PslibThesesBackend.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ThesesContext _context;
        private readonly ILogger<UsersController> _logger;

        public UsersController(ThesesContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
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
        public ActionResult<IEnumerable<User>> GetUsers(
            string search = null, 
            string firstname = null, 
            string lastname = null, 
            string email = null, 
            Gender? gender = null, 
            bool? author = null, 
            bool? evaluator = null, 
            string order = "lastname", 
            int page = 0, 
            int pagesize = 0)
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
            if (gender != null)
                users = users.Where(u => (u.Gender == gender));
            if (author != null)
                users = users.Where(u => (u.CanBeAuthor == author));
            if (evaluator != null)
                users = users.Where(u => (u.CanBeEvaluator == evaluator));
            int filtered = users.CountAsync().Result;
            users = order switch
            {
                "firstname" => users.OrderBy(u => u.FirstName),
                "firstname_desc" => users.OrderByDescending(u => u.FirstName),
                "lastname" => users.OrderBy(u => u.LastName),
                "lastname_desc" => users.OrderByDescending(u => u.LastName),
                "email" => users.OrderBy(u => u.Email),
                "email_desc" => users.OrderByDescending(u => u.Email),
                "id_desc" => users.OrderByDescending(u => u.Id),
                _ => users.OrderBy(u => u.Id),
            };
            if (pagesize != 0 )
            {
                users = users.Skip(page * pagesize).Take(pagesize);
            }
            var result = users.ToList();
            int count = result.Count();
            return Ok(new { total = total, filtered = filtered, count = count, page = page, pages = ((pagesize == 0) ? 0 : Math.Ceiling((double)filtered / pagesize)), data = result });
        }

        // GET: /Users/aaa
        /// <summary>
        /// Gets data of one user specified by his Id
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns>User</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                _logger.LogError("user not found",id);
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
        public async Task<IActionResult> PutUser(Guid id, User user)
        {
            if (id != user.Id)
            {
                _logger.LogError("user not found, so cannot be updated", id);
                return BadRequest();
            }

            if (!User.HasClaim(ClaimTypes.NameIdentifier, user.Id.ToString())
                && !User.HasClaim(Security.THESES_ROBOT_CLAIM, "1")
                && !User.HasClaim(Security.THESES_ADMIN_CLAIM, "1"))
            {
                return Unauthorized("only user himself or privileged user can edit user record");
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
                    _logger.LogError("storing of updated user has failed", user);
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("storing of updated user has failed", ex, user);
                throw;
            }
            _logger.LogInformation("new user data stored", user);
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
            if (!User.HasClaim(Security.THESES_ROBOT_CLAIM, "1")
                && !User.HasClaim(Security.THESES_ADMIN_CLAIM, "1"))
            {
                var loggedUserId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
                user.Id = Guid.Parse(loggedUserId.Value); // replace possibly forged Guid with Guid of current user, unless user is Admin
            }
            var existingUser = await _context.Users.FindAsync(user.Id);
            if (existingUser == null)
            {
                try
                {
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("new user created", user);
                    return CreatedAtAction("GetUser", new { id = user.Id }, user);
                }
                catch (Exception ex)
                {
                    _logger.LogError("storing of new user has failed", ex, user);
                    throw ex;
                }
            }
            else
            {
                _logger.LogInformation("existing user used", user);
                return Ok(existingUser);
            }
        }

        // DELETE: /Users/aaa
        /// <summary>
        /// Deletes one user specified by his Id
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns>User data if success, HTTP 404 if not found</returns>
        [Authorize(Policy = "Administrator")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(Guid id)
        {
            // TODO User roles
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                _logger.LogError("user not found, so cannot be deleted", id);
                return NotFound();
            }
            try
            {
                _context.Users.Remove(user);
                _logger.LogInformation("user deleted", user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("deleting of user has failed", ex, user);
                throw ex;
            }

            return user;
        }
        /// <summary>
        /// Check if user with Id exists.
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns>boolean</returns>
        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
