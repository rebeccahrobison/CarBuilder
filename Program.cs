using CarBuilder.Models;
using CarBuilder.Models.DTOs;

List<PaintColor> paintColors = new List<PaintColor>
{
    new PaintColor { Id = 1, Price = 50.99m, Color = "Red" },
    new PaintColor { Id = 2, Price = 55.99m, Color = "Blue" },
    new PaintColor { Id = 3, Price = 45.50m, Color = "Green" },
    new PaintColor { Id = 4, Price = 60.75m, Color = "Yellow" }
};

List<Interior> interiors = new List<Interior>
{
    new Interior { Id = 1, Price = 150.75m, Material = "Leather" },
    new Interior { Id = 2, Price = 120.50m, Material = "Fabric" },
    new Interior { Id = 3, Price = 200.25m, Material = "Suede" },
    new Interior { Id = 4, Price = 180.00m, Material = "Vinyl" }
};

List<Technology> technologies = new List<Technology>
{
    new Technology { Id = 1, Price = 500.00m, Package = "Advanced" },
    new Technology { Id = 2, Price = 350.25m, Package = "Basic" },
    new Technology { Id = 3, Price = 450.50m, Package = "Standard" },
    new Technology { Id = 4, Price = 600.75m, Package = "Premium" }
};

List<Wheels> wheels = new List<Wheels>
{
    new Wheels { Id = 1, Price = 300.00m, Style = "Alloy" },
    new Wheels { Id = 2, Price = 250.50m, Style = "Steel" },
    new Wheels { Id = 3, Price = 350.75m, Style = "Chrome" },
    new Wheels { Id = 4, Price = 280.50m, Style = "Black" }
};

List<Order> orders = new List<Order>
{
    new Order { Id = 1, Timestamp = new DateTime(2023, 11, 26), WheelId = 1, InteriorId = 2, PaintId = 3, TechnologyId = 4 },
    new Order { Id = 2, Timestamp = new DateTime(2023, 11, 25), WheelId = 2, InteriorId = 3, PaintId = 4, TechnologyId = 1 },
    new Order { Id = 3, Timestamp = new DateTime(2023, 11, 24), WheelId = 3, InteriorId = 4, PaintId = 1, TechnologyId = 2 },
    new Order { Id = 4, Timestamp = new DateTime(2023, 11, 23), WheelId = 4, InteriorId = 1, PaintId = 2, TechnologyId = 3 }
};

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(options =>
            {
                options.AllowAnyOrigin();
                options.AllowAnyMethod();
                options.AllowAnyHeader();
            });
}

app.UseHttpsRedirection();

app.MapGet("/interiors", () =>
{
    return interiors.Select(i => new InteriorDTO
    {
        Id = i.Id,
        Price = i.Price,
        Material = i.Material
    });
});

