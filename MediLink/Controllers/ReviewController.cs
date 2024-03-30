using MediLink.Entities;
using MediLink.Models;
using MediLink.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MediLink.Controllers
{
    public class ReviewController : Controller
    {
        public ReviewController(MediLinkDbContext mediLinkContext, IUserService userService)
        {
            _mediLinkContext = mediLinkContext;
            _userService = userService;
        }

        public async Task<IActionResult> UnreviewedPractitioners()
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string userName = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                userName = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
            }

            ViewData["userName"] = userName;

            Patient patient = await _userService.GetUserByEmail(userName);

            List<int> reviewedPractitionerIds = await _mediLinkContext.PractitionerReviews.Where(pr => pr.PatientId == patient.Id).Select(pr => pr.PractitionerId).ToListAsync();

            var unreviewedPractitioners = await _mediLinkContext.PractitionerPatients.Where(p => p.PatientId == patient.Id && !reviewedPractitionerIds.Contains(p.PractitionerId)).Include(p => p.Practitioner).ToListAsync();

            PractitionerReviewViewModel viewModel = new PractitionerReviewViewModel
            {
                review = new PractitionerReview(),
                currentPractitioners = unreviewedPractitioners.Where(p => p.IsCurrent == true).ToList(),
                pastPractitioners = unreviewedPractitioners.Where(p => p.IsCurrent != true).ToList(),
            };

            if (TempData["Success"] != null)
            {
                ViewBag.Success = TempData["Success"];
            }

            return View(viewModel);
        }

        public async Task<IActionResult> ReviewedPractitioners()
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string userName = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                userName = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
            }

            ViewData["userName"] = userName;

            Patient patient = await _userService.GetUserByEmail(userName);

            var reviews = await _mediLinkContext.PractitionerReviews.Where(pr => pr.PatientId == patient.Id).Include(pr => pr.Practitioner).ToListAsync();
            List<int> reviewedPractitionerIds = reviews.Select(r => r.PractitionerId).ToList();

            var reviewedPractitioners = await _mediLinkContext.PractitionerPatients.Where(p => p.PatientId == patient.Id && reviewedPractitionerIds.Contains(p.PractitionerId)).ToListAsync();

            List<PractitionerReview> currentReviews = new List<PractitionerReview>();
            List<PractitionerReview> pastReviews = new List<PractitionerReview>();

            foreach (var p in reviewedPractitioners)
            {
                if (p.IsCurrent)
                {
                    currentReviews.Add(reviews.Find(r => r.PractitionerId == p.PractitionerId));
                }
                else
                {
                    pastReviews.Add(reviews.Find(r => r.PractitionerId == p.PractitionerId));
                }
            }

            PractitionerReviewViewModel viewModel = new PractitionerReviewViewModel
            {
                currentPractitionerReviews = currentReviews,
                pastPractitionerReviews = pastReviews
            };

            if (TempData["Success"] != null)
            {
                ViewBag.Success = TempData["Success"];
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddReview(PractitionerReview review)
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string userName = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                userName = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
            }

            Patient patient = await _userService.GetUserByEmail(userName);

            review.PatientId = patient.Id;

            _mediLinkContext.PractitionerReviews.Add(review);
            await _mediLinkContext.SaveChangesAsync();

            TempData["Success"] = "Your review has been added.";

            return RedirectToAction("UnreviewedPractitioners", "Review");
        }

        [HttpPost]
        public async Task<IActionResult> EditReview(PractitionerReview review)
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string userName = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                userName = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
            }

            Patient patient = await _userService.GetUserByEmail(userName);

            review.PatientId = patient.Id;

            _mediLinkContext.PractitionerReviews.Update(review);
            await _mediLinkContext.SaveChangesAsync();

            TempData["Success"] = "Your review has been updated.";

            return RedirectToAction("ReviewedPractitioners", "Review");
        }

        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = _mediLinkContext.PractitionerReviews.Find(id);
            _mediLinkContext.PractitionerReviews.Remove(review);
            await _mediLinkContext.SaveChangesAsync();

            TempData["Success"] = "Your review has been deleted";

            return RedirectToAction("ReviewedPractitioners", "Review");
        }

        public async Task<IActionResult> PractitionerReviews()
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string userName = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                userName = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
            }

            ViewData["userName"] = userName;

            Practitioner practitioner = await _userService.GetPractitionerByEmail(userName);

            var reviews = await _mediLinkContext.PractitionerReviews.Where(pr => pr.PractitionerId == practitioner.Id).ToListAsync();

            PractitionerReviewViewModel viewModel = new PractitionerReviewViewModel
            {
                currentPractitionerReviews = reviews,
            };

            return View(viewModel);
        }

        private MediLinkDbContext _mediLinkContext;
        private IUserService _userService;
    }
}
