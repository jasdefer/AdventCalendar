using AdventCalendarWebApp.Helper.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.IO;
using System.IO.Compression;

namespace AdventCalendarWebApp.Pages.Misc
{
    public class LogsModel : PageModel
    {
        public IActionResult OnGet(string password)
        {
            if (password != Environment.GetEnvironmentVariable("LOG_PASSWORD"))
            {
                return NotFound();
            }
            var zipFile = Guid.NewGuid().ToString() + ".zip";
            ZipFile.CreateFromDirectory(StatisticLogger.LogDir, zipFile);
            var fs = new FileStream(zipFile, FileMode.Open, FileAccess.Read, FileShare.None, 4096, FileOptions.DeleteOnClose);
            return File(
                fileStream: fs,
                contentType: System.Net.Mime.MediaTypeNames.Application.Octet,
                fileDownloadName: "AdventCalendarLog.zip");
        }
    }
}