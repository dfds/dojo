using Demo.Domain.Repositories;
using Demo.Infrastructure.Context;
using Demo.Infrastructure.Repositories;
using Infrastructure.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<DbContext, DemoDbContext>();
builder.Services.AddDbContext<DemoDbContext>(opts => opts.UseSqlServer(builder.Configuration["ConnectionString:DBBlog"]));
builder.Services.AddScoped<DemoDbContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();



var app = builder.Build();

// migrate any database changes on program (includes initial db creation)
using (var scope = app.Services.CreateScope())
{
    var demoDbContext = scope.ServiceProvider.GetRequiredService<DemoDbContext>();
    demoDbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
