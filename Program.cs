var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    Console.WriteLine("Developement");
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