app.MapGet("/interiors/{id}", (int id) =>
{
    Interior interior = interiors.FirstOrDefault(i => i.Id == id);
    if (interior == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(new InteriorDTO
    {
        Id = interior.Id,
        Price = interior.Price,
        Material = interior.Material
    });
});

app.MapGet("/paintcolors", () =>
{
    return paintColors.Select(pc => new PaintColorDTO
    {
        Id = pc.Id,
        Color = pc.Color,
        Price = pc.Price
    });
});

app.MapGet("/paintcolors/{id}", (int id) =>
{
    PaintColor paintColor = paintColors.FirstOrDefault(pc => pc.Id == id);
    if (paintColor == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(new PaintColorDTO
    {
        Id = paintColor.Id,
        Color = paintColor.Color,
        Price = paintColor.Price
    });
});

app.MapGet("/technologies", () =>
{
    return technologies.Select(t => new TechnologyDTO
    {
        Id = t.Id,
        Package = t.Package,
        Price = t.Price
    });
});

app.MapGet("/technologies/{id}", (int id) =>
{
    Technology technology = technologies.FirstOrDefault(t => t.Id == id);
    if (technology == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(new TechnologyDTO
    {
        Id = technology.Id,
        Package = technology.Package,
        Price = technology.Price
    });
});

app.MapGet("/wheels", () =>
{
    return wheels.Select(w => new WheelsDTO
    {
        Id = w.Id,
        Price = w.Price,
        Style = w.Style
    });
});

app.MapGet("/wheels/{id}", (int id) =>
{
    Wheels wheel = wheels.FirstOrDefault(w => w.Id == id);
    if (wheel == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(new WheelsDTO
    {
        Id = wheel.Id,
        Price = wheel.Price,
        Style = wheel.Style
    });
});

app.MapGet("/orders", () =>
{
    List<Order> unfulfilledOrders = orders.Where(o => o.Fulfilled == false).ToList();
    
    return unfulfilledOrders.Select(o => new OrderDTO
    {
        Id = o.Id,
        Timestamp = o.Timestamp,
        WheelId = o.WheelId,
        TechnologyId = o.TechnologyId,
        PaintId = o.PaintId,
        InteriorId = o.InteriorId,
        Fulfilled = o.Fulfilled,
        Wheels = wheels.FirstOrDefault(w => w.Id == o.WheelId) == null ? null : new WheelsDTO
        {
            Id = o.WheelId,
            Price = wheels.First(w => w.Id == o.WheelId).Price,
            Style = wheels.First(w => w.Id == o.WheelId).Style
        },
        Technology = technologies.FirstOrDefault(t => t.Id == o.TechnologyId) == null ? null : new TechnologyDTO
        {
            Id = o.TechnologyId,
            Price = technologies.First(t => t.Id == o.TechnologyId).Price,
            Package = technologies.First(t => t.Id == o.TechnologyId).Package
        },
        PaintColor = paintColors.FirstOrDefault(pc => pc.Id == o.PaintId) == null ? null : new PaintColorDTO
        {
            Id = o.PaintId,
            Price = paintColors.First(pc => pc.Id == o.PaintId).Price,
            Color = paintColors.First(pc => pc.Id == o.PaintId).Color
        },
        Interior = interiors.FirstOrDefault(i => i.Id == o.InteriorId) == null ? null : new InteriorDTO
        {
            Id = o.InteriorId,
            Price = interiors.First(i => i.Id == o.InteriorId).Price,
            Material = interiors.First(i => i.Id == o.InteriorId).Material
        }
    });
});

app.MapGet("orders/{id}", (int id) =>
{
    Order order = orders.FirstOrDefault(o => o.Id == id);
    if (order == null)
    {
        return Results.NotFound();
    }

    Wheels wheel = wheels.FirstOrDefault(w => w.Id == order.WheelId);
    Technology technology = technologies.FirstOrDefault(t => t.Id == order.TechnologyId);
    Interior interior = interiors.FirstOrDefault(i => i.Id == order.InteriorId);
    PaintColor paintColor = paintColors.FirstOrDefault(pc => pc.Id == order.PaintId);

    return Results.Ok(new OrderDTO
    {
        Id = order.Id,
        Timestamp = order.Timestamp,
        Fulfilled = order.Fulfilled,
        WheelId = order.WheelId,
        Wheels = wheels == null ? null : new WheelsDTO
        {
            Id = wheel.Id,
            Price = wheel.Price,
            Style = wheel.Style
        },
        TechnologyId = order.TechnologyId,
        Technology = technologies == null ? null : new TechnologyDTO
        {
            Id = technology.Id,
            Price = technology.Price,
            Package = technology.Package
        },
        PaintId = order.PaintId,
        PaintColor = paintColors == null ? null : new PaintColorDTO
        {
            Id = paintColor.Id,
            Price = paintColor.Price,
            Color = paintColor.Color
        },
        InteriorId = order.InteriorId,
        Interior = interiors == null ? null : new InteriorDTO
        {
            Id = interior.Id,
            Price = interior.Price,
            Material = interior.Material
        }
    });
});

app.MapPost("/orders", (Order order) =>
{
    order.Id = orders.Max(o => o.Id) + 1;
    order.Timestamp = DateTime.Now;
    Wheels wheel = wheels.FirstOrDefault(w => w.Id == order.WheelId);
    Technology technology = technologies.FirstOrDefault(t => t.Id == order.TechnologyId);
    Interior interior = interiors.FirstOrDefault(i => i.Id == order.InteriorId);
    PaintColor paintColor = paintColors.FirstOrDefault(pc => pc.Id == order.PaintId);

    if (wheel == null || technology == null || interior == null || paintColor == null)
    {
        return Results.BadRequest();
    }

    orders.Add(order);

    return Results.Created($"/orders/{order.Id}", new OrderDTO
    {
        Id = order.Id,
        Timestamp = order.Timestamp,
        WheelId = order.WheelId,
        Wheels = new WheelsDTO
        {
            Id = wheel.Id,
            Price = wheel.Price,
            Style = wheel.Style
        },
        TechnologyId = order.TechnologyId,
        Technology = new TechnologyDTO
        {
            Id = technology.Id,
            Price = technology.Price,
            Package = technology.Package
        },
        PaintId = order.PaintId,
        PaintColor = new PaintColorDTO
        {
            Id = paintColor.Id,
            Price = paintColor.Price,
            Color = paintColor.Color
        },
        InteriorId = order.InteriorId,
        Interior = new InteriorDTO
        {
            Id = interior.Id,
            Price = interior.Price,
            Material = interior.Material
        }
    });
});

app.MapPost("/orders/{id}/fulfill", (int id) =>
{
    Order orderToFulfill = orders.FirstOrDefault(o => o.Id == id);
    orderToFulfill.Fulfilled = true;
});



app.Run();