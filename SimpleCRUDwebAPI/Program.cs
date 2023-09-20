using Microsoft.EntityFrameworkCore;
using SimpleCRUDwebAPI.DAL;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Enable internal debugging for log4net 
log4net.Util.LogLog.InternalDebugging = true;


//clear inbuilt logger providers
builder.Logging.ClearProviders();

// initialize log4net
log4net.Config.XmlConfigurator.Configure(new FileInfo("log4net.config"));

//log4net Registration added
builder.Logging.AddLog4Net();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Register Db connection string 
builder.Services.AddDbContext<MyAppDbContext>(options => options.UseSqlServer
(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
