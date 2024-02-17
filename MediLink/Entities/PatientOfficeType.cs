namespace MediLink.Entities
{
	public class PatientOfficeType
	{
		public int PatientPreferenceId { get; set; }

		public int OfficeTypeId { get; set; }

		public OfficeType OfficeType { get; set; } = null!;

		public PatientPreference PatientPreference { get; set; } = null!;
	}
}
