using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.CoreCli.Abstractions;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Datahub;
using TheFipster.Aviation.Modules.Airports.Abstractions;
using TheFipster.Aviation.Modules.Airports.Components;
using TheFipster.Aviation.Modules.FlightPlan.Abstractions;
using TheFipster.Aviation.Modules.FlightPlan.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IFlightFinder, FlightFinder>();
builder.Services.AddTransient<IFlightFileScanner, FlightFileScanner>();
builder.Services.AddTransient<IFlightMeta, FlightMeta>();
builder.Services.AddTransient<IFileOperations, FileOperations>();

builder.Services.AddTransient<IOperationsPlan, OperationsPlan>();
builder.Services.AddTransient<IJsonReader<Stats>, JsonReader<Stats>>();

builder.Services.AddSingleton<IAirportFinder, AirportFinder>();
builder.Services.AddTransient<IJsonReader<IEnumerable<Airport>>, JsonReader<IEnumerable<Airport>>>();

builder.Services.AddTransient<IFlightPlanReader, FlightPlanReader>();
builder.Services.AddTransient<IFlightPlanWriter, FlightPlanWriter>();
builder.Services.AddTransient<IJsonReader<IEnumerable<Leg>>, JsonReader<IEnumerable<Leg>>>();
builder.Services.AddTransient<IJsonWriter<IEnumerable<Leg>>, JsonWriter<IEnumerable<Leg>>>();

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAll",
                      policy =>
                      {
                          policy.WithOrigins("*");
                          policy.WithMethods("*");
                          policy.WithHeaders("*");
                      });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureSwaggerGen(options =>
{
    options.CustomSchemaIds(x => x.FullName);
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
