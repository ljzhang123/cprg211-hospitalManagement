using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Data
{
	public class AppDbContext : DbContext
	{
		public DbSet<Patient> Patients { get; set; }
		public DbSet<Gender> Genders { get; set; }

		// Constructor used by Dependency Injection
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Configure relationships if needed (Fluent API)
			// Many-to-One relationship: Patient -> Gender
			modelBuilder.Entity<Patient>()
				.HasOne(p => p.Gender)         // each patient only has one gender
				.WithMany(g => g.Patients)     // each gender can have many patients
				.HasForeignKey(p => p.GenderId) 
				.IsRequired()                   // gender is required for a Patient
				.OnDelete(DeleteBehavior.Restrict); // prevent gender delete if patients exist

			// configure Gender unique constraint
			modelBuilder.Entity<Gender>()
				.HasIndex(g => g.Name)
				.IsUnique();
		}
	}
}
