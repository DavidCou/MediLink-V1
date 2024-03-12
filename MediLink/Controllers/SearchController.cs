using MediLink.Entities;
using MediLink.Models;
using MediLink.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Runtime.InteropServices;
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

        [HttpGet]
        public async Task<IActionResult> SearchPractitioner()
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
            viewModel.minimumRating = 0;

            var selectedLanguagesId = new List<int>();
            var selectedOfficeTypesId = new List<int>();

            if(preferences != null)
            {
                selectedLanguagesId = await _mediLinkContext.PreferedLanguages.Where(pl => pl.PatientPreferenceId == preferences.Id).Select(pl => pl.LanguageId).ToListAsync();
                selectedOfficeTypesId = await _mediLinkContext.PatientOfficeTypes.Where(po => po.PatientPreferenceId == preferences.Id).Select(pl => pl.OfficeTypeId).ToListAsync();
                viewModel.city = preferences.location;
                viewModel.minimumRating = preferences.rating??0;
            }

            List<int> practitionerIds = new List<int>();
            List<int> languagePractitionerIds = new List<int>();
            List<int> officePractitionerIds = new List<int>();

            if (viewModel.selectedLanguageIds.Count > 0)
            {
                languagePractitionerIds.AddRange(await _mediLinkContext.PractitionerSpokenLanguages.Where(psl => viewModel.selectedLanguageIds.Contains(psl.LanguageId)).Select(psl => psl.PractitionerId).Distinct().ToListAsync());
            }

            List<int> addressIds = new List<int>();

            if (viewModel.selectedOfficeTypeIds.Count > 0)
            {
                if (viewModel.city != null)
                {
                    addressIds = await _mediLinkContext.OfficeAddresses.Where(oa => oa.City == viewModel.city && viewModel.selectedOfficeTypeIds.Contains(oa.OfficeTypeId)).Select(oa => oa.Id).Distinct().ToListAsync();
                }
                else
                {
                    addressIds = await _mediLinkContext.OfficeAddresses.Where(oa => viewModel.selectedOfficeTypeIds.Contains(oa.OfficeTypeId)).Select(oa => oa.Id).Distinct().ToListAsync();
                }
            }
            else
            {
                if (viewModel.city != null)
                {
                    addressIds = await _mediLinkContext.OfficeAddresses.Where(oa => oa.City == viewModel.city).Select(oa => oa.Id).Distinct().ToListAsync();
                }
                else
                {
                    addressIds = await _mediLinkContext.OfficeAddresses.Select(oa => oa.Id).Distinct().ToListAsync();
                }
            }

            if (addressIds.Count > 0)
            {
                officePractitionerIds = await _mediLinkContext.PractitionerAddresses.Where(pa => addressIds.Contains(pa.OfficeAddressesId)).Select(pa => pa.PractitionerId).Distinct().ToListAsync();
            }

            if (viewModel.selectedLanguageIds.Count > 0)
            {
                practitionerIds = officePractitionerIds.Intersect(languagePractitionerIds).ToList();
            }
            else
            {
                practitionerIds = officePractitionerIds;
            }


            if (practitionerIds.Count > 0)
            {
                if (viewModel.minimumRating == 0)
                {
                    viewModel.practitioners.AddRange(await _mediLinkContext.Practitioners.Where(p => practitionerIds.Contains(p.Id)).OrderBy(p => p.FirstName + p.LastName).ToListAsync());
                }
                else
                {
                    viewModel.practitioners.AddRange(await _mediLinkContext.Practitioners.Where(p => practitionerIds.Contains(p.Id) && p.rating >= viewModel.minimumRating).OrderBy(p => p.FirstName + p.LastName).ToListAsync());
                }
            }

            var languages = _mediLinkContext.Languages.Where(l => l.IsDeleted == false).OrderBy(l => l.LanguageName).ToList();
            var officeTypes = _mediLinkContext.OfficeTypes.Where(ot => ot.IsDeleted == false).OrderBy(ot => ot.OfficeTypeName).ToList();

            viewModel.languages = new MultiSelectList(languages, "Id", "LanguageName", selectedLanguagesId);
            viewModel.officeTypes = new MultiSelectList(officeTypes, "Id", "OfficeTypeName", selectedOfficeTypesId);
            viewModel.selectedLanguageIds = selectedLanguagesId;
            viewModel.selectedOfficeTypeIds = selectedOfficeTypesId;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SearchPractitioner(PractitionerSearchViewModel viewModel)
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string userName = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                userName = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
            }

            ViewData["userName"] = userName;

            List<int> practitionerIds = new List<int>();
            List<int> languagePractitionerIds = new List<int>();
            List<int> officePractitionerIds = new List<int>();

            if (viewModel.selectedLanguageIds.Count > 0)
            {
                languagePractitionerIds.AddRange(await _mediLinkContext.PractitionerSpokenLanguages.Where(psl => viewModel.selectedLanguageIds.Contains(psl.LanguageId)).Select(psl => psl.PractitionerId).Distinct().ToListAsync());
            }

            List<int> addressIds = new List<int>();

            if (viewModel.selectedOfficeTypeIds.Count > 0)
            {
                if (viewModel.city != null)
                {
                    addressIds = await _mediLinkContext.OfficeAddresses.Where(oa => oa.City == viewModel.city && viewModel.selectedOfficeTypeIds.Contains(oa.OfficeTypeId)).Select(oa => oa.Id).Distinct().ToListAsync();
                }
                else
                {
                    addressIds = await _mediLinkContext.OfficeAddresses.Where(oa => viewModel.selectedOfficeTypeIds.Contains(oa.OfficeTypeId)).Select(oa => oa.Id).Distinct().ToListAsync();
                }
            }
            else
            {
                if (viewModel.city != null)
                {
                    addressIds = await _mediLinkContext.OfficeAddresses.Where(oa => oa.City == viewModel.city).Select(oa => oa.Id).Distinct().ToListAsync();
                }
                else
                {
                    addressIds = await _mediLinkContext.OfficeAddresses.Select(oa => oa.Id).Distinct().ToListAsync();
                }
            }

            if (addressIds.Count > 0)
            {
                officePractitionerIds = await _mediLinkContext.PractitionerAddresses.Where(pa => addressIds.Contains(pa.OfficeAddressesId)).Select(pa => pa.PractitionerId).Distinct().ToListAsync();
            }

            if(viewModel.selectedLanguageIds.Count > 0)
            {
                practitionerIds = officePractitionerIds.Intersect(languagePractitionerIds).ToList();
            }
            else
            {
                practitionerIds = officePractitionerIds;
            }
            

            if (practitionerIds.Count > 0)
            {
                if (viewModel.minimumRating == 0)
                {
                    viewModel.practitioners.AddRange(await _mediLinkContext.Practitioners.Where(p => practitionerIds.Contains(p.Id)).OrderBy(p => p.FirstName + p.LastName).ToListAsync());
                }
                else
                {
                    viewModel.practitioners.AddRange(await _mediLinkContext.Practitioners.Where(p => practitionerIds.Contains(p.Id) && p.rating >= viewModel.minimumRating).OrderBy(p => p.FirstName + p.LastName).ToListAsync());
                }
            }

            var languages = _mediLinkContext.Languages.Where(l => l.IsDeleted == false).OrderBy(l => l.LanguageName).ToList();
            var officeTypes = _mediLinkContext.OfficeTypes.Where(ot => ot.IsDeleted == false).OrderBy(ot => ot.OfficeTypeName).ToList();

            viewModel.languages = new MultiSelectList(languages, "Id", "LanguageName", viewModel.selectedLanguageIds);
            viewModel.officeTypes = new MultiSelectList(officeTypes, "Id", "OfficeTypeName", viewModel.selectedOfficeTypeIds);

            return View(viewModel);
        }

        public async Task<IActionResult> PractitionerDetails(string email)
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string userName = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                userName = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
            }

            ViewData["userName"] = userName;

            Practitioner practitioner = await _userService.GetPractitionerByEmail(email);

            List<PractitionerOfficeAddress> practitionerOfficeAddresses = await _mediLinkContext.PractitionerAddresses
                .Where(pa => pa.PractitionerId == practitioner.Id)
                .Include(pa => pa.OfficeAddresses)
                .ThenInclude(oa => oa.OfficeType)
                .ToListAsync();

            List<OfficeAddress> officeAddresses = new List<OfficeAddress>();

            foreach (var officeAddress in practitionerOfficeAddresses)
            {
                officeAddresses.Add(officeAddress.OfficeAddresses);
            }

            List<PractitionerSpokenLanguages> practitionerSpokenLanguages = await _mediLinkContext.PractitionerSpokenLanguages
                .Where(psl => psl.PractitionerId == practitioner.Id).Include(psl => psl.Language).ToListAsync();

            PractitionerType practitionerType = await _mediLinkContext.PractitionerTypes
                .Where(pt => pt.Id == practitioner.PractitionerTypeId)
                .FirstOrDefaultAsync();

            string isAcceptingNewPatients = "";
            if (practitioner.IsAcceptingNewPatients = true)
            {
                isAcceptingNewPatients = "Currently accepting new patients";
            }
            else
            {
                isAcceptingNewPatients = "Currently not accepting new patients";
            }

            string lastAcceptedPatientDateString = practitioner.lastPatientAcceptedDate.ToString();

            if (!string.IsNullOrEmpty(lastAcceptedPatientDateString) || !string.IsNullOrWhiteSpace(lastAcceptedPatientDateString))
            {
                lastAcceptedPatientDateString = practitioner.lastPatientAcceptedDate.ToString().Substring(0, 11);
            }
            else
            {
                lastAcceptedPatientDateString = "No patients accepted yet";
            }

            PractitionerViewModel practitionerViewModel = new PractitionerViewModel()
            {
                Practitioner = practitioner,
                OfficeAddresses = officeAddresses,
                PractitionerSpokenLanguages = practitionerSpokenLanguages,
                PractitionerType = practitionerType,
                IsAcceptingNewPatients = isAcceptingNewPatients,
                LastAcceptedPatientDate = lastAcceptedPatientDateString
            };

            return View(practitionerViewModel);

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
                            HistoricalWaitTimeMin = t.HistoricalWaitTimeMin, // To Be Removed, no longer needed, I will need to test first - DC
                            HistoricalWaitTimeMax = t.HistoricalWaitTimeMax, // To Be Removed, no longer needed, I will need to test first - DC
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
