using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", policy =>
    {
        // Allowing localhost for development
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();

        // In the future, a front-end domain may be added
        policy.WithOrigins("https://norkart-frontend.netlify.app")
              .AllowAnyHeader().AllowAnyMethod();
    });
});
var app = builder.Build();

app.UseCors("ReactPolicy");


var norkartOffices = new List<NorkartOffice>
{
    new(1, "Skøyen", "Hovedkontor", "Hoffsveien 4, 0275 Oslo", "Hoffsveien 4, 0275 Oslo", 59.9254, 10.6747),
    new(2, "Trondheim", "Distriktskontor", "Holtermanns veg 7, Blokk C, 5. etasje, 7030 Trondheim", "Hoffsveien 4, 0275 Oslo", 63.4142, 10.3979),
    new(3, "Lillehammer", "Distriktskontor", "Fåberggt. 155, 2615 Lillehammer", "Fåberggt. 155, 2615 Lillehammer", 61.1237, 10.4572),
    new(4, "Bergen", "Distriktskontor", "Inger Bang Lunds vei 12, 5059 Bergen", "Inger Bang Lunds vei 12, 5059 Bergen", 60.3727, 5.3414),
    // Will be added from the UI
    // new(5, "Kristiansand", "Distriktskontor", "Markens gate 19, 3. etg, 4611 Kristiansand", "Norkart AS (Kristiansand)\nAtt: Alexander N. / Roman\nMarkens Gate 19, 3. etg.\n4611 KRISTIANSAND S", 58.1449, 7.9938)
};

app.MapGet("/api/offices", () => TypedResults.Ok(norkartOffices));
app.MapGet("/api/offices/{id:int}", (int id) =>
    norkartOffices.Find(o => o.Id == id) is NorkartOffice o ? Results.Ok(o) : Results.NotFound());

app.MapGet("/api/hello", () =>
    Results.Ok(new { message = "Klar for Norkart!" }));


app.MapPost("/api/offices", (NorkartOffice office) =>
{
    // Unique check
    if (norkartOffices.Any(o =>
      o.Name.Equals(office.Name, StringComparison.OrdinalIgnoreCase)))
    {
        return Results.Conflict(new { message = "Office with same name already exists" });
    }
    // coordinates check
    if (office.Lat < -90 || office.Lat > 90 || office.Lon < -180 || office.Lon > 180)
        return Results.BadRequest("Invalid coordinates");

    var newOffice = office with { Id = norkartOffices.Max(o => o.Id) + 1 }; // auto-increment id
    norkartOffices.Add(newOffice);
    return Results.Created($"/api/offices/{newOffice.Id}", newOffice);

})
.Accepts<NorkartOffice>("application/json");

app.MapPost("/api/reset", () =>
{
    if (norkartOffices.Count == 0)
    {
        norkartOffices.AddRange([
            new(1, "Skøyen", "Hovedkontor", "Hoffsveien 4, 0275 Oslo", "Hoffsveien 4, 0275 Oslo", 59.9254, 10.6747),
        new(2, "Trondheim", "Distriktskontor", "Holtermanns veg 7, Blokk C, 5. etasje, 7030 Trondheim", "Hoffsveien 4, 0275 Oslo", 63.4142, 10.3979),
        new(3, "Lillehammer", "Distriktskontor", "Fåberggt. 155, 2615 Lillehammer", "Fåberggt. 155, 2615 Lillehammer", 61.1237, 10.4572),
        new(4, "Bergen", "Distriktskontor", "Inger Bang Lunds vei 12, 5059 Bergen", "Inger Bang Lunds vei 12, 5059 Bergen", 60.3727, 5.3414),
    ]);
        return Results.Ok(new { message = "Data reset to original!" });
    }

    return Results.Ok(new { message = "The list is already full" });
});

app.MapDelete("/api/offices/{id:int}", (int id) =>
{
    var item = norkartOffices.Find(e => e.Id == id);
    if (item is null) return Results.NotFound();
    norkartOffices.Remove(item);
    return Results.NoContent();
});


app.Run();

record NorkartOffice(
    int Id,
    string Name,           // By / Kontor navn
    string Type,           // Kontortype (Hovedkontor / Distriktskontor)
    string VisitAddress,   // Besøksadresse
    string PostalAddress,  // Postadresse
    double Lat,
    double Lon
);



