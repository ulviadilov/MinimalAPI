using MinimalApi.DAL;
using Microsoft.EntityFrameworkCore;
using MinimalApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MinimalApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<AppDbContext>(x => x.UseInMemoryDatabase("Students"));
            var app = builder.Build();
            app.MapPost("/students", async(Student student,AppDbContext dbContext) =>
            {
                await dbContext.Students.AddAsync(student);
                await dbContext.SaveChangesAsync();
                return Results.Created($"/students/{student.Id}",student);
            });

            app.MapGet("/students/{id}", async (int id, AppDbContext dbContext) =>
            {
                return await dbContext.Students.FindAsync(id);
            });

            app.MapGet("/students", async(AppDbContext dbContext) => {
                return await dbContext.Students.ToListAsync();
            });

            app.MapPut("/students/{id}", async (int id,Student student, AppDbContext dbContext) =>
            {
                
                if (await dbContext.Students.FindAsync(id) is Student existStudent)
                {
                    existStudent.Age = student.Age;
                    existStudent.Name = student.Name;
                    await dbContext.SaveChangesAsync();
                    return Results.NoContent();
                }
                return Results.NotFound();
                
            });

            app.MapDelete("/students/{id}", async (int id, AppDbContext dbContext) =>
            {
                if(await dbContext.Students.FindAsync(id) is Student student)
                {
                    dbContext.Students.Remove(student);
                    await dbContext.SaveChangesAsync();
                    return Results.Ok("Student removed from database !");
                }
                return Results.NotFound();
            });

            app.Run();
        }
    }
}
