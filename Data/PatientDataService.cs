using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalManagement.Data;
using HospitalManagement.Models;

namespace HospitalManagement.Data
{
	// interacts with UI to perform CRUD functions
	public class PatientDataService
	{
		private readonly IPatientRepository _patientRepository;

		// inject repository
		public PatientDataService(IPatientRepository patientRepository)
		{
			_patientRepository = patientRepository;
		}

		public async Task<List<Patient>> GetAllPatientsAsync()
		{
			return await _patientRepository.GetAllAsync();
		}

		public async Task<Patient?> GetPatientByIdAsync(int id)
		{
			return await _patientRepository.GetByIdAsync(id);
		}

		public async Task AddPatientAsync(Patient patient)
		{
			// Example: Could add validation logic here before saving
			if (patient.DateOfBirth > DateTime.Today)
			{
				throw new ArgumentException("Date of Birth cannot be in the future.");
			}
			await _patientRepository.AddAsync(patient);
		}

		public async Task UpdatePatientAsync(Patient patient)
		{
			// Example: More validation could go here
			await _patientRepository.UpdateAsync(patient);
		}

		public async Task DeletePatientAsync(int id)
		{
			// Example: Add check if patient can be deleted based on rules
			await _patientRepository.DeleteAsync(id);
		}

		public async Task<List<Gender>> GetGenders()
		{
			return await _patientRepository.GetGendersAsync();
		}
	}
}
