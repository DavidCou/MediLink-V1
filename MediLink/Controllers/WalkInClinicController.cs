using MediLink.Entities;
using MediLink.Models;
using MediLink.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
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

            List<WalkInClinicHours> walkInClinicHours = await _mediLinkContext.WalkInClinicHours
                .Where(wch => wch.WalkInClinicId == walkInClinic.Id)
                .ToListAsync();

            Dictionary<string, string> hoursDictionary = new Dictionary<string, string>();

            foreach (var hours in walkInClinicHours)
            {
                if (hours.OpeningTime == "Closed")
                {
                    hoursDictionary[hours.DayOfTheWeek] = "Closed";
                }
                else
                {
                    hoursDictionary[hours.DayOfTheWeek] = hours.OpeningTime + " - " + hours.ClosingTime;
                }
            }

            WalkClinicViewModel walkClinicViewModel = new WalkClinicViewModel()
            {
                WalkInClinic = walkInClinic,
                WalkInPractitionerSpokenLanguages = walkInPractitionerSpokenLanguages,
                OfficeAddress = officeAddress,
                WalkInClinicHours = new WalkInClinicHoursInfo()
                {
                    MondayHours = hoursDictionary.GetValueOrDefault("Monday", ""),
                    TuesdayHours = hoursDictionary.GetValueOrDefault("Tuesday", ""),
                    WednesdayHours = hoursDictionary.GetValueOrDefault("Wednesday", ""),
                    ThursdayHours = hoursDictionary.GetValueOrDefault("Thursday", ""),
                    FridayHours = hoursDictionary.GetValueOrDefault("Friday", ""),
                    SaturdayHours = hoursDictionary.GetValueOrDefault("Saturday", ""),
                    SundayHours = hoursDictionary.GetValueOrDefault("Sunday", "")
                }
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

        [HttpPost()]
        public async Task<IActionResult> CheckPatientOut(int id)
        {
            WalkInClinicCheckedInPatient walkInClinicCheckedInPatient = await _mediLinkContext.WalkInClinicCheckedInPatients
                .FindAsync(id);
  
            DateTime patientCheckInTime = walkInClinicCheckedInPatient.PatientCheckInTime;
            DateTime currentTime = DateTime.Now;
            TimeSpan waitTime = currentTime - patientCheckInTime;


            WalkInClinicHistoricalWaitTimes historicalWaitTimes = new WalkInClinicHistoricalWaitTimes()
            {
                PatientCheckInTime = patientCheckInTime,
                DayOfTheWeek = walkInClinicCheckedInPatient.PatientCheckInTime.DayOfWeek.ToString(),
                WaitTimeInSeconds = (int)waitTime.TotalSeconds,
                WalkInClinicId = walkInClinicCheckedInPatient.WalkInClinicId
            };

            _mediLinkContext.WalkInClinicHistoricalWaitTimes.Add(historicalWaitTimes);
            await _mediLinkContext.SaveChangesAsync();

            _mediLinkContext.WalkInClinicCheckedInPatients.Remove(walkInClinicCheckedInPatient);
            await _mediLinkContext.SaveChangesAsync();

            TempData["WalkInSuccess"] = $"{walkInClinicCheckedInPatient.PatientFirstName}  {walkInClinicCheckedInPatient.PatientLastName} was checked out successfully.";

            return RedirectToAction("WalkinClinicHomePage");
        }

        [HttpGet]
        public async Task<IActionResult> AddEditOperatingHours()
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string userName = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                userName = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
            }

            ViewData["userName"] = userName;

            //WalkInClinic walkInClinic = await _userService.GetWalkInClinicByEmail(userName);

            WalkInHoursViewModel walkInHoursViewModel = new WalkInHoursViewModel();

            return View(walkInHoursViewModel);

            //Dictionary<string, string> operatingHours = new Dictionary<string, string>();

            //List<WalkInClinicHours> walkInClinicHours = _mediLinkContext.WalkInClinicHours
            //    .Where(wch => wch.WalkInClinicId == walkInClinic.Id)
            //    .ToList();

            //if (walkInClinicHours != null)
            //{
            //    foreach (WalkInClinicHours walkInClinichour in walkInClinicHours)
            //    {
            //        if (walkInClinichour.DayOfTheWeek == "Monday")
            //        {
            //            operatingHours.Add("MondayOpening", walkInClinichour.OpeningTime);
            //            operatingHours.Add("MondayClosing", walkInClinichour.ClosingTime);
            //        }
            //        else if (walkInClinichour.DayOfTheWeek == "Tuesday")
            //        {
            //            operatingHours.Add("TuesdayOpening", walkInClinichour.OpeningTime);
            //            operatingHours.Add("TuesdayClosing", walkInClinichour.ClosingTime);
            //        }
            //        else if (walkInClinichour.DayOfTheWeek == "Wednesday")
            //        {
            //            operatingHours.Add("TuesdayOpening", walkInClinichour.OpeningTime);
            //            operatingHours.Add("TuesdayClosing", walkInClinichour.ClosingTime);
            //        }
            //        else if (walkInClinichour.DayOfTheWeek == "Thursday")
            //        {
            //            operatingHours.Add("TuesdayOpening", walkInClinichour.OpeningTime);
            //            operatingHours.Add("TuesdayClosing", walkInClinichour.ClosingTime);
            //        }
            //        else if (walkInClinichour.DayOfTheWeek == "Friday")
            //        {
            //            operatingHours.Add("TuesdayOpening", walkInClinichour.OpeningTime);
            //            operatingHours.Add("TuesdayClosing", walkInClinichour.ClosingTime);
            //        }
            //        else if (walkInClinichour.DayOfTheWeek == "Saturday")
            //        {
            //            operatingHours.Add("TuesdayOpening", walkInClinichour.OpeningTime);
            //            operatingHours.Add("TuesdayClosing", walkInClinichour.ClosingTime);
            //        }
            //        else if (walkInClinichour.DayOfTheWeek == "Sunday")
            //        {
            //            operatingHours.Add("TuesdayOpening", walkInClinichour.OpeningTime);
            //            operatingHours.Add("TuesdayClosing", walkInClinichour.ClosingTime);
            //        }
            //    }
            //}
        }

        [HttpPost]
        public async Task<IActionResult> AddEditOperatingHours(WalkInHoursViewModel walkInHoursViewModel)
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string userName = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                userName = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
            }

            ViewData["userName"] = userName;

            List<string> errorMessage = new List<string>();
            string timePattern = @"^\d{2}:\d{2}$";
            Regex regex = new Regex(timePattern);
            bool isBlank = false;

            Dictionary<string, string> operatingHours = new Dictionary<string, string>
            {
                { "opening time on Mondays", walkInHoursViewModel.MondayOpeningTime },
                { "closing time on Mondays", walkInHoursViewModel.MondayClosingTime },
                { "opening time on Tuesdays", walkInHoursViewModel.TuesdayOpeningTime },
                { "closing time on Tuesdays", walkInHoursViewModel.TuesdayClosingTime },
                { "opening time on Wednesdays", walkInHoursViewModel.WednesdayOpeningTime },
                { "closing time on Wednesdays", walkInHoursViewModel.WednesdayClosingTime },
                { "opening time on Thursdays", walkInHoursViewModel.ThursdayOpeningTime },
                { "closing time on Thursdays", walkInHoursViewModel.ThursdayClosingTime },
                { "opening time on Fridays", walkInHoursViewModel.FridayOpeningTime },
                { "closing time on Fridays", walkInHoursViewModel.FridayClosingTime },
                { "opening time on Saturdays", walkInHoursViewModel.SaturdayOpeningTime },
                { "closing time on Saturdays", walkInHoursViewModel.SaturdayClosingTime },
                { "opening time on Sundays", walkInHoursViewModel.SundayOpeningTime },
                { "closing time on Sundays", walkInHoursViewModel.SundayClosingTime },
            };

            foreach (var operatingHour in operatingHours)
            {
                if (string.IsNullOrEmpty(operatingHour.Value) || string.IsNullOrWhiteSpace(operatingHour.Value))
                {
                    errorMessage.Add($"The {operatingHour.Key} cannot be blank.");
                }
                else
                {
                    if (!regex.IsMatch(operatingHour.Value) && operatingHour.Value != "Closed")
                    {
                        errorMessage.Add($"The {operatingHour.Key} is invalid. Please use the following format: 00:00 or enter 'Closed' if your clinic is not open that day.");
                    }
                }
            }


            if (errorMessage.Count == 0) 
            {
                WalkInClinic walkInClinic = await _userService.GetWalkInClinicByEmail(userName);

                WalkInClinicHours mondayHours = new WalkInClinicHours();
                WalkInClinicHours tuesdayHours = new WalkInClinicHours();
                WalkInClinicHours wednesdayHours = new WalkInClinicHours();
                WalkInClinicHours thursdayHours = new WalkInClinicHours();
                WalkInClinicHours fridayHours = new WalkInClinicHours();
                WalkInClinicHours saturdayHours = new WalkInClinicHours();
                WalkInClinicHours sundayHours = new WalkInClinicHours();

                mondayHours.DayOfTheWeek = "Monday";
                mondayHours.OpeningTime = operatingHours["opening time on Mondays"] + "AM";
                mondayHours.ClosingTime = operatingHours["closing time on Mondays"] + "PM";
                mondayHours.WalkInClinicId = walkInClinic.Id;
                mondayHours.WalkInClinic = walkInClinic;
                _mediLinkContext.WalkInClinicHours.Add(mondayHours);
                await _mediLinkContext.SaveChangesAsync();

                tuesdayHours.DayOfTheWeek = "Tuesday";
                tuesdayHours.OpeningTime = operatingHours["opening time on Tuesdays"] + "AM";
                tuesdayHours.ClosingTime = operatingHours["closing time on Tuesdays"] + "PM";
                tuesdayHours.WalkInClinicId = walkInClinic.Id;
                tuesdayHours.WalkInClinic = walkInClinic;
                _mediLinkContext.WalkInClinicHours.Add(tuesdayHours);
                await _mediLinkContext.SaveChangesAsync();

                wednesdayHours.DayOfTheWeek = "Wednesday";
                wednesdayHours.OpeningTime = operatingHours["opening time on Wednesdays"] + "AM";
                wednesdayHours.ClosingTime = operatingHours["closing time on Wednesdays"] + "PM";
                wednesdayHours.WalkInClinicId = walkInClinic.Id;
                wednesdayHours.WalkInClinic = walkInClinic;
                _mediLinkContext.WalkInClinicHours.Add(wednesdayHours);
                await _mediLinkContext.SaveChangesAsync();

                thursdayHours.DayOfTheWeek = "Thursday";
                thursdayHours.OpeningTime = operatingHours["opening time on Thursdays"] + "AM";
                thursdayHours.ClosingTime = operatingHours["closing time on Thursdays"] + "PM";
                thursdayHours.WalkInClinicId = walkInClinic.Id;
                thursdayHours.WalkInClinic = walkInClinic;
                _mediLinkContext.WalkInClinicHours.Add(thursdayHours);
                await _mediLinkContext.SaveChangesAsync();

                fridayHours.DayOfTheWeek = "Friday";
                fridayHours.OpeningTime = operatingHours["opening time on Fridays"] + "AM";
                fridayHours.ClosingTime = operatingHours["closing time on Fridays"] + "PM";
                fridayHours.WalkInClinicId = walkInClinic.Id;
                fridayHours.WalkInClinic = walkInClinic;
                _mediLinkContext.WalkInClinicHours.Add(fridayHours);
                await _mediLinkContext.SaveChangesAsync();

                saturdayHours.DayOfTheWeek = "Saturday";
                saturdayHours.OpeningTime = operatingHours["opening time on Saturdays"] + "AM";
                saturdayHours.ClosingTime = operatingHours["closing time on Saturdays"] + "PM";
                saturdayHours.WalkInClinicId = walkInClinic.Id;
                saturdayHours.WalkInClinic = walkInClinic;
                _mediLinkContext.WalkInClinicHours.Add(saturdayHours);
                await _mediLinkContext.SaveChangesAsync();

                sundayHours.DayOfTheWeek = "Sunday";
                sundayHours.OpeningTime = operatingHours["opening time on Sundays"] + "AM";
                sundayHours.ClosingTime = operatingHours["closing time on Sundays"] + "PM";
                sundayHours.WalkInClinicId = walkInClinic.Id;
                sundayHours.WalkInClinic = walkInClinic;
                _mediLinkContext.WalkInClinicHours.Add(sundayHours);
                await _mediLinkContext.SaveChangesAsync();


                TempData["WalkInSuccess"] = " Your clinics operating hours were added successfully.";

                return RedirectToAction("WalkinClinicHomePage");
            }
            else 
            {
                ViewBag.MyErrorList = errorMessage;
                ViewData["UpdateErrorMessage"] = errorMessage;
                return View(walkInHoursViewModel);
            }
        }

        private MediLinkDbContext _mediLinkContext;
        private IUserService _userService;
    }
}
