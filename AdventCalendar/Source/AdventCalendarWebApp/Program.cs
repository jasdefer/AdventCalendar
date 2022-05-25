using AdventCalendarWebApp.Helper.TimeProvider;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddTransient<ITimeProvider, UtcTimeProvider>();
}
else
{
    builder.Services.AddTransient<ITimeProvider, UtcTimeProvider>();
}
builder.Services.AddTransient<DayValidation>();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Pipeline
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}
app.UseSession();
app.UseStatusCodePagesWithReExecute("/Error/{0}");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
});
app.Run();
