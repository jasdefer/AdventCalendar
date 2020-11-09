using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace AdventCalendarWebApp.Pages.Misc
{
    public class InfoModel : PageModel
    {
        public string Version { get; set; }
        public string PageDescription { get; set; }
        public string[] ReleaseNotes { get; set; } = new string[0];
        private static readonly string[] ProjectPaths = new string[]
        {
            Path.Combine(Environment.CurrentDirectory, "AdventCalendarWebApp.csproj"),
            Path.Combine(Environment.SystemDirectory, "AdventCalendarWebApp.csproj"),
            "AdventCalendarWebApp.csproj"
        };

        public void OnGet()
        {
            var assembly = typeof(Startup).Assembly;
            Version = assembly.GetName().Version.ToString();
            PageDescription = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;

            //Read the release notes
            //I did not find any way to read the release notes as I did for the above version and description
            //So I just read the *.csproj file
            var counter = 0;
            while (!System.IO.File.Exists(ProjectPaths[counter]))
            {
                counter++;
                if (counter >= ProjectPaths.Length)
                {
                    return;
                }
            }
            var xml = new XmlDocument();
            xml.Load(ProjectPaths[counter]);
            ReleaseNotes = xml.GetElementsByTagName("PackageReleaseNotes").Item(0).FirstChild.Value.Split('\n');
        }
    }
}