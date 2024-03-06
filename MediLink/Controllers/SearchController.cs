using MediLink.Entities;
using MediLink.Models;
using MediLink.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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

            if(selectedLanguagesId.Count > 0)
            {
                practitionerIds.AddRange(await _mediLinkContext.PractitionerSpokenLanguages.Where(psl => selectedLanguagesId.Contains(psl.LanguageId)).Select(psl => psl.PractitionerId).Distinct().ToListAsync());
            }
            else
            {
                practitionerIds.AddRange(await _mediLinkContext.PractitionerSpokenLanguages.Select(psl => psl.PractitionerId).Distinct().ToListAsync());
            }

            List<int> addressIds = new List<int>();

            if(selectedOfficeTypesId.Count > 0)
            {
                if (viewModel.city != null)
                {
                    addressIds = await _mediLinkContext.OfficeAddresses.Where(oa => oa.City == viewModel.city && selectedOfficeTypesId.Contains(oa.OfficeTypeId)).Select(oa => oa.Id).Distinct().ToListAsync();
                }
                else
                {
                    addressIds = await _mediLinkContext.OfficeAddresses.Where(oa => selectedOfficeTypesId.Contains(oa.OfficeTypeId)).Select(oa => oa.OfficeTypeId).Distinct().ToListAsync();
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
                    addressIds = await _mediLinkContext.OfficeAddresses.Select(oa => oa.OfficeTypeId).Distinct().ToListAsync();
                }
            }


            for (int i = 0; i < Math.Ceiling((double)addressIds.Count / 2100); i++)
            {
                if (Math.Floor((double)addressIds.Count / 2100) > 0)
                {
                    var currentIds = addressIds.Take(2100).ToList();
                    practitionerIds.AddRange(await _mediLinkContext.PractitionerAddresses.Where(oa => currentIds.Contains(oa.OfficeAddressesId)).Select(oa => oa.PractitionerId).ToListAsync());
                }
                else
                {
                    practitionerIds.AddRange(await _mediLinkContext.PractitionerAddresses.Where(oa => addressIds.Contains(oa.OfficeAddressesId)).Select(oa => oa.PractitionerId).ToListAsync());
                }

                if (addressIds.Count > 2100)
                {
                    addressIds.RemoveRange(0, 2100);
                }
            }

            if (practitionerIds.Count > 0)
            {
                practitionerIds = practitionerIds.Distinct().ToList();

                for (int i = 0; i < Math.Ceiling((double)practitionerIds.Count / 2100); i++)
                {
                    if (Math.Floor((double)practitionerIds.Count / 2100) > 0)
                    {
                        var currentIds = practitionerIds.Take(2100).ToList();
                        if (viewModel.minimumRating == 0)
                        {
                            viewModel.practitioners.AddRange(await _mediLinkContext.Practitioners.Where(p => currentIds.Contains(p.Id)).ToListAsync());
                        }
                        else
                        {
                            viewModel.practitioners.AddRange(await _mediLinkContext.Practitioners.Where(p => currentIds.Contains(p.Id) && p.rating >= viewModel.minimumRating).ToListAsync());
                        }
                    }
                    else
                    {
                        if (viewModel.minimumRating == 0)
                        {
                            viewModel.practitioners.AddRange(await _mediLinkContext.Practitioners.Where(p => practitionerIds.Contains(p.Id)).ToListAsync());
                        }
                        else
                        {
                            viewModel.practitioners.AddRange(await _mediLinkContext.Practitioners.Where(p => practitionerIds.Contains(p.Id) && p.rating >= viewModel.minimumRating).ToListAsync());
                        }
                    }

                    if (practitionerIds.Count > 2100)
                    {
                        practitionerIds.RemoveRange(0, 2100);
                    }
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

            if (viewModel.selectedLanguageIds.Count > 0)
            {
                practitionerIds.AddRange(await _mediLinkContext.PractitionerSpokenLanguages.Where(psl => viewModel.selectedLanguageIds.Contains(psl.LanguageId)).Select(psl => psl.PractitionerId).Distinct().ToListAsync());
            }
            else
            {
                practitionerIds.AddRange(await _mediLinkContext.PractitionerSpokenLanguages.Select(psl => psl.PractitionerId).Distinct().ToListAsync());
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
                    addressIds = await _mediLinkContext.OfficeAddresses.Where(oa => viewModel.selectedOfficeTypeIds.Contains(oa.OfficeTypeId)).Select(oa => oa.OfficeTypeId).Distinct().ToListAsync();
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
                    addressIds = await _mediLinkContext.OfficeAddresses.Select(oa => oa.OfficeTypeId).Distinct().ToListAsync();
                }
            }

            if (practitionerIds.Count > 0)
            {
                practitionerIds = practitionerIds.Distinct().ToList();

                if (Math.Floor((double)practitionerIds.Count / 2100) > 0)
                {
                    var currentIds = practitionerIds.Take(2100).ToList();
                    if (viewModel.minimumRating == 0)
                    {
                        viewModel.practitioners.AddRange(await _mediLinkContext.Practitioners.Where(p => currentIds.Contains(p.Id)).ToListAsync());
                    }
                    else
                    {
                        viewModel.practitioners.AddRange(await _mediLinkContext.Practitioners.Where(p => currentIds.Contains(p.Id) && p.rating >= viewModel.minimumRating).ToListAsync());
                    }
                }
                else
                {
                    if (viewModel.minimumRating == 0)
                    {
                        viewModel.practitioners.AddRange(await _mediLinkContext.Practitioners.Where(p => practitionerIds.Contains(p.Id)).ToListAsync());
                    }
                    else
                    {
                        viewModel.practitioners.AddRange(await _mediLinkContext.Practitioners.Where(p => practitionerIds.Contains(p.Id) && p.rating >= viewModel.minimumRating).ToListAsync());
                    }
                }
            }

            var languages = _mediLinkContext.Languages.Where(l => l.IsDeleted == false).OrderBy(l => l.LanguageName).ToList();
            var officeTypes = _mediLinkContext.OfficeTypes.Where(ot => ot.IsDeleted == false).OrderBy(ot => ot.OfficeTypeName).ToList();

            viewModel.languages = new MultiSelectList(languages, "Id", "LanguageName", viewModel.selectedLanguageIds);
            viewModel.officeTypes = new MultiSelectList(officeTypes, "Id", "OfficeTypeName", viewModel.selectedOfficeTypeIds);

            return View(viewModel);
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
