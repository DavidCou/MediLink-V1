using MediLink.Entities;
using MediLink.Models;
using MediLink.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace MediLink.Controllers
{
    public class PatientController : Controller
    {
        public PatientController(MediLinkDbContext mediLinkContext, IUserService userService)
        {
            _mediLinkContext = mediLinkContext;
            _userService = userService;
        }

        public async Task<IActionResult> PatientHomePage()
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

            PatientDetail patientDetail = await _mediLinkContext.PatientDetails
                .Where(pd => pd.Id == patient.PatientDetailsId)
                .FirstOrDefaultAsync();

            PatientAddress patientAddress = await _mediLinkContext.PatientAddress
                .Where(pa => pa.Id == patientDetail.PatientAddressesId)
                .FirstOrDefaultAsync();

            List<PatientSpokenLanguage> patientSpokenLanguages = await _mediLinkContext.PatientSpokenLanguages.Where(psl => psl.PatientDetailsId == patientDetail.Id).Include(psl => psl.Language).ToListAsync();

            PatientViewModel patientViewModel = new PatientViewModel()
            {
                Email = patient.Email,
                SpokenLanguages = patientSpokenLanguages,
                PatientDetail = patientDetail,
                PatientAddress = patientAddress
            };

            return View(patientViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> UpdatePatientDetails()
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

            PatientDetail patientDetail = await _mediLinkContext.PatientDetails
                .Where(pd => pd.Id == patient.PatientDetailsId)
                .FirstOrDefaultAsync();

            PatientAddress patientAddress = await _mediLinkContext.PatientAddress
                .Where(pa => pa.Id == patientDetail.PatientAddressesId)
                .FirstOrDefaultAsync();

            List<Languages> languages = await _mediLinkContext.Languages.ToListAsync();

            PatientUpdateViewModel patientUpdateViewModel = new PatientUpdateViewModel()
            {
                Languages = languages,
                Email = patient.Email,
                FirstName = patientDetail.FirstName,
                LastName = patientDetail.LastName,
                gender = patientDetail.gender,
                PhoneNumber = patientDetail.PhoneNumber,
                DoB = patientDetail.DoB,
                City = patientAddress.City,
                Province = patientAddress.Province,
                country = patientAddress.country,
                PostalCode = patientAddress.PostalCode,
                StreetAddress = patientAddress.StreetAddress
            };

            return View(patientUpdateViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePatientDetails(PatientUpdateViewModel patientUpdateViewModel)
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string userName = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                userName = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
            }

            List<string> errorMessage = new List<string>();
            string phonePattern = @"^\d{3}-\d{3}-\d{4}$";
            Regex regex = new Regex(phonePattern);

            DateTime currentDate = DateTime.Now;
            DateTime? dob = null;
            if (patientUpdateViewModel.DoB.HasValue)
            {
                dob = new DateTime(patientUpdateViewModel.DoB.Value.Year, patientUpdateViewModel.DoB.Value.Month, patientUpdateViewModel.DoB.Value.Day);
            }

            DateTime currentDateMinusTwoDays = currentDate.AddDays(-2);

            if (string.IsNullOrEmpty(patientUpdateViewModel.FirstName) || string.IsNullOrWhiteSpace(patientUpdateViewModel.FirstName))
            {
                errorMessage.Add("Your first name cannont be blank");
            }

            if (string.IsNullOrEmpty(patientUpdateViewModel.LastName) || string.IsNullOrWhiteSpace(patientUpdateViewModel.LastName))
            {
                errorMessage.Add("Your last name cannot be blank");
            }

            if (string.IsNullOrEmpty(patientUpdateViewModel.Email) || string.IsNullOrWhiteSpace(patientUpdateViewModel.Email))
            {
                errorMessage.Add("Your email cannot be blank");
            }

            if (!string.IsNullOrEmpty(patientUpdateViewModel.PhoneNumber) && !string.IsNullOrWhiteSpace(patientUpdateViewModel.PhoneNumber))
            {
                if (!regex.IsMatch(patientUpdateViewModel.PhoneNumber))
                {
                    errorMessage.Add("Phone number not valid, please use the following input format: 222-222-2222");
                }
            }

            if (dob >= currentDateMinusTwoDays)
            {
                errorMessage.Add("Birthday is invalid, your birthday must be at least two days in the past");
            }

            if (errorMessage.Count == 0)
            {
                Patient patient = await _userService.GetUserByEmail(userName);

                PatientDetail patientDetails = await _mediLinkContext.PatientDetails
                    .Where(pd => pd.Id == patient.Id)
                    .FirstOrDefaultAsync();

                PatientAddress patientAddress = await _mediLinkContext.PatientAddress
                    .Where(pa => pa.Id == patient.Id)
                    .FirstOrDefaultAsync();

                if (patient != null && patientDetails != null && patientAddress != null)
                {
                    if (patientUpdateViewModel.DoB.ToString().Contains("0001-01-01"))
                    {
                        dob = null;
                    }

                    //TODO - Fix patientUpdateViewModel.SpokenLanguageIds it is always null for some reason and causes errors when reloading the page to show error messages
                    if (patientUpdateViewModel.SpokenLanguageIds != null)
                    {
                        List<PatientSpokenLanguage>? previousSpokenLanguages = await _mediLinkContext.PatientSpokenLanguages
                        .Where(psl => psl.PatientDetailsId == patientDetails.Id).ToListAsync();
                        foreach (var language in previousSpokenLanguages)
                        {
                            _mediLinkContext.PatientSpokenLanguages.Remove(language);
                            await _mediLinkContext.SaveChangesAsync();
                        }

                        foreach (var languageId in patientUpdateViewModel.SpokenLanguageIds)
                        {
                            var currentSpokenlanguage = new PatientSpokenLanguage { LanguageId = languageId, PatientDetailsId = patientDetails.Id };
                            _mediLinkContext.PatientSpokenLanguages.Add(currentSpokenlanguage);
                            await _mediLinkContext.SaveChangesAsync();
                        }
                    }

                    patient.Email = patientUpdateViewModel.Email;
                    _mediLinkContext.Update(patient);
                    await _mediLinkContext.SaveChangesAsync();

                    patientDetails.FirstName = patientUpdateViewModel.FirstName;
                    patientDetails.LastName = patientUpdateViewModel.LastName;
                    patientDetails.gender = patientUpdateViewModel.gender;
                    patientDetails.PhoneNumber = patientUpdateViewModel.PhoneNumber;
                    patientDetails.DoB = dob;
                    _mediLinkContext.Update(patientDetails);
                    await _mediLinkContext.SaveChangesAsync();

                    patientAddress.StreetAddress = patientUpdateViewModel.StreetAddress;
                    patientAddress.City = patientUpdateViewModel.City;
                    patientAddress.PostalCode = patientUpdateViewModel.PostalCode;
                    patientAddress.Province = patientUpdateViewModel.Province;
                    patientAddress.country = patientUpdateViewModel.country;
                    _mediLinkContext.Update(patientAddress);
                    await _mediLinkContext.SaveChangesAsync();
                }
                else
                {
                    ViewBag.MyErrorList = errorMessage;
                    ViewData["UpdateErrorMessage"] = errorMessage;
                    return View(patientUpdateViewModel);
                }

            }
            else
            {
                ViewBag.MyErrorList = errorMessage;
                ViewData["UpdateErrorMessage"] = errorMessage;
                return View(patientUpdateViewModel);
            }

            ViewBag.Success = "Your profile has been updated.";
            ViewData["UpdateSuccess"] = "Your profile has been updated.";

            return RedirectToAction("PatientHomePage");
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

            ViewData["userName"] = userName;

            Patient patient = await _userService.GetUserByEmail(userName);

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
            var officeTypes = _mediLinkContext.OfficeTypes.Where(ot => ot.IsDeleted == false).OrderBy(ot => ot.OfficeTypeName).ToList();
            PreferencesViewModel viewModel = new PreferencesViewModel();

            viewModel.preferences = preferences;
            
            viewModel.languages = new MultiSelectList(languages, "Id", "LanguageName", preferences.PreferedLanguages.Select(pl => pl.LanguageId));

            viewModel.officeTypes = officeTypes;

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

            ViewData["userName"] = userName;

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
            var officeTypes = _mediLinkContext.OfficeTypes.Where(ot => ot.IsDeleted == false).OrderBy(ot => ot.OfficeTypeName).ToList();

            viewModel.languages = new MultiSelectList(languages, "Id", "LanguageName", viewModel.selectedLanguageIds);

            viewModel.officeTypes = officeTypes;

            ViewData["Message"] = "Your preferences have been updated";

            return View(viewModel);
        }

        private MediLinkDbContext _mediLinkContext;
        private IUserService _userService;
    }
}
