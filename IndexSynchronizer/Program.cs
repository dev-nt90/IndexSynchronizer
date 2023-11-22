/*
 * TODO:
 * *Front-end button/logic to sync
 * *Front-end button/logic to only read these definitions (i.e. "preview")
 * *Front-end security 
 *     *https/SSL/TLS
 *     *Cross-site request forgery (CSRF) protection
 *     *input sanitation
 *     *Content Security Policy (CSP)
 * *Front/middle/back: toggle between sql and windows auth for DB access
 * *Statistics monitoring i.e. when was the last time these indexes were synchronized? 
 *     *Requires tracking on a per-database (per-user?) level
 *     *Would be helpful to capture user information
 *     *Separate page from sync actions? maybe a pop-up?
 * *Registration/Authentication
 *     *In-memory? sqlite?
 *     *This area requires the most research
 
 * *Desperate back-end need for certs
 * 
 * ENHANCEMENTS
 * 
 * *The name might be "index" sync, but there's no reason for that to not extend to other parts of a given schema.
 * *With a kinda functioning CI pipeline, it's time to make some automated tests
 *     *CI pipeline needs secret management
 * *Support for multiple platforms in addition to sql server. Oracle, postgres, etc. To get this to work would require mapping syntax from db a to db b. Makes this more like a schema transformer.
 * *"Strict" option to only allow exact table name matches? e.g. don't want to over TableB with TbleA
 */

using IndexSynchronizer.Hubs;
using IndexSynchronizerServices.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSignalR(options => { options.EnableDetailedErrors = true; });
builder.Services.AddIndexSyncRepositories();
builder.Services.AddIndexSyncServices();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=IndexSync}/{action=IndexSync}/{id?}");

app.MapRazorPages();
app.MapHub<IndexSyncHub>("/IndexSyncHub");
app.MapHub<IndexPreviewHub>("/IndexPreviewHub");

app.Run();
