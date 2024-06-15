using Microsoft.EntityFrameworkCore;
using Datalayer.Data;
using Datalayer.Repository.IRepository;
using Datalayer.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Set the URLs to listen on
builder.WebHost.UseUrls("http://0.0.0.0:5000"); // یا URL و پورت دلخواه خودتان

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
