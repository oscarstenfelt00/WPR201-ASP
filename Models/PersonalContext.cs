using System;
using Microsoft.EntityFrameworkCore;
namespace Slutprojekt_ASP.Net.Models
{
	public class PersonalContext : DbContext
	{
		public DbSet<Personalen>? Personalen { get; set; }

		public PersonalContext()
		{
			Database.EnsureCreated();
		}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
			optionsBuilder.UseSqlite("Data Source=personalData.db");
        }
    }
}

