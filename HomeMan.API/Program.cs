using HomeMan.API.Helpers;
using HomeMan.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// add services to DI container
var services = builder.Services;
var env = builder.Environment;

// configure DI for application services
builder.Services.AddScoped<IUserService, UserService>();

// EF DB context
services.AddDbContext<DataContext>();

services.AddCors();
services.AddControllers().AddJsonOptions(x =>
{
    // serialize enums as strings in api responses (e.g. Role)
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

    // ignore omitted parameters on models to enable optional params (e.g. User update)
    x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// add a default httpContext accessor here to read stuff like the username or role
services.AddHttpContextAccessor();

// JWT authentication stuff here ...
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)), // get the key from appsettings
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();

// Swagger without JWT authorization
services.AddSwaggerGen();

// Swagger with JWT authorization
services.AddSwaggerGen(options =>
 {
     options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
     {
         Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
         In = ParameterLocation.Header,
         Name = "Authorization",
         Type = SecuritySchemeType.ApiKey
     });

     options.OperationFilter<SecurityRequirementsOperationFilter>();
 });

var app = builder.Build();

// configure HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// global cors policy
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

// global error handler
app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

// activate JWT authentication (important: call this before UseAuthorization()!)
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();