using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalManagement.Models;


namespace HospitalManagement.Data
{
	public class PatientRepository : IPatientRepository
	{
		private readonly AppDbContext _context; 

		public PatientRepository(AppDbContext context)
		{
			_context = context;
		}

		// get all patients, ordered by last name then first name
		public async Task<List<Patient>> GetAllAsync()
		{
			try
			{
				return await _context.Patients
									 .Include(p => p.Gender) 
									 .OrderBy(p => p.LastName)
									 .ThenBy(p => p.FirstName)
									 .AsNoTracking() 
									 .ToListAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error fetching all patients: {ex.Message}"); 
				throw; // Rethrow for upper layer handling
			}
		}

		// Get a single patient by ID, including Gender
		public async Task<Patient?> GetByIdAsync(int id)
		{
			try
			{
				return await _context.Patients
									 .Include(p => p.Gender) 
									 .FirstOrDefaultAsync(p => p.PatientId == id);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error fetching patient by ID {id}: {ex.Message}");
				throw;
			}
		}

		// Add a new patient
		public async Task AddAsync(Patient patient)
		{
			if (patient == null)
			{
				throw new ArgumentNullException(nameof(patient)); 
			}

			try
			{
				_context.Patients.Add(patient); // Stage the addition
				await _context.SaveChangesAsync(); // Commit to database
			}
			catch (DbUpdateException ex) // Catch specific DB exceptions
			{
				Console.WriteLine($"Error adding patient: {ex.InnerException?.Message ?? ex.Message}");
				throw new ApplicationException("Could not save patient data. Please check input.", ex); // More specific exception for UI
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Generic error adding patient: {ex.Message}");
				throw;
			}
		}

		// Update an existing patient
		public async Task UpdateAsync(Patient patient)
		{
			if (patient == null)
			{
				throw new ArgumentNullException(nameof(patient));
			}

			// Optional: Check if patient exists before attempting update
			var existingPatient = await _context.Patients.FindAsync(patient.PatientId);
			if (existingPatient == null)
			{
				throw new KeyNotFoundException($"Patient with ID {patient.PatientId} not found for update.");
			}

			try
			{
				// Detach the existing entity if it's being tracked to avoid conflicts
				_context.Entry(existingPatient).State = EntityState.Detached;

				_context.Patients.Update(patient); // Stage the update
												   // Or, update only specific fields if needed:
												   // _context.Entry(existingPatient).CurrentValues.SetValues(patient);
				await _context.SaveChangesAsync(); // Commit
			}
			catch (DbUpdateConcurrencyException ex) // Handle concurrency issues (if someone else edited)
			{
				Console.WriteLine($"Concurrency error updating patient ID {patient.PatientId}: {ex.Message}");
				// Can implement retry logic or inform user here
				throw new ApplicationException("The patient record was modified by another user. Please reload and try again.", ex);
			}
			catch (DbUpdateException ex)
			{
				Console.WriteLine($"Error updating patient ID {patient.PatientId}: {ex.InnerException?.Message ?? ex.Message}");
				throw new ApplicationException("Could not update patient data. Please check input.", ex);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Generic error updating patient ID {patient.PatientId}: {ex.Message}");
				throw;
			}
		}

		// Delete a patient by ID
		public async Task DeleteAsync(int id)
		{
			try
			{
				var patientToDelete = await _context.Patients.FindAsync(id); // Find the patient first
				if (patientToDelete != null)
				{
					_context.Patients.Remove(patientToDelete); // Stage deletion
					await _context.SaveChangesAsync(); // Commit
				}
				else
				{
					// Optional: throw specific exception if delete target not found
					throw new KeyNotFoundException($"Patient with ID {id} not found for deletion.");
				}
			}
			catch (DbUpdateException ex) // Catch FK constraint issues etc.
			{
				Console.WriteLine($"Error deleting patient ID {id}: {ex.InnerException?.Message ?? ex.Message}");
				// Could check for specific SQL Server error codes (e.g., 547 for FK violation)
				throw new ApplicationException($"Could not delete patient. Check related records (e.g., Appointments if implemented). Details: {ex.InnerException?.Message}", ex);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Generic error deleting patient ID {id}: {ex.Message}");
				throw;
			}
		}

		// Get list of genders for dropdowns
		public async Task<List<Gender>> GetGendersAsync()
		{
			try
			{
				return await _context.Genders
									.OrderBy(g => g.Name)
									.AsNoTracking()
									.ToListAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error fetching genders: {ex.Message}");
				throw;
			}
		}
	}
}
