using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WritingIdeas.Data;
using WritingIdeas.Authorization;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();
//Auth handlers
builder.Services.AddScoped<IAuthorizationHandler, IsOwnerIdeaAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, AdministratorAuthorizationHandler>();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services= scope.ServiceProvider;
    var context= services.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
    string testPass = builder.Configuration.GetValue<string>("TestUser");
    // dotnet user-secrets set "TestUser" <Pass>
    if (context.Users.Count() == 0)
        await SeedAccounts.Initialize(services, testPass);
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
