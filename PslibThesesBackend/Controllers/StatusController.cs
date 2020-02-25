using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PslibThesesBackend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        [HttpGet("Logs")]
        public List<string> GetLogs()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Log");
            List<string> filePaths = Directory.GetFiles(@path).Select(Path.GetFileName).ToList();
            return filePaths;
        }

        [HttpGet("Logs/{filename}")]
        public async Task<ActionResult> DownloadLog(string filename)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Log", filename);
            if (System.IO.File.Exists(path))
            {
                var memory = new MemoryStream();
                try
                {
                    using (var stream = new FileStream(path, FileMode.Open))
                    {
                        await stream.CopyToAsync(memory);
                    }
                    memory.Position = 0;
                    return File(memory, "text/plain", Path.GetFileName(path));
                }
                catch (IOException)
                {
                    return BadRequest("file is not accessible");
                }
            }
            return NotFound("file " + filename + " does not exists");
        }
    }
}