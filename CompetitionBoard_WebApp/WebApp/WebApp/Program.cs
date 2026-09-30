using Base.Models.DB;
using DBSvcs;
using Interfaces;
using Microsoft.Extensions.Hosting.WindowsServices;
using Repos.RnHs;
using Repos.Title;
using WebApp.Components;
using WebApp.Configs;
using WebApp.Services.Board;
using WebApp.Services.Login;
using WebApp.Services.StandByModus;

WebApplicationOptions options = new WebApplicationOptions
{
    Args = args,
    // Als Windows-Dienst ist das Arbeitsverzeichnis C:\Windows\System32,
    // appsettings.json und wwwroot liegen aber neben der exe.
    ContentRootPath = WindowsServiceHelpers.IsWindowsService() ? AppContext.BaseDirectory : default
};

var builder = WebApplication.CreateBuilder(options);

builder.Host.UseWindowsService(serviceOptions => serviceOptions.ServiceName = "CompetitionBoard");

builder.Services.Configure<DBConnectionSettings>(builder.Configuration.GetSection("Db"));
builder.Services.Configure<UIConfig>(builder.Configuration.GetSection("UI"));

// Pro Browser-Verbindung eine eigene DB-Verbindung, da DBSvc nicht threadsicher ist.
builder.Services.AddScoped<IDBSvc, DBSvc>();
builder.Services.AddScoped<IRnHsRepo, RnHsRepo>();
builder.Services.AddScoped<ITitleRepo, TitleRepo>();
builder.Services.AddScoped<BoardSvc>();

builder.Services.AddHostedService<StandBySvc>();

builder.Services.AddSingleton(new PasswordStore(
    Path.Combine(builder.Environment.ContentRootPath, "appsettings.json")));

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
