 using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Description = @"JWT Authorization header using the Bearer scheme.
                        Enter 'Bearer'[space] and then your token in the text input below.
                        Example: 'Bearer 1234qwertr'"
    });
    c.OperationFilter<SecurityRequirementsOperationFilter>();
});
var key = Environment.GetEnvironmentVariable("KEY");
builder.Services.AddAuthentication(//options =>
//{
//options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;}
).AddJwtBearer(options =>
{
options.SaveToken = true;
options.RequireHttpsMetadata = false;
options.TokenValidationParameters = new TokenValidationParameters()
{
    ValidateIssuerSigningKey = true,
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidAudience = builder.Configuration["Jwt:Audience"],
    ValidIssuer = builder.Configuration["Jwt:Issuer"],
    ClockSkew = TimeSpan.Zero,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
    };
}).Services.AddCors(options =>
{
    options.AddPolicy("allowAll", corsPolicyBuilder =>
    {
        corsPolicyBuilder.AllowAnyOrigin();
        corsPolicyBuilder.AllowAnyHeader();
        corsPolicyBuilder.AllowAnyMethod();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("allowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
