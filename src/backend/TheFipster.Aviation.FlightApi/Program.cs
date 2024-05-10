using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.CoreCli.Abstractions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IFlightFinder, FlightFinder>();
builder.Services.AddTransient<IFlightFileScanner, FlightFileScanner>();
builder.Services.AddTransient<IFlightMeta, FlightMeta>();
builder.Services.AddTransient<IFileOperations, FileOperations>();

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAll",
                      policy =>
                      {
                          policy.WithOrigins("*");
                      });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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
