var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

List<Item> items = new List<Item>
{
    new Item { Id = 1, Name = "Item Uno" },
    new Item { Id = 2, Name = "Item Dos" },
    new Item { Id = 3, Name = "Item Tres" },
    new Item { Id = 4, Name = "Item Cuatro" }
};

// GET /items
app.MapGet("/items", () =>
{
return Results.Ok(items);
}).WithName("GetAll")
.WithOpenApi();

// GET /items/{id}
app.MapGet("/items/{id}", (int id) =>
{
var item = items.FirstOrDefault(i => i.Id == id);
return item != null ? Results.Ok(item) : Results.NotFound("Item no encontrado");
}).WithName("GetItemById")
.WithOpenApi();

// POST /items
app.MapPost("/items", (Item newItem) =>
{
newItem.Id = items.Count + 1;
items.Add(newItem);
return Results.Created($"/items/{newItem.Id}", newItem);
}).WithName("CreateItem")
.WithOpenApi();

// PUT /items/{id}
app.MapPut("/items/{id}", (int id, Item updatedItem) =>
{
var item = items.FirstOrDefault(i => i.Id == id);
if (item is null) return Results.NotFound("Item no encontrado");

item.Name = updatedItem.Name;
return Results.Ok(item);
}).WithName("UpdateItem")
.WithOpenApi();

// DELETE /items/{id}
app.MapDelete("/items/{id}", (int id) =>
{
var item = items.FirstOrDefault(i => i.Id == id);
if (item is null) return Results.NotFound("Item no encontrado");

items.Remove(item);
return Results.NoContent();
}).WithName("DeleteItemById")
.WithOpenApi();

app.Run();

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}