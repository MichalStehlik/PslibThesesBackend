﻿using System;
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
    public class TargetsController : ControllerBase
    {
        private readonly ThesesContext _context;

        public TargetsController(ThesesContext context)
        {
            _context = context;
        }

        // GET: Targets
        /// <summary>
        /// Gets list of targets satisfying parameters.
        /// </summary>
        /// <param name="search">If Text of record contains this text, this record will be returned.</param>
        /// <param name="text">If Text of record contains this text, this record will be returned.</param>
        /// <param name="order">Sorting order of result - valid values are: text, text_desc, id, id_desc</param>
        /// <param name="page">Index of currently returned page of result. Starts with 0, which is default value.</param>
        /// <param name="pagesize">Size of returned page. Default is 0. If 0, no paging is performed.</param>
        /// <returns>
        /// anonymous object: {
        /// total = amount of all targets,
        /// filtered = number of records after filter was applied,
        /// count = number of records after filter and paging was applied,
        /// page = index of returned page,
        /// pages = count of pages for applied filter, returns 0 if no paging was requested,
        /// data = list of targets
        /// }
        /// </returns>
        [HttpGet]
        public ActionResult<IEnumerable<Target>> GetTargets(
            string search = null, 
            string text = null, 
            string order = "text", 
            int page = 0, 
            int pagesize = 0)
        {
            IQueryable<TargetViewModel> targets = _context.Targets.Select(t => 
                new TargetViewModel
                {
                    Id = t.Id,
                    Text = t.Text,
                    Color = t.Color
                }
            );
            int total = targets.CountAsync().Result;
            if (!String.IsNullOrEmpty(search))
                targets = targets.Where(t => (t.Text.Contains(search)));
            if (!String.IsNullOrEmpty(text))
                targets = targets.Where(t => (t.Text.Contains(text)));
            int filtered = targets.CountAsync().Result;
            switch (order)
            {
                case "text":
                    targets = targets.OrderBy(t => t.Text);
                    break;
                case "text_desc":
                    targets = targets.OrderByDescending(t => t.Text);
                    break;
                case "id_desc":
                    targets = targets.OrderByDescending(t => t.Id);
                    break;
                case "id":
                default:
                    targets = targets.OrderBy(t => t.Id);
                    break;
            }
            if (pagesize != 0)
            {
                targets = targets.Skip(page * pagesize).Take(pagesize);
            }
            var count = targets.CountAsync().Result;
            return Ok(new { total = total, filtered = filtered, count = count, page = page, pages = ((pagesize == 0) ? 0 : Math.Ceiling((double)filtered / pagesize)), data = targets.AsNoTracking() });
        }

        // GET: Targets/5
        /// <summary>
        /// Gets data of one target specified by his Id
        /// </summary>
        /// <param name="id">Target Id</param>
        /// <returns>Target</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Target>> GetTarget(int id)
        {
            var target = await _context.Targets.FindAsync(id);

            if (target == null)
            {
                return NotFound();
            }

            return target;
        }

        // PUT: Targets/5
        /// <summary>
        /// Overwrites data of target specified by his Id
        /// </summary>
        /// <param name="id">Target Id</param>
        /// <param name="target">Target data</param>
        /// <returns>Target</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTarget(int id, Target target)
        {
            if (id != target.Id)
            {
                return BadRequest();
            }

            _context.Entry(target).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TargetExists(id))
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

        // POST: Targets
        /// <summary>
        /// Creates and stores a new target
        /// </summary>
        /// <param name="target">Target data</param>
        /// <returns>Target</returns>
        [HttpPost]
        public async Task<ActionResult<Target>> PostTarget(Target target)
        {
            _context.Targets.Add(target);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetTarget", new { id = target.Id }, target);
        }

        // DELETE: Targets/5
        /// <summary>
        /// Deletes one target specified by its Id
        /// </summary>
        /// <param name="id">Target id</param>
        /// <returns>Target data if success, HTTP 404 if not found</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Target>> DeleteTarget(int id)
        {
            var target = await _context.Targets.FindAsync(id);
            if (target == null)
            {
                return NotFound();
            }

            _context.Targets.Remove(target);
            await _context.SaveChangesAsync();

            return target;
        }

        /// <summary>
        /// Check if target with Id exists.
        /// </summary>
        /// <param name="id">Target id</param>
        /// <returns>boolean</returns>
        private bool TargetExists(int id)
        {
            return _context.Targets.Any(e => e.Id == id);
        }
    }
}