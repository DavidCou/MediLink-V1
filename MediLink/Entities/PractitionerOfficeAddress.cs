namespace MediLink.Entities
{
	public class PractitionerOfficeAddress
	{
		public int PractitionerId { get; set; }

		public int OfficeAddressesId { get; set; }

		public OfficeAddress OfficeAddresse { get; set; } = null!;

		public Practitioner Practitioner { get; set; } = null!;
	}
}
