using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.CoreCli.Abstractions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IFileSystemFinder, FileSystemFinder>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
