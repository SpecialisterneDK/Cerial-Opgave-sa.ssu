using CerealDB;
using CerealDB.Models;
using CerealDB.Service;
using CerealDB.Settings;
using SkiaSharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;

//This is the primary code that will be executed when the program is launched.

//It is not possible to simply move parts of this code off to other classes, as that will lead to client execution.

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Cereals") ?? "Data Source=Cereals.db";
builder.Services.AddSqlite<CerealContext>(connectionString);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddScoped<CerealService>();
builder.Services.AddScoped<ImageService>();
builder.Services.AddSingleton<TokenService>();

var secretKey = ApiSettings.GenerateSecretByte();
builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("admin", policy => policy.RequireRole("admin"));
});



var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CerealContext>();

    dbContext.Database.EnsureCreated();
}

app.MapGet("/", () => "Hello World!");

app.MapPost("/login", (User userModel, TokenService service) =>
{
    var user = UserRepository.Find(userModel.Username, userModel.Password);

    if (user is null)
        return Results.NotFound(new { message = "Invalid username or password" });

    var token = TokenService.GenerateToken(user);

    user.Password = string.Empty;

    return Results.Ok(new { user, token });
});

app.MapGet("/admin", (ClaimsPrincipal user) =>
{
    Results.Ok(new { message = $"Authenticated as {user?.Identity?.Name}" });
}).RequireAuthorization("admin");


app.MapGet("/results/{id}", async (int id, CerealContext db) =>
await db.Cereals.FindAsync(id)
is Cereal cereal ? Results.Ok(cereal) : Results.NotFound());


app.MapGet("/results", async ([AsParameters] SearchCriteria criteria, CerealContext db) =>
await db.Cereals.Where(t => (criteria.Name == null || t.Name == criteria.Name)
    && (criteria.Mfr == null || t.MFR == criteria.Mfr)
    && (criteria.Type == null || t.Type == criteria.Type)
    && (criteria.Calories == null || t.Calories == criteria.Calories)
    && (criteria.Protein == null || t.Protein == criteria.Protein)
    && (criteria.Fat == null || t.Fat == criteria.Fat)
    && (criteria.Sodium == null || t.Sodium == criteria.Sodium)
    && (criteria.Fiber == null || t.Fiber == criteria.Fiber)
    && (criteria.Carbo == null || t.Carbo == criteria.Carbo)
    && (criteria.Sugars == null || t.Sugars == criteria.Sugars)
    && (criteria.Potass == null || t.Potass == criteria.Potass)
    && (criteria.Vitamins == null || t.Vitamins == criteria.Vitamins)
    && (criteria.Shelf == null || t.Shelf == criteria.Shelf)
    && (criteria.Weight == null || t.Weight == criteria.Weight)
    && (criteria.Cups == null || t.Cups == criteria.Cups)
    && (criteria.Rating == null || t.Rating == criteria.Rating)
    ).ToListAsync());

app.MapGet("/filter", async ([AsParameters] FilterCriteria criteria, CerealContext db) =>
{
    string[] validparas = { "calories", "protein", "fat", "sodium", "fiber", "carbo", "sugars", "potass", "vitamins", "shelf", "weight", "cups", "rating" };
    string[] validops = { "<=", "<", ">=", ">", "!=" };
    string[] validorderby = { "id","mfr","type", "calories", "protein", "fat", "sodium", "fiber", "carbo", "sugars", "potass", "vitamins", "shelf", "weight", "cups", "rating" };
    if (!(validparas.Contains(criteria.Parameter) && validops.Contains(criteria.Op) && validorderby.Contains(criteria.Orderby))) { return Results.NotFound(); };
    var query = $"SELECT * FROM Cereals WHERE {criteria.Parameter} {criteria.Op} {criteria.Value} ORDER BY {criteria.Orderby}";
    var result = await db.Cereals.FromSqlRaw(query).ToListAsync();
    return Results.Ok(result);
});

app.MapPost("/submit", async (Cereal cerial, CerealContext db) =>
{
    db.Cereals.Add(cerial);
    await db.SaveChangesAsync();
    return Results.Created($"/results/{cerial.Id}", cerial);

}).RequireAuthorization("admin");

app.MapPost("/submit/{id}", async (int id, Cereal inputCereal, CerealContext db) =>
{
    var cereal = await db.Cereals.FindAsync(id);

    if (cereal == null) return Results.NotFound();

    cereal.Name = inputCereal.Name;
    cereal.MFR = inputCereal.MFR;
    cereal.Calories = inputCereal.Calories;
    cereal.Protein = inputCereal.Protein;
    cereal.Fat = inputCereal.Fat;
    cereal.Sodium = inputCereal.Sodium;
    cereal.Fiber = inputCereal.Fiber;
    cereal.Carbo = inputCereal.Carbo;
    cereal.Sugars = inputCereal.Sugars;
    cereal.Potass = inputCereal.Potass;
    cereal.Vitamins = inputCereal.Vitamins;
    cereal.Shelf = inputCereal.Shelf;
    cereal.Weight = inputCereal.Weight;
    cereal.Cups = inputCereal.Cups;
    cereal.Rating = inputCereal.Rating;

    await db.SaveChangesAsync();

    return Results.NoContent();

}).RequireAuthorization("admin");

app.MapDelete("/results/{id}", async (int id, CerealContext db) =>
{
    if (await db.Cereals.FindAsync(id) is Cereal cereal)
    {
        db.Cereals.Remove(cereal);
        await db.SaveChangesAsync();
        return Results.Ok(cereal);
    }
    return Results.NotFound();
}).RequireAuthorization("admin");


app.MapGet("/image/{id}", async (int id, CerealContext db) => {
    var productImage = await db.ProductImages.FindAsync(id);
    if (productImage is null)
        return Results.NotFound(new { message = "No Image with that ID" });

    var surface = SKSurface.Create(new SKImageInfo(500, 500));
    var canvas = surface.Canvas;
    canvas.Clear(SKColors.Transparent);


    var productImageData = productImage.Imagefile;
    var body = SKBitmap.Decode(productImageData);
    canvas.DrawBitmap(body, 0, 0);

    var resultImage = surface.Snapshot();
    var imageData = resultImage.Encode(SKEncodedImageFormat.Png, 100);

    return Results.File(imageData.ToArray(), "image/png");
}); 

app.Run();
