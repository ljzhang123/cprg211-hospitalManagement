using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalManagement.Models;

namespace HospitalManagement.Data
{
	public interface IPatientRepository
	{
		Task<List<Patient>> GetAllAsync();
		Task<Patient?> GetByIdAsync(int id); // Can return null if not found
		Task AddAsync(Patient patient);
		Task UpdateAsync(Patient patient);
		Task DeleteAsync(int id);
		Task<List<Gender>> GetGendersAsync(); // Needed for dropdowns
	}
}
