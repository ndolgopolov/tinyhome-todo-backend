using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TinyHomeTodo.Api.Middleware;
using TinyHomeTodo.Application.Dtos;
using TinyHomeTodo.Application.Interfaces;
using TinyHomeTodo.Application.Services;
using TinyHomeTodo.Infrastructure.Persistence;
using TinyHomeTodo.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var message = context.ModelState.Values
            .SelectMany(entry => entry.Errors)
            .Select(error => error.ErrorMessage)
            .FirstOrDefault(text => !string.IsNullOrWhiteSpace(text)) ?? "Invalid request.";

        return new BadRequestObjectResult(new ErrorResponseDto { Message = message });
    };
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TinyHomeTodoDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ITaskRepository, EfTaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

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
