using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebApplication1.Models;
using WebApplication1.Repository.Interface;
using WebApplication1.Repository.Services;

var builder = WebApplication.CreateBuilder(args);

//Added for repository using in Api
builder.Services.AddScoped<IEmployeeRepo, EmployeeRepository>();


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<FileUploadOperationFilter>(); // Register the custom operation filter
});


// Added JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, //token was issued by trusted authority, issuer is who generated  toke i.e server
        ValidateAudience = true, // audience represents the recipient of the token (e.g., your API).The audience is usually a string (e.g., "JwtAudience") defined in the appsettings.json file.
        ValidateLifetime = true, //checks if the token is expired based on the exp (expiry) claim.
        ValidateIssuerSigningKey = true, //Ensures the token's signature is valid and matches the secret or key used to sign it.
        ValidIssuer = builder.Configuration["Jwt:Issuer"], //specifies expected issuer of token. value (Jwt:Issuer) is retrieved from the appsettings.json file.
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) //Defines the key used to validate the token's signature. 
    };
});


//JWT Authorization
builder.Services.AddAuthorization();

//connecting path from here //added
builder.Services.AddDbContext<EmplyoeeContext>(db =>
db.UseSqlServer(builder.Configuration.GetConnectionString("ConnEmp"))); //connemp is in application.json

//added
builder.Services.AddCors(cors => cors.AddPolicy("MyPolicy", builder=>
{
builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();


//Added for JWT user authentication
app.UseAuthentication();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



//app.UseHttpsRedirection();
app.UseCors("MyPolicy"); //should be before UseAuthorization and after UseHttpsRedirection

app.UseAuthorization();
//for images
app.UseStaticFiles();

app.MapControllers();

app.Run();


