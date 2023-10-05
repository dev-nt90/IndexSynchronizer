/*
 * 
 * TODO:
 * 
 * TESTS
 * 
 * *Write SQL query/procedure to output index definition of input table
 *     *Deploy sproc or keep logic internal to tool?
 *     *Support multiple database platforms? SQL Server, MySQL, Postgres? Introduces N complexity for back, maybe for middle.
 *         *SQL Server sys.indexes/tables/etc
 *         *MySQL performance.?
 *         *Postgres pg_indexes view
 * *Front-end input/logic for FQDN+Instance+Database+Table+User+Password (maybe database platform?)
 * *Front-end output/logic for index definitions
 * *Front-end button/logic to sync
 * *Front-end button/logic to only read these definitions (i.e. "preview")
 * *Front-end security 
 *     *https/SSL/TLS
 *     *Cross-site request forgery (CSRF) protection
 *     *input sanitation
 *     *Content Security Policy (CSP)
 * *Front/middle/back: toggle between sql and windows auth for DB access
 * *Middleware controllers
 * *Middleware logging
 * *Middleware exception handling
 * *Middleware database client(s)
 * *Middleware object mapping? Would this change on a per-database-platform case? SQL Server indexes have a number of options
 * *Easier to pass index definitions directly from A to B OR to issue middleware commands to apply index N? At the point of issuing commands, does it make sense to implement event stores? Other DDD techniques?
 * *Statistics monitoring i.e. when was the last time these indexes were synchronized? 
 *     *Requires tracking on a per-database (per-user?) level
 *     *Would be helpful to capture user information
 *     *Separate page from sync actions? maybe a pop-up?
 * *Registration/Authentication
 *     *In-memory? sqlite?
 *     *This area requires the most research
 * *"Strict" option to only allow exact table name matches? e.g. don't want to over TableB with TbleA
 */
using IndexSynchronizer.Hubs;
using IndexSynchronizerServices.Extensions;

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
