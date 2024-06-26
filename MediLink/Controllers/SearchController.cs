﻿using MediLink.Entities;
using MediLink.Models;
using MediLink.Services;
using MediLink.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Model.Strings;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace MediLink.Controllers
{
    public class SearchController : Controller
    {
        public SearchController(MediLinkDbContext mediLinkContext, IUserService userService, IWebHostEnvironment webHostEnvironment)
        {
            _mediLinkContext = mediLinkContext;
            _userService = userService;
            _webHostEnvironment = webHostEnvironment;
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

            string mensajeResultPract = TempData["mensajeRequestPract"] as string;

            ViewData["mensajeResultPract"] = mensajeResultPract;

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
                    viewModel.practitioners.AddRange(await _mediLinkContext.Practitioners
                        .Where(p => practitionerIds.Contains(p.Id))
                        .Include(p => p.PractitionerReviews)
                        .OrderBy(p => p.FirstName + p.LastName)
                        .Select(p => new Practitioner()
                        {
                            Email = p.Email,
                            FirstName = p.FirstName,
                            LastName = p.LastName,
                            rating = p.PractitionerReviews.Average(p => p.Rating)
                        })
                        .ToListAsync());
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
                    viewModel.practitioners.AddRange(await _mediLinkContext.Practitioners
                        .Where(p => practitionerIds.Contains(p.Id))
                        .Include(p => p.PractitionerReviews)
                        .OrderBy(p => p.FirstName + p.LastName)
                        .Select(p => new Practitioner()
                        {
                            Email = p.Email,
                            FirstName = p.FirstName,
                            LastName = p.LastName,
                            rating = p.PractitionerReviews.Average(p => p.Rating)
                        })
                        .ToListAsync());
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

        [HttpGet("/PractitionerDetails/{email}")]
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

            Patient oPatient = await _userService.GetUserByEmail(userName);

            Practitioner practitioner = await _userService.GetPractitionerByEmail(email);

            List<PractitionerOfficeAddress> practitionerOfficeAddresses = await _mediLinkContext.PractitionerAddresses
                .Where(pa => pa.PractitionerId == practitioner.Id)
                .Include(pa => pa.OfficeAddresses)
                .ThenInclude(oa => oa.OfficeType)
                .ToListAsync();

            List<OfficeAddress> officeAddresses = new List<OfficeAddress>();

            List<OfficeInfo> listOfficeInfo = new List<OfficeInfo>();


            //foreach (var officeAddress in practitionerOfficeAddresses)
            //{
            //    officeAddresses.Add(officeAddress.OfficeAddresses);
            //}

            foreach (var officeAddress in practitionerOfficeAddresses)
            {
                OfficeInfo oOfficeInfo = new OfficeInfo();

                oOfficeInfo.Id = officeAddress.OfficeAddresses.Id;
                oOfficeInfo.OfficeName = officeAddress.OfficeAddresses.OfficeName;
                oOfficeInfo.Street = officeAddress.OfficeAddresses.StreetAddress;
                oOfficeInfo.City = officeAddress.OfficeAddresses.City;
                oOfficeInfo.Province = officeAddress.OfficeAddresses.Province;
                oOfficeInfo.PostalCode = officeAddress.OfficeAddresses.PostalCode;
                oOfficeInfo.country = officeAddress.OfficeAddresses.country;
                oOfficeInfo.OfficeTypeName = officeAddress.OfficeAddresses.OfficeType.OfficeTypeName;
                oOfficeInfo.fullAddress = officeAddress.OfficeAddresses.StreetAddress + " " + officeAddress.OfficeAddresses.City + " " + officeAddress.OfficeAddresses.Province;

                listOfficeInfo.Add(oOfficeInfo);
            }


            //get the list of new request by patient to get practitioner
            List<NewPatientRequest> ListNewPatientRequest = await _mediLinkContext.NewPatientRequests
                .Where(pat => pat.PatientId == oPatient.Id && pat.PractitionerId == practitioner.Id).ToListAsync();

            List<PractitionerSpokenLanguages> practitionerSpokenLanguages = await _mediLinkContext.PractitionerSpokenLanguages
                .Where(psl => psl.PractitionerId == practitioner.Id).Include(psl => psl.Language).ToListAsync();

            List<string> spokenLanguages = new List<string>();

            foreach (var practSpoken in practitionerSpokenLanguages)
            {
                spokenLanguages.Add(practSpoken.Language.LanguageName);
            }

            PractitionerType practitionerType = await _mediLinkContext.PractitionerTypes
                .Where(pt => pt.Id == practitioner.PractitionerTypeId)
                .FirstOrDefaultAsync();

            string isAcceptingNewPatients = "";
            if (practitioner.IsAcceptingNewPatients == true)
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

            // start Compare the two lists and remove duplicates
            // Check if the id is present in the list of new request

            bool isIdPresent = false;

            //verify ig the office id exist in the list of patient request
            foreach (OfficeInfo officeInfo in listOfficeInfo)
            {
                isIdPresent = ListNewPatientRequest.Any(obj => obj.officePractitionerId == officeInfo.Id);
                                
                if (isIdPresent)
                {
                    int practId = Convert.ToInt32(practitioner.Id);
                    int patId = Convert.ToInt32(oPatient.Id);
                    int offiID = officeInfo.Id;

                    // Find the record(s)  using a where statement
                    NewPatientRequest oNewPatientRequest = await _mediLinkContext.NewPatientRequests.Where(e => e.PractitionerId == practId && e.PatientId == patId && e.officePractitionerId == offiID).FirstAsync();

                    if (oNewPatientRequest != null)
                    {
                        // Update the entity
                        officeInfo.isRequested = true;
                        officeInfo.statusRequest = oNewPatientRequest.status;

                        var dateReq = oNewPatientRequest.DateRequest.ToShortDateString();
                        if (dateReq != null)
                        {
                            officeInfo.DateRequest = dateReq;
                        }
                        else
                        {
                            officeInfo.DateRequest = "";
                        }
                    }
                    else
                    {
                        // Handle the case where no entity is found with the specified ID
                        // For example: return an error message or display a user-friendly message
                    }
                  


                }
            }
            //end

            var reviews = await _mediLinkContext.PractitionerReviews.Where(pr => pr.PractitionerId == practitioner.Id).ToListAsync();
            if (reviews.Count > 0)
            {
                double rating = reviews.Average(r => r.Rating);
                practitioner.rating = rating;
            }
            else
            {
                practitioner.rating = 0;
            }

            SearchPractitionerRequest oSearchPractitionerRequest = new SearchPractitionerRequest()
            {
                Id = practitioner.Id,
                Email = practitioner.Email,
                FirstName = practitioner.FirstName,
                LastName = practitioner.LastName,
                PhoneNumber = practitioner.PhoneNumber,
                gender = practitioner.gender,
                OfficeAddresses = listOfficeInfo,
                Rating = (int)practitioner.rating,
                PractitionerSpokenLanguages = string.Join(", ", spokenLanguages),
                PractitionerType = practitionerType.Name,
                IsAcceptingNewPatients = isAcceptingNewPatients,
                LastAcceptedPatientDate = lastAcceptedPatientDateString,
                PatientId = oPatient.Id,
                

            };

            return Json(oSearchPractitionerRequest);

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
                            CurrentWaitTime = t.CurrentWaitTime.ToString(),
                            Id = t.Id

                        })
                        .ToListAsync();

            foreach(var walkClinic in walkClinics) 
            {
                List<WalkInClinicHours> walkInClinicHours = await _mediLinkContext.WalkInClinicHours
                .Where(wch => wch.WalkInClinicId == walkClinic.Id)
                .ToListAsync();

                DateTime thirtyMinutesAgo = DateTime.Now.AddMinutes(-30);

                string dayOfTheWeek = DateTime.Now.DayOfWeek.ToString();

                List<WalkInClinicHistoricalWaitTimes> walkInClinicHistoricalWaitTimes = await _mediLinkContext.WalkInClinicHistoricalWaitTimes
                    .Where(wch => wch.WalkInClinicId == walkClinic.Id
                            && wch.DayOfTheWeek == dayOfTheWeek
                            && wch.PatientCheckInTime >= thirtyMinutesAgo)
                    .ToListAsync();

                double currentWaitTime = 0;
                if (walkInClinicHistoricalWaitTimes.Any())
                {
                    walkClinic.CurrentWaitTime = ((int)walkInClinicHistoricalWaitTimes.Average(w => w.WaitTimeInSeconds) / 60).ToString() + " minutes";
                }
                else
                {
                    walkClinic.CurrentWaitTime = "No Current Data";
                }
            }

            ViewBag.WalkClinics = walkClinics;
            
            WalkInSearchViewModel walkInSearchViewModel = new WalkInSearchViewModel()
            {
               WalkInClinicInfo = walkClinics,
            };

            return View(walkInSearchViewModel);
        }

        [HttpPost("/newRequestPractictioner")]
        public async Task<IActionResult> SaveRequestPractictioner()
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string userName = "";


            if (claimuser.Identity.IsAuthenticated)
            {
                userName = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
            }

            // Access form fields using FormCollection
            string idpractictioner = Request.Form["idpractictioner"];
            string idpatient = Request.Form["idpatient"];
            string listofficeaddrequest = Request.Form["listofficeaddrequest"];
            string listofficeremoverequest = Request.Form["listofficeremoverequest"];
            string practictionerEmail = Request.Form["practemail"];

            Patient oPatient = await _mediLinkContext.Patients
               .Where(pa => pa.Id == Convert.ToInt32(idpatient))
               .Include(pa => pa.PatientDetails)
               .FirstAsync();

            Practitioner oPractitioner = await _mediLinkContext.Practitioners
              .Where(pa => pa.Id == Convert.ToInt32(idpractictioner))
              .FirstAsync();



            List<NewPatientRequest> ListNewPatientRequest = new List<NewPatientRequest>();


            //verify if the user add offices address to the new practitioner
            if (!string.IsNullOrEmpty(listofficeaddrequest))
            {
                //convert the string to array to iterate over each office id added
                string[] ListAddNewRequest = listofficeaddrequest.Split(",");

                //iterate over each office id added and save the address
                foreach (string office in ListAddNewRequest)
                {
                    int idOffice = Convert.ToInt32(office);

                    string statusPract = "";

                    if (oPractitioner.IsAcceptingNewPatients)
                    {
                        statusPract = "pending";
                    }
                    else
                    {
                        statusPract = "waitlist";
                    }

                    ListNewPatientRequest.Add(new NewPatientRequest { PractitionerId = Convert.ToInt32(idpractictioner), PatientId = Convert.ToInt32(idpatient), officePractitionerId = idOffice, status = statusPract });


                }

                _mediLinkContext.NewPatientRequests.AddRange(ListNewPatientRequest);

                await _mediLinkContext.SaveChangesAsync();

                TempData["mensajeRequestPract"] = "the Request for Add a New Practitioner has been created succesfully";

                // Map the path relative to the content root
                string path = Path.Combine(_webHostEnvironment.ContentRootPath, "Templates", "PractitionerRequestNotification.html");

                string content = System.IO.File.ReadAllText(path);

                string fullnamePract = oPractitioner.FirstName + " " + oPractitioner.LastName;

                string fullnamePatient = oPatient.PatientDetails.FirstName + " " + oPatient.PatientDetails.LastName;


                string htmlBody = string.Format(content, fullnamePract, fullnamePatient);

                Email emailDTO = new Email()
                {
                    recipient = oPractitioner.Email,
                    subject = "Notification New Patient Request MediLink",
                    body = htmlBody
                };

                bool sent = EmailService.SendEmail(emailDTO);

            }

            //verify if the user wants remove offices address to the practitioner
            if (!string.IsNullOrEmpty(listofficeremoverequest))
            {
                //convert the string to array to iterate over each office id removed
                string[] ListRemovedNewRequest = listofficeremoverequest.Split(",");

                //iterate over each office id added and save the address
                foreach (string office in ListRemovedNewRequest)
                {
                    int idOffice = Convert.ToInt32(office);

                    // Find the record(s) you want to delete using a where statement
                    var recordsToDelete = _mediLinkContext.NewPatientRequests.Where(e => e.PractitionerId == Convert.ToInt32(idpractictioner) && e.PatientId == Convert.ToInt32(idpatient) && e.officePractitionerId == idOffice).ToList();

                    // Delete the record(s)
                    _mediLinkContext.NewPatientRequests.RemoveRange(recordsToDelete);


                    await _mediLinkContext.SaveChangesAsync();
                }


                TempData["mensajeRequestPract"] = "the Request for Remove Practitioner has been created succesfully";

            }



            return RedirectToAction("SearchPractitioner", "Search");

        }

        [HttpGet]
        public async Task<IActionResult> ViewClinicHours(int id) 
        {
            Debug.WriteLine(id);
            ClaimsPrincipal claimuser = HttpContext.User;
            string userName = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                userName = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
            }

            ViewData["userName"] = userName;

            WalkInClinic walkInClinic = await _mediLinkContext.WalkInClinics.FindAsync(id);

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

            WalkInClinicHoursInfo walkInHoursInfo = new WalkInClinicHoursInfo() 
            {
                MondayHours = hoursDictionary.GetValueOrDefault("Monday", ""),
                TuesdayHours = hoursDictionary.GetValueOrDefault("Tuesday", ""),
                WednesdayHours = hoursDictionary.GetValueOrDefault("Wednesday", ""),
                ThursdayHours = hoursDictionary.GetValueOrDefault("Thursday", ""),
                FridayHours = hoursDictionary.GetValueOrDefault("Friday", ""),
                SaturdayHours = hoursDictionary.GetValueOrDefault("Saturday", ""),
                SundayHours = hoursDictionary.GetValueOrDefault("Sunday", "")
            };

            return View(walkInHoursInfo);
        }

        [HttpGet]
        public async Task<IActionResult> SearchViewHistoricalWaitTimes()
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string userName = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                userName = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
            }

            ViewData["userName"] = userName;

            Dictionary<string, string> historicalWaitTimes = new Dictionary<string, string>();

            HistoricalWaitTimesViewModel historicalWaitTimesViewModel = new HistoricalWaitTimesViewModel()
            {
                HistoricalWaitTimes = historicalWaitTimes
            };

            return View(historicalWaitTimesViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SearchViewHistoricalWaitTimes(int id, HistoricalWaitTimesViewModel historicalWaitTimesViewModel)
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string userName = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                userName = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
            }

            ViewData["userName"] = userName;

            WalkInClinic walkInClinic = await _mediLinkContext.WalkInClinics.FindAsync(id);

            var historicalWaitTimesQuery = _mediLinkContext.WalkInClinicHistoricalWaitTimes
                .Where(wch => wch.WalkInClinicId == walkInClinic.Id && wch.DayOfTheWeek == historicalWaitTimesViewModel.DayOfTheWeek);

            var clinicHoursQuery = _mediLinkContext.WalkInClinicHours
                .Where(wch => wch.WalkInClinicId == walkInClinic.Id && wch.DayOfTheWeek == historicalWaitTimesViewModel.DayOfTheWeek);

            var historicalWaitTimes = await historicalWaitTimesQuery.ToListAsync();
            var clinicHours = await clinicHoursQuery.ToListAsync();

            int openingHour = 0;
            int closingHour = 0;

            foreach (var day in clinicHours)
            {
                if (day.DayOfTheWeek == historicalWaitTimesViewModel.DayOfTheWeek)
                {
                    openingHour = int.Parse(day.OpeningTime.Substring(0, 2));
                    closingHour = int.Parse(day.ClosingTime.Substring(0, 2)) + 12;
                    break;
                }
            }

            // Create a dictionary to store the average wait times by hour
            Dictionary<int, int> averageWaitTimesByHour = new Dictionary<int, int>();

            // Group wait times by hour
            var waitTimesGroupedByHour = historicalWaitTimes
                .GroupBy(w => w.PatientCheckInTime.Hour);

            // Calculate average wait time for each hour
            foreach (var group in waitTimesGroupedByHour)
            {
                double averageWaitTime = group.Average(w => w.WaitTimeInSeconds);
                averageWaitTimesByHour[group.Key] = (int)averageWaitTime / 60;
            }

            historicalWaitTimesViewModel.HistoricalWaitTimes = ConstructHistoricalWaitTimesDictionary(openingHour, closingHour, averageWaitTimesByHour);

            return View(historicalWaitTimesViewModel);
        }

        private Dictionary<string, string> ConstructHistoricalWaitTimesDictionary(int openingHour, int closingHour, Dictionary<int, int> averageWaitTimesByHour)
        {
            var historicalWaitTimes = new Dictionary<string, string>();

            for (int hour = openingHour; hour < closingHour; hour++)
            {
                int hour12 = hour > 12 ? hour - 12 : hour;
                string period = hour >= 12 ? "PM" : "AM";
                string hourString = $"{hour12.ToString("00")}:00 {period}";

                if (averageWaitTimesByHour.ContainsKey(hour))
                {
                    historicalWaitTimes[hourString] = averageWaitTimesByHour[hour].ToString("0");
                }
                else
                {
                    historicalWaitTimes[hourString] = "N/A";
                }
            }

            return historicalWaitTimes;
        }

        private MediLinkDbContext _mediLinkContext;
        private IUserService _userService;
        private readonly IWebHostEnvironment _webHostEnvironment;
    }
}
