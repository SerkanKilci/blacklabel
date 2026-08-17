using System.Text;
using Blacklabel.Application;
using Blacklabel.Application.Auth;
using Blacklabel.Infrastructure;
using Blacklabel.Infrastructure.Persistence;
using Blacklabel.Infrastructure.Storage;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter the JWT token returned by POST /api/v1/auth/device."
    });
});

builder.Services.AddHttpClient();

if (builder.Environment.IsDevelopment())
{
    // Only needed because the Expo web build runs on a different origin (e.g.
    // localhost:8090) than this API (localhost:5236) during local browser testing —
    // native builds never go through a browser's CORS layer. Not registered outside
    // Development.
    builder.Services.AddCors(options => options.AddPolicy("DevWeb", policy =>
        policy.SetIsOriginAllowed(_ => true).AllowAnyHeader().AllowAnyMethod()));
}

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>() ?? new JwtOptions();
builder.Services.AddSingleton(jwtOptions);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("DevWeb");

    if (app.Configuration.GetValue<bool>("Database:UseInMemory"))
    {
        using var scope = app.Services.CreateScope();
        scope.ServiceProvider.GetRequiredService<BlacklabelDbContext>().Database.EnsureCreated();
    }
}

app.UseHttpsRedirection();

var imageStorageOptions = app.Services.GetRequiredService<ImageStorageOptions>();
Directory.CreateDirectory(imageStorageOptions.RootPath);
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(imageStorageOptions.RootPath),
    RequestPath = imageStorageOptions.PublicPathPrefix,
    ContentTypeProvider = new FileExtensionContentTypeProvider()
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
