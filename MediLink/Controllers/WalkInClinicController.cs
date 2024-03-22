using MediLink.Entities;
using MediLink.Models;
using MediLink.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace MediLink.Controllers
{
    public class WalkInClinicController : Controller
    {
        public WalkInClinicController(MediLinkDbContext mediLinkContext, IUserService userService)
        {
            _mediLinkContext = mediLinkContext;
            _userService = userService;
        }

        public async Task<IActionResult> WalkInClinicHomePage()
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string userName = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                userName = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
            }

            ViewData["userName"] = userName;

            WalkInClinic walkInClinic = await _userService.GetWalkInClinicByEmail(userName);

            OfficeAddress officeAddress = await _mediLinkContext.OfficeAddresses
                .Where(of => of.Id == walkInClinic.OfficeAddressId)
                .FirstOrDefaultAsync();

            List<WalkInPractitionerSpokenLanguages> walkInPractitionerSpokenLanguages = await _mediLinkContext.WalkInPractitionerSpokenLanguages
                .Where(wpsl => wpsl.WalkInPractitionerId == walkInClinic.Id)
                .Include(psl => psl.Language)
                .ToListAsync();


            WalkClinicViewModel walkClinicViewModel = new WalkClinicViewModel()
            {
                WalkInClinic = walkInClinic,
                WalkInPractitionerSpokenLanguages = walkInPractitionerSpokenLanguages,
                OfficeAddress = officeAddress
            };

            if (TempData["WalkInSuccess"] != null)
            {
                ViewBag.WalkInSuccess = TempData["WalkInSuccess"];
            }

            return View(walkClinicViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateWalkInClinicDetails()
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string userName = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                userName = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
            }

            ViewData["userName"] = userName;

            WalkInClinic walkInClinic = await _userService.GetWalkInClinicByEmail(userName);

            OfficeAddress officeAddress = await _mediLinkContext.OfficeAddresses
                .Where(of => of.Id == walkInClinic.OfficeAddressId)
                .FirstOrDefaultAsync();

            List<Languages> languages = await _mediLinkContext.Languages.ToListAsync();

            List <WalkInPractitionerSpokenLanguages> walkInPractitionerSpokenLanguages = await _mediLinkContext.WalkInPractitionerSpokenLanguages
                .Where(wpsl => wpsl.WalkInPractitionerId == walkInClinic.Id)
                .ToListAsync();

            List<int> currentSpokenLanguageIds = new List<int>();

            foreach (var language in walkInPractitionerSpokenLanguages) 
            {
                currentSpokenLanguageIds.Add(language.LanguageId);
            }

            WalkClinicUpdateViewModel walkClinicUpdateViewModel = new WalkClinicUpdateViewModel()
            {
                Languages = languages,
                CurrentSpokenLanguageIds = currentSpokenLanguageIds,
                PhoneNumber = walkInClinic.PhoneNumber,
                ClinicNotes = walkInClinic.ClinicNotes,
                Email = walkInClinic.Email,
                OfficeName = officeAddress.OfficeName,
                City = officeAddress.City,
                Province = officeAddress.Province,
                Country = officeAddress.country,
                PostalCode = officeAddress.PostalCode,
                StreetAddress = officeAddress.StreetAddress
            };

            return View(walkClinicUpdateViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateWalkInClinicDetails(WalkClinicUpdateViewModel walkClinicUpdateViewModel)
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

            if (string.IsNullOrEmpty(walkClinicUpdateViewModel.Email) || string.IsNullOrWhiteSpace(walkClinicUpdateViewModel.Email))
            {
                errorMessage.Add("Your email cannot be blank");
            }

            if (!string.IsNullOrEmpty(walkClinicUpdateViewModel.PhoneNumber) && !string.IsNullOrWhiteSpace(walkClinicUpdateViewModel.PhoneNumber))
            {
                if (!regex.IsMatch(walkClinicUpdateViewModel.PhoneNumber))
                {
                    errorMessage.Add("Phone number not valid, please use the following input format: 222-222-2222");
                }
            }
            else
            {
                errorMessage.Add("Phone number cannot be left blank");
            }

            if (string.IsNullOrEmpty(walkClinicUpdateViewModel.OfficeName) || string.IsNullOrWhiteSpace(walkClinicUpdateViewModel.OfficeName))
            {
                errorMessage.Add("Office name cannot be blank");
            }

            if (string.IsNullOrEmpty(walkClinicUpdateViewModel.StreetAddress) || string.IsNullOrWhiteSpace(walkClinicUpdateViewModel.StreetAddress))
            {
                errorMessage.Add("Street address cannot be blank");
            }

            if (string.IsNullOrEmpty(walkClinicUpdateViewModel.City) || string.IsNullOrWhiteSpace(walkClinicUpdateViewModel.City))
            {
                errorMessage.Add("City cannot be blank");
            }

            if (string.IsNullOrEmpty(walkClinicUpdateViewModel.PostalCode) || string.IsNullOrWhiteSpace(walkClinicUpdateViewModel.PostalCode))
            {
                errorMessage.Add("Postal code cannot be blank");
            }

            if (string.IsNullOrEmpty(walkClinicUpdateViewModel.Province) || string.IsNullOrWhiteSpace(walkClinicUpdateViewModel.Province))
            {
                errorMessage.Add("Province cannot be blank");
            }

            if (string.IsNullOrEmpty(walkClinicUpdateViewModel.Country) || string.IsNullOrWhiteSpace(walkClinicUpdateViewModel.Country))
            {
                errorMessage.Add("Country cannot be blank");
            }

            if (walkClinicUpdateViewModel.CurrentSpokenLanguageIds == null)
            {
                errorMessage.Add("There must be at least one selected spoken language");
            }

            WalkInClinic walkInClinic = await _userService.GetWalkInClinicByEmail(userName);

            List<WalkInPractitionerSpokenLanguages> currentSpokenLanguages = await _mediLinkContext.WalkInPractitionerSpokenLanguages
                .Where(wpsl => wpsl.WalkInPractitionerId == walkInClinic.Id)
                .Include(psl => psl.Language)
                .ToListAsync();

            if (errorMessage.Count == 0)
            {
                
                OfficeAddress officeAddress = await _mediLinkContext.OfficeAddresses
                .Where(of => of.Id == walkInClinic.OfficeAddressId)
                .FirstOrDefaultAsync();

                OfficeType officeType = await _mediLinkContext.OfficeTypes
                    .Where(ot => ot.OfficeTypeName == "Walk In Clinic")
                    .FirstOrDefaultAsync();

                if (walkClinicUpdateViewModel.CurrentSpokenLanguageIds != null)
                {
                    foreach (var language in currentSpokenLanguages)
                    {
                        _mediLinkContext.WalkInPractitionerSpokenLanguages.Remove(language);
                        await _mediLinkContext.SaveChangesAsync();
                    }

                    foreach (var languageId in walkClinicUpdateViewModel.CurrentSpokenLanguageIds)
                    {
                        var currentSpokenlanguage = new WalkInPractitionerSpokenLanguages { LanguageId = languageId, WalkInPractitionerId = walkInClinic.Id };
                        _mediLinkContext.WalkInPractitionerSpokenLanguages.Add(currentSpokenlanguage);
                        await _mediLinkContext.SaveChangesAsync();
                    }
                }

                walkInClinic.PhoneNumber = walkClinicUpdateViewModel.PhoneNumber;
                walkInClinic.Email = walkClinicUpdateViewModel.Email;
                walkInClinic.ClinicNotes = walkClinicUpdateViewModel.ClinicNotes;
                _mediLinkContext.Update(walkInClinic);
                await _mediLinkContext.SaveChangesAsync();

                officeAddress.OfficeName = walkClinicUpdateViewModel.OfficeName;
                officeAddress.StreetAddress = walkClinicUpdateViewModel.StreetAddress;
                officeAddress.City = walkClinicUpdateViewModel.City;
                officeAddress.PostalCode = walkClinicUpdateViewModel.PostalCode;
                officeAddress.Province = walkClinicUpdateViewModel.Province;
                officeAddress.country = walkClinicUpdateViewModel.Country;
                officeAddress.OfficeTypeId = officeType.Id;
                _mediLinkContext.Update(officeAddress);
                await _mediLinkContext.SaveChangesAsync();
            }
            else
            {
                List<Languages> languages = await _mediLinkContext.Languages.ToListAsync();
                List<int> currentSpokenLanguageIds = new List<int>();
               
                foreach(var currentLanguage  in currentSpokenLanguages) 
                { 
                    currentSpokenLanguageIds.Add(currentLanguage.LanguageId);
                }

                walkClinicUpdateViewModel.Languages = languages;
                walkClinicUpdateViewModel.CurrentSpokenLanguageIds = currentSpokenLanguageIds;

                ViewBag.MyErrorList = errorMessage;
                ViewData["UpdateErrorMessage"] = errorMessage;
                return View(walkClinicUpdateViewModel);
            }

            TempData["WalkInSuccess"] = "Your profile has been updated.";

            return RedirectToAction("WalkinClinicHomePage");
        }

        [HttpPost]
        public async Task<ActionResult> CheckPatientIn()
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string userName = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                userName = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
            }

            WalkInClinic walkInClinic = await _mediLinkContext.WalkInClinics
                .Where(wc => wc.Email == userName)
                .FirstOrDefaultAsync();

            string firstName = Request.Form["firstName"];
            string lastName = Request.Form["lastName"];

            WalkInClinicCheckedInPatient walkInClinicCheckedInPatient = new WalkInClinicCheckedInPatient()
            {
                PatientFirstName = firstName,
                PatientLastName = lastName,
                PatientCheckInTime = DateTime.Now,
                WalkInClinicId = walkInClinic.Id,
                WalkInClinic = walkInClinic
            };

            _mediLinkContext.WalkInClinicCheckedInPatients.Add(walkInClinicCheckedInPatient);
            await _mediLinkContext.SaveChangesAsync();

            TempData["WalkInSuccess"] = "Patient checked in successfully.";

            return RedirectToAction("WalkinClinicHomePage");
        }


        [HttpGet]
        public async Task<IActionResult> CheckPatientOut() 
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string userName = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                userName = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
            }

            ViewData["userName"] = userName;

            WalkInClinic walkInClinic = await _userService.GetWalkInClinicByEmail(userName);

            List<WalkInClinicCheckedInPatient> checkedInPatients = _mediLinkContext.WalkInClinicCheckedInPatients
                .Where(cp => cp.WalkInClinicId == walkInClinic.Id)
                .ToList();

            CheckOutViewModel checkOutViewModel = new CheckOutViewModel() 
            { 
                CheckedInPatients = checkedInPatients   
            };

            return View(checkOutViewModel);
        }

        [HttpPost("/WalkInClinic/CheckPatientOut/{id}")]
        public async Task<IActionResult> CheckPatientOut(int id)
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string userName = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                userName = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
            }

            ViewData["userName"] = userName;

            WalkInClinic walkInClinic = await _userService.GetWalkInClinicByEmail(userName);

            WalkInClinicCheckedInPatient walkInClinicCheckedInPatient = await _mediLinkContext.WalkInClinicCheckedInPatients
                .FindAsync(id);

            DateTime patientCheckInTime = walkInClinicCheckedInPatient.PatientCheckInTime;
            DateTime currentTime = DateTime.Now;
            TimeSpan waitTime = currentTime - patientCheckInTime;
            int hours = waitTime.Hours;
            int minutes = waitTime.Minutes;
            int seconds = waitTime.Seconds;

            WalkInClinicHistoricalWaitTimes historicalWaitTimes = new WalkInClinicHistoricalWaitTimes() 
            {
                PatientCheckInTime = patientCheckInTime,
                DayOfTheWeek = walkInClinicCheckedInPatient.PatientCheckInTime.DayOfWeek.ToString(),
                TimeOfDay = walkInClinicCheckedInPatient.PatientCheckInTime.TimeOfDay.ToString(),
                WaitTime = hours.ToString() + ":" + minutes.ToString() + ":" + seconds.ToString(),
                WalkInClinicId = walkInClinic.Id,
                WalkInClinic = walkInClinic
            };

            _mediLinkContext.WalkInClinicHistoricalWaitTimes.Add(historicalWaitTimes);
            await _mediLinkContext.SaveChangesAsync();

            TempData["WalkInSuccess"] = $"{walkInClinicCheckedInPatient.PatientFirstName}  {walkInClinicCheckedInPatient.PatientLastName} was checked out successfully.";

            return RedirectToAction("WalkinClinicHomePage");
        }

        private MediLinkDbContext _mediLinkContext;
        private IUserService _userService;
    }
}
