using BLL.classes;
using BLL.inter;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(opotion => opotion.AddPolicy("AllowAll",//נתינת שם להרשאה
                p => p.AllowAnyOrigin()//מאפשר כל מקור
                .AllowAnyMethod()//כל מתודה - פונקציה
                .AllowAnyHeader()));//וכל כותרת פונקציה
builder.Services.AddControllers()
           .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddScoped<IbuildGraph ,buildGraph>();
var app = builder.Build();
app.UseCors("AllowAll");




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
