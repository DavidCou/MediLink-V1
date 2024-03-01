using MediLink.Entities;
using MediLink.Models;
using MediLink.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MediLink.Controllers
{
    public class SearchController : Controller
    {
        public SearchController(MediLinkDbContext mediLinkContext, IUserService userService)
        {
            _mediLinkContext = mediLinkContext;
            _userService = userService;
        }

        public async Task<IActionResult> SearchPractitioners()
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

            PatientPreference preferences = await _mediLinkContext.PatientPreferences.Where(p => p.Id == patient.PatientPreferencesId).FirstOrDefaultAsync();

            var viewModel = new PractitionerSearchViewModel();

            var selectedLanguagesId = new List<int>();
            var selectedOfficeTypesId = new List<int>();

            if(preferences != null)
            {
                selectedLanguagesId = await _mediLinkContext.PreferedLanguages.Where(pl => pl.PatientPreferenceId == preferences.Id).Select(pl => pl.LanguageId).ToListAsync();
                selectedOfficeTypesId = await _mediLinkContext.PatientOfficeTypes.Where(po => po.PatientPreferenceId == preferences.Id).Select(pl => pl.OfficeTypeId).ToListAsync();
                viewModel.minimumRating = preferences.rating??0;
            }

            var languages = _mediLinkContext.Languages.Where(l => l.IsDeleted == false).OrderBy(l => l.LanguageName).ToList();
            var officeTypes = _mediLinkContext.OfficeTypes.Where(ot => ot.IsDeleted == false).OrderBy(ot => ot.OfficeTypeName).ToList();

            return View();
        }

        public async Task<IActionResult> SearchWalkClinic()
        {

            ClaimsPrincipal claimuser = HttpContext.User;
            string userName = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                userName = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
            }

            ViewData["userName"] = userName;

            //get a list all the practitioners tyoes
            //List<WalkInClinic> walkInClinic = await _mediLinkContext.WalkInClinics.ToListAsync();

            // use the DB contet to query for all Address entities and transform them into
            // OfficeInf bjects:
            List<WalkClinicInfo> walkClinics = await _mediLinkContext.WalkInClinics
                        .Include(t => t.OfficeAddress)
                        .OrderByDescending(t => t.OfficeAddress.OfficeName)
                        .Select(t => new WalkClinicInfo()
                        {
                            OfficeName = t.OfficeAddress.OfficeName,
                            fullAddress = t.OfficeAddress.StreetAddress + " " + t.OfficeAddress.City + " " + t.OfficeAddress.zone + " " + t.OfficeAddress.PostalCode,
                            CurrentWaitTime = t.CurrentWaitTime,
                            HistoricalWaitTimeMin = t.HistoricalWaitTimeMin,
                            HistoricalWaitTimeMax = t.HistoricalWaitTimeMax,
                            Id = t.Id

                        })
                        .ToListAsync();

            ViewBag.WalkClinics = walkClinics;
            return View();
        }

        private MediLinkDbContext _mediLinkContext;
        private IUserService _userService;
    }
}
