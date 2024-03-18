using MediLink.Entities;

namespace MediLink.Models
{
    public class PractitionerReviewViewModel
    {
        public PractitionerReview review = new PractitionerReview();

        public List<PractitionerReview> currentPractitionerReviews { get; set; }

        public List<PractitionerReview> pastPractitionerReviews { get; set; }

        public List<PractitionerPatient> currentPractitioners { get; set; }

        public List<PractitionerPatient> pastPractitioners { get; set; }
    }
}
