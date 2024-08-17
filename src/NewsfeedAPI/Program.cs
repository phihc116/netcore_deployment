using AppTelemetry;
using NewsfeedAPI.Abtractions;
using Refit;

var builder = WebApplication.CreateBuilder(args);
 
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRefitClient<IPlatformApi>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("http://platformapi:8080/api"));
 
builder.ConfigureBuilderDefault();
 
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
 

app.UseAuthorization();

app.MapControllers();

app.ConfigureDefautApp();
app.ConfigureMapEndpointDefault();

app.Run();
