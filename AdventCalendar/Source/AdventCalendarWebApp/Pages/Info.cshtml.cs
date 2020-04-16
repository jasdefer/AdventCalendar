using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;
using System.IO;
using System.Xml;

namespace AdventCalendarWebApp.Pages
{
    public class InfoModel : PageModel
    {
        public string Version { get; set; }
        public string Description { get; set; }
        public string[] ReleaseNotes { get; set; }
        public void OnGet()
        {
            var assembly = typeof(Startup).Assembly;
            Version = assembly.GetName().Version.ToString();
            Description = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;

            try
            {
                //Read the release notes
                //I did not find any way to read the release notes as I did for the above version and description
                //So I just read the *.csproj file
                var file = Path.Combine(Environment.CurrentDirectory, "AdventCalendarWebApp.csproj");
                var xml = new XmlDocument();
                xml.Load(file);
                ReleaseNotes = xml.GetElementsByTagName("PackageReleaseNotes").Item(0).FirstChild.Value.Split('\n');
            }
            catch (Exception)
            {
                ReleaseNotes = Array.Empty<string>();
            }
        }
    }
}