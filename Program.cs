using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ssd_authorization_solution;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("AppDb");
    options.UseSqlite(connectionString);
});
builder.Services.AddScoped<DbSeeder>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();

builder
    .Services.AddIdentityApiEndpoints<IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

var app = builder.Build();

// Seed
using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<DbSeeder>().SeedAsync().Wait();
}

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapIdentityApi<IdentityUser>();

app.Run();

