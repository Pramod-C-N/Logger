using log4net.Config;
using log4net.Core;
using LoggerService;
using Microsoft.EntityFrameworkCore;
using NLog;
using SimpleCRUDwebAPI.DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(option => 
    {
        option.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))

        };

    });


//nlog
LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/Nlogs.config"));

// Enable internal debugging for log4net 
log4net.Util.LogLog.InternalDebugging = true;


    //clear inbuilt logger providers
    builder.Logging.ClearProviders();

    // initialize log4net
    XmlConfigurator.Configure(new FileInfo("log4net.config"));

    //log4net Registration added
    //builder.Logging.AddLog4Net();

    // Add services to the container.
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();


    //Register Db connection string 
    builder.Services.AddDbContext<MyAppDbContext>(options => options.UseSqlServer
    (builder.Configuration.GetConnectionString("DefaultConnection")));

//Register to DI container
builder.Services.AddScoped<ILoggersManager, LoggersManager>();

var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
//authentication add
app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
