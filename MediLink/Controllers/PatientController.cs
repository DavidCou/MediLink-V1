using MediLink.Entities;
using MediLink.Models;
using MediLink.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MediLink.Controllers
{
    public class PatientController : Controller
    {
        public PatientController(MediLinkDbContext mediLinkContext, IUserService userService)
        {
            _mediLinkContext = mediLinkContext;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Preferences()
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string userName = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                userName = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
            }

            Patient patient = await _userService.GetUserByEmail(userName);

            if (patient == null)
            {
                TempData["mensajeResultLogin"] = "Please log in";
                return RedirectToAction("LoginPatient", "Management");
            }

            PatientPreference preferences = await _mediLinkContext.PatientPreferences.Where(p => p.Id == patient.PatientPreferencesId).FirstOrDefaultAsync();

            if(preferences == null)
            {
                preferences = new PatientPreference();
                preferences.Patients = patient;
                preferences.rating = 0;
                preferences.location = "";
                preferences.PreferedLanguages = new List<PreferedLanguage>();
                preferences.PatientOfficeType = new List<PatientOfficeType>();
                _mediLinkContext.PatientPreferences.Add(preferences);
                await _mediLinkContext.SaveChangesAsync();

                patient.PatientPreferencesId = preferences.Id;
                _mediLinkContext.Patients.Update(patient);
                await _mediLinkContext.SaveChangesAsync();
            }
            else
            {
                preferences.PatientOfficeType = await _mediLinkContext.PatientOfficeTypes.Where(po => po.PatientPreferenceId == preferences.Id).ToListAsync();
                preferences.PreferedLanguages = await _mediLinkContext.PreferedLanguages.Where(pl => pl.PatientPreferenceId == preferences.Id).ToListAsync();
            }

            var languages = _mediLinkContext.Languages.Where(l => l.IsDeleted == false).OrderBy(l => l.LanguageName).ToList();
            var officeTypes = _mediLinkContext.OfficeTypes.Where(ot => ot.IsDeleted == false).OrderBy(ot => ot.OfficeName).ToList();
            PreferencesViewModel viewModel = new PreferencesViewModel();

            viewModel.preferences = preferences;
            
            languages.Add(new Languages { Id = 0, LanguageName = "" });
            viewModel.languages = new MultiSelectList(languages, "Id", "LanguageName", preferences.PreferedLanguages.Select(pl => pl.LanguageId));

            officeTypes.Add(new OfficeType { Id = 0, OfficeName = "" });
            viewModel.officeTypes = new MultiSelectList(officeTypes, "Id", "OfficeName", preferences.PatientOfficeType.Select(po => po.OfficeTypeId));

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Preferences(PreferencesViewModel viewModel)
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string userName = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                userName = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
            }

            Patient patient = await _userService.GetUserByEmail(userName);

            if (viewModel.previousLanguages != viewModel.selectedLanguageIds)
            {
                var previousLanguages = await _mediLinkContext.PreferedLanguages.Where(pl => pl.PatientPreferenceId == viewModel.preferences.Id).ToListAsync();
                foreach(var language in previousLanguages)
                {
                    _mediLinkContext.PreferedLanguages.Remove(language);
                    await _mediLinkContext.SaveChangesAsync();
                }

                foreach (var languageId in viewModel.selectedLanguageIds)
                {
                    var language = new PreferedLanguage() { LanguageId = languageId, PatientPreferenceId = viewModel.preferences.Id };
                    _mediLinkContext.PreferedLanguages.Add(language);
                    await _mediLinkContext.SaveChangesAsync();
                }
            }

            if(viewModel.previousOfficeTypes != viewModel.selectedOfficeTypeIds)
            {
                var previousOfficeTypes = await _mediLinkContext.PatientOfficeTypes.Where(po => po.PatientPreferenceId == viewModel.preferences.Id).ToListAsync();
                foreach (var officeType in previousOfficeTypes)
                {
                    _mediLinkContext.PatientOfficeTypes.Remove(officeType);
                    await _mediLinkContext.SaveChangesAsync();
                }

                foreach(var officeTypeId in viewModel.selectedOfficeTypeIds)
                {
                    var officeType = new PatientOfficeType() { OfficeTypeId = officeTypeId, PatientPreferenceId = viewModel.preferences.Id };
                    _mediLinkContext.PatientOfficeTypes.Add(officeType);
                    await _mediLinkContext.SaveChangesAsync();
                }
            }

            if(viewModel.preferences.location == null)
            {
                viewModel.preferences.location = "";
            }

            _mediLinkContext.PatientPreferences.Update(viewModel.preferences);
            await _mediLinkContext.SaveChangesAsync();

            var languages = _mediLinkContext.Languages.Where(l => l.IsDeleted == false).OrderBy(l => l.LanguageName).ToList();
            var officeTypes = _mediLinkContext.OfficeTypes.Where(ot => ot.IsDeleted == false).OrderBy(ot => ot.OfficeName).ToList();

            languages.Add(new Languages { Id = 0, LanguageName = "" });
            viewModel.languages = new MultiSelectList(languages, "Id", "LanguageName", viewModel.selectedLanguageIds);

            officeTypes.Add(new OfficeType { Id = 0, OfficeName = "" });
            viewModel.officeTypes = new MultiSelectList(officeTypes, "Id", "OfficeName", viewModel.selectedOfficeTypeIds);

            ViewData["Message"] = "Your preferences have been updated";

            return View(viewModel);
        }

        private MediLinkDbContext _mediLinkContext;
        private IUserService _userService;
    }
}
