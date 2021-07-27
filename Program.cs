using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

//factory to start
var factory = new LibraryContextFactory();
var context = factory.CreateDbContext(args);

//Insert
Console.WriteLine("Adding Book");
Book book = new Book() { Name = "b4", AboutBook = "test 4 for adding book", stars = 3 };
context.Books.Add(book);
await context.SaveChangesAsync();
Console.WriteLine($"Book added(id= {book.Id} )seccesfully");

//Select
Console.WriteLine("check stars of book");
var books = await context.Books.Where(b => b.Name == "b4").ToListAsync();
Console.WriteLine($"Book has {books[0].stars}");

//Update
Console.WriteLine("Change stars of book");
book.stars = 4;
await context.SaveChangesAsync();
Console.WriteLine("Stars changed");

//Delete
Console.WriteLine("Removing book from database");
context.Books.Remove(book);
await context.SaveChangesAsync();
Console.WriteLine($"Book (id ={book.Id}) removed");
class Book
{
	public int Id { get; set; }

	[MaxLength(100)]
	public string Name { get; set; } = string.Empty;

	[MaxLength(1000)]
	public string? AboutBook { get; set; }

	public int? stars { get; set; }

	public List<Athure> arthures { get; set; } = new();
}

class Athure
{
	public int Id { get; set; }

	[MaxLength(50)]
	public string Name { get; set; } = string.Empty;

	[Column(TypeName ="Decimal(5,2)")]
	public decimal amoumtofwhat { get; set; }

	public Book? book { get; set; }

	public int BookId { get; set; }
}

class LibraryContext : DbContext
{
	public DbSet<Book> Books { get; set; }
	public DbSet<Athure> Athures { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public LibraryContext(DbContextOptions<LibraryContext> options) :base(options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	{ }
}

class LibraryContextFactory : IDesignTimeDbContextFactory<LibraryContext>
{
	public LibraryContext CreateDbContext(string[] args)
	{
		var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();
        optionsBuilder
            // Uncomment the following line if you want to print generated
            // SQL statements on the console.
            //.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
            .UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);

        return new LibraryContext(optionsBuilder.Options);
	}
}