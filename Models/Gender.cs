using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Models
{
	public class Gender
	{
		[Key]
		public int GenderId { get; set; }

		[Required]
		[StringLength(50)]
		public string Name { get; set; }

		// Navigation property: A Gender can be associated with many Patients
		[InverseProperty("Gender")]
		public virtual List<Patient> Patients { get; set; } = new List<Patient>();
	}
}
