using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagement.Models
{
	public class Patient
	{
		[Key]
		public int PatientId { get; set; }

		[Required(ErrorMessage = "First Name is required")] // validate name
		[StringLength(100, ErrorMessage = "First Name cannot be longer than 100 characters")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Last Name is required")]
		[StringLength(100, ErrorMessage = "Last Name cannot be longer than 100 characters")]
		public string LastName { get; set; }

		[Required(ErrorMessage = "Date of Birth is required")]
		[DataType(DataType.Date)] // helps with UI formatting/input type
								  
		public DateTime DateOfBirth { get; set; } = DateTime.Today; 

		[Required(ErrorMessage = "Gender is required")]
		[Range(1, int.MaxValue, ErrorMessage = "Please select a valid Gender")] // ensure a Gender is selected
		public int GenderId { get; set; } // foreign key

		[ForeignKey("GenderId")]
		[InverseProperty("Patients")]
		public virtual Gender? Gender { get; set; }

		[Phone(ErrorMessage = "Invalid Phone Number format")]
		[StringLength(20)]
		public string? PhoneNumber { get; set; } 

		[StringLength(255)]
		public string? Address { get; set; } 

		[StringLength(100)]
		public string? EmergencyContactName { get; set; } 

		[Phone(ErrorMessage = "Invalid Emergency Phone Number format")]
		[StringLength(20)]
		public string? EmergencyContactPhone { get; set; } 
	}
}
