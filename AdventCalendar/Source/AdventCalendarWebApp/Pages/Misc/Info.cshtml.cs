using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace AdventCalendarWebApp.Pages.Misc;

public class InfoModel : PageModel
{
    private static readonly string CsprojPath = Path.Combine(Environment.CurrentDirectory, "AdventCalendarWebApp.csproj");
    public string Version { get; set; }
    public string PageDescription { get; set; }
    public string[] ReleaseNotes { get; set; } = new string[0];

    public void OnGet()
    {
        var assembly = typeof(Startup).Assembly;
        Version = assembly.GetName().Version.ToString();
        PageDescription = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;

        //Read the release notes
        //I did not find any way to read the release notes as I did for the above version and description
        //So I just read the *.csproj file
        if (System.IO.File.Exists(CsprojPath))
        {
            var xml = new XmlDocument();
            xml.Load(CsprojPath);
            ReleaseNotes = xml.GetElementsByTagName("PackageReleaseNotes").Item(0).FirstChild.Value.Split('\n');
        }
    }
}
