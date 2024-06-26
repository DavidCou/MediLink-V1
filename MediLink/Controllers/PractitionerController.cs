﻿using MediLink.Entities;
using MediLink.Models;
using MediLink.Services;
using MediLink.Services.Contract;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace MediLink.Controllers
{
    public class PractitionerController : Controller
    {
        public PractitionerController(MediLinkDbContext mediLinkContext, IUserService userService, IWebHostEnvironment webHostEnvironment)
        {
            _mediLinkContext = mediLinkContext;
            _userService = userService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> PractitionerHomePage()
        {
            //created by juan quintana -setup a message to inforn status for add or remove address
            string mensajeResultSave = TempData["mensajeResultSaveAddress"] as string;
        
            ViewData["messageUpdatePract"] = mensajeResultSave;

            //created by juan quintana -setup a message to inforn status for add or remove address
            string mensajeResAddAddress = TempData["mensajeResultAddAddress"] as string;

            ViewData["messageUpdateOffic"] = mensajeResAddAddress;



            ClaimsPrincipal claimuser = HttpContext.User;
            string userName = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                userName = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
            }

            ViewData["userName"] = userName;

            Practitioner practitioner = await _userService.GetPractitionerByEmail(userName);

            List<PractitionerOfficeAddress> practitionerOfficeAddresses = await _mediLinkContext.PractitionerAddresses
                .Where(pa => pa.PractitionerId == practitioner.Id)
                .Include(pa => pa.OfficeAddresses)
                .ThenInclude(oa => oa.OfficeType)
                .ToListAsync();

            List<OfficeAddress> officeAddresses = new List<OfficeAddress>();

            foreach(var officeAddress in practitionerOfficeAddresses)
            {
                officeAddresses.Add(officeAddress.OfficeAddresses);
            }

            List<PractitionerSpokenLanguages> practitionerSpokenLanguages = await _mediLinkContext.PractitionerSpokenLanguages
                .Where(psl => psl.PractitionerId == practitioner.Id).Include(psl => psl.Language).ToListAsync();

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

            var reviews = await _mediLinkContext.PractitionerReviews.Where(pr => pr.PractitionerId == practitioner.Id).ToListAsync();
            if(reviews.Count > 0)
            {
                double rating = reviews.Average(r => r.Rating);
                practitioner.rating = rating;
            }
            else
            {
                practitioner.rating = 0;
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

        [HttpGet]
        public async Task<IActionResult> UpdatePractitionerDetails()
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

            PractitionerType currentPractitionerType = await _mediLinkContext.PractitionerTypes
                .Where(pt => pt.Id == practitioner.PractitionerTypeId)
                .FirstOrDefaultAsync();

            List<PractitionerSpokenLanguages> practitionerSpokenLanguages = await _mediLinkContext.PractitionerSpokenLanguages
                .Where(psl => psl.PractitionerId == practitioner.Id)
                .ToListAsync();

            List<int> currentSpokenLanguageIds = new List<int>();

            foreach (var language in practitionerSpokenLanguages)
            {
                currentSpokenLanguageIds.Add(language.LanguageId);
            }

            List<PractitionerType> practitionerTypes = await _mediLinkContext.PractitionerTypes.ToListAsync();

            List<Languages> languages = await _mediLinkContext.Languages.ToListAsync();

            string isAcceptingNewPatients = "";
            if (practitioner.IsAcceptingNewPatients = true)
            {
                isAcceptingNewPatients = "true";
            }
            else
            {
                isAcceptingNewPatients = "false";
            }

            PractitionerUpdateViewModel practitionerUpdateViewModel = new PractitionerUpdateViewModel()
            {
                Email = practitioner.Email,
                FirstName = practitioner.FirstName,
                LastName = practitioner.LastName,
                Gender = practitioner.gender,
                PhoneNumber = practitioner.PhoneNumber,
                IsAcceptingNewPatients = isAcceptingNewPatients,
                Languages = languages,
                CurrentSpokenLanguageIds = currentSpokenLanguageIds,
                PractitionerTypes = practitionerTypes,
                CurrentPractitionerTypeId = currentPractitionerType.Id

    };

            return View(practitionerUpdateViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePractitionerDetails(PractitionerUpdateViewModel practitionerUpdateViewModel)
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

            if (string.IsNullOrEmpty(practitionerUpdateViewModel.Email) || string.IsNullOrWhiteSpace(practitionerUpdateViewModel.Email))
            {
                errorMessage.Add("Your email cannot be blank");
            }

            if (!string.IsNullOrEmpty(practitionerUpdateViewModel.PhoneNumber) && !string.IsNullOrWhiteSpace(practitionerUpdateViewModel.PhoneNumber))
            {
                if (!regex.IsMatch(practitionerUpdateViewModel.PhoneNumber))
                {
                    errorMessage.Add("Phone number not valid, please use the following input format: 222-222-2222");
                }
            }
            else
            {
                errorMessage.Add("Phone number cannot be left blank");
            }

            if (practitionerUpdateViewModel.Gender == null)
            {
                errorMessage.Add("You must select a gender");
            }

            if (practitionerUpdateViewModel.CurrentPractitionerTypeId == null || practitionerUpdateViewModel.CurrentPractitionerTypeId == 0)
            {
                errorMessage.Add("You must select a practitioner type");
            }

            if (practitionerUpdateViewModel.CurrentSpokenLanguageIds == null)
            {
                errorMessage.Add("There must be at least one selected spoken language");
            }
            Practitioner practitioner = await _userService.GetPractitionerByEmail(userName);

            List<PractitionerSpokenLanguages> spokenLanguages = await _mediLinkContext.PractitionerSpokenLanguages
                    .Where(psl => psl.PractitionerId == practitioner.Id)
                    .Include(psl => psl.Language)
                    .ToListAsync();

            if (errorMessage.Count == 0)
            {
                if (practitionerUpdateViewModel.CurrentSpokenLanguageIds != null)
                {
                    
                    foreach (var language in spokenLanguages)
                    {
                        _mediLinkContext.PractitionerSpokenLanguages.Remove(language);
                        await _mediLinkContext.SaveChangesAsync();
                    }

                    foreach (var languageId in practitionerUpdateViewModel.CurrentSpokenLanguageIds)
                    {
                        var currentSpokenlanguage = new PractitionerSpokenLanguages { LanguageId = languageId, PractitionerId = practitioner.Id };
                        _mediLinkContext.PractitionerSpokenLanguages.Add(currentSpokenlanguage);
                        await _mediLinkContext.SaveChangesAsync();
                    }
                }

                if (practitionerUpdateViewModel.IsAcceptingNewPatients == "true")
                {
                    practitioner.IsAcceptingNewPatients = true;
                }
                else
                {
                    practitioner.IsAcceptingNewPatients = false;
                }

                practitioner.Email = practitionerUpdateViewModel.Email;
                practitioner.FirstName = practitionerUpdateViewModel.FirstName;
                practitioner.LastName = practitionerUpdateViewModel.LastName;
                practitioner.gender = practitionerUpdateViewModel.Gender;
                practitioner.PhoneNumber = practitionerUpdateViewModel.PhoneNumber;
                practitioner.PractitionerTypeId = practitionerUpdateViewModel.CurrentPractitionerTypeId; 
                _mediLinkContext.Update(practitioner);
                await _mediLinkContext.SaveChangesAsync();

                ViewBag.Success = "Your profile has been updated.";
                ViewData["UpdateSuccess"] = "Your profile has been updated.";

                return RedirectToAction("PractitionerHomePage");

            }
            else
            {
                List<Languages> languages = await _mediLinkContext.Languages.ToListAsync();
                List<PractitionerType> practitionerTypes = await _mediLinkContext.PractitionerTypes.ToListAsync();
                PractitionerType practitionerType = await _mediLinkContext.PractitionerTypes.Where(pt => pt.Id ==practitioner.PractitionerTypeId).FirstOrDefaultAsync();

                List<int> currentSpokenLanguageIds = new List<int>();

                foreach (var currentLanguage in spokenLanguages)
                {
                    currentSpokenLanguageIds.Add(currentLanguage.LanguageId);
                }

                practitionerUpdateViewModel.Languages = languages;
                practitionerUpdateViewModel.CurrentSpokenLanguageIds = currentSpokenLanguageIds;
                practitionerUpdateViewModel.PractitionerTypes = practitionerTypes;
                practitionerUpdateViewModel.CurrentPractitionerTypeId = practitionerType.Id;
                

                ViewBag.MyErrorList = errorMessage;
                ViewData["UpdateErrorMessage"] = errorMessage;
                return View(practitionerUpdateViewModel);
            }
        }



        //created by juan quintana
        //remove an existing address in the practictioner profile
        [HttpGet("/RemovePractitionerAddress/{id}")]
        public async Task<IActionResult> RemovePractitionerAddress(int id)
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string userName = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                userName = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
            }

            //get the current practitioner
            Practitioner practitioner = await _userService.GetPractitionerByEmail(userName);


            //create an instace of practitioneraddress
            PractitionerOfficeAddress practitionerOfficeAddress = new PractitionerOfficeAddress();

            //verify if exist a practitioner
            if (practitioner != null)
            {

                //set the values of address and practitioner
                practitionerOfficeAddress.PractitionerId = practitioner.Id;
                practitionerOfficeAddress.OfficeAddressesId = id;

                // Remove the record from the DbSet
                _mediLinkContext.PractitionerAddresses.Remove(practitionerOfficeAddress);

                // Save changes to persist the deletion
                _mediLinkContext.SaveChanges();

            }

            return Ok();
        }

        //get all the offices availables
        [HttpGet("/ListOffices")]
        public async Task<IActionResult> ListOffices()
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


            // use the DB contet to query for all Address entities and transform them into
            // OfficeInf bjects:
            List<OfficeInfo> practitionerOfficeAddresses = await _mediLinkContext.PractitionerAddresses
                    .Where(t => t.PractitionerId == practitioner.Id)
                    .Include(t => t.OfficeAddresses)
                    .Include(t => t.OfficeAddresses.OfficeType)
                    .Select(t => new OfficeInfo()
                    {
                        fullAddress = t.OfficeAddresses.StreetAddress + " " + t.OfficeAddresses.City + " " + t.OfficeAddresses.PostalCode,
                        OfficeName = t.OfficeAddresses.OfficeName,
                        OfficeTypeName = t.OfficeAddresses.OfficeType.OfficeTypeName,
                        Id = t.OfficeAddresses.Id

                    })
                    .ToListAsync();


            // use the DB contet to query for all Address entities and transform them into
            // OfficeInf bjects:
            List<OfficeInfo> offices = await _mediLinkContext.OfficeAddresses
                    .Include(t => t.OfficeType)
                    .OrderByDescending(t => t.StreetAddress)
                    .Select(t => new OfficeInfo()
                    {
                        fullAddress = t.StreetAddress + " " + t.City + " " + t.PostalCode,
                        OfficeName = t.OfficeName,
                        OfficeTypeName = t.OfficeType.OfficeTypeName,
                        Id = t.Id

                    })
                    .ToListAsync();



            // Compare the two lists and remove duplicates
            // Check if the id is present in the list
            bool isIdPresent = false;
            List<OfficeInfo> uniqueList = new List<OfficeInfo>();

            foreach (OfficeInfo off in practitionerOfficeAddresses)
            {
                isIdPresent = offices.Any(obj => obj.Id == off.Id);

                if (isIdPresent)
                {

                    OfficeInfo officeInfoToRemove = offices.FirstOrDefault(o => o.Id == off.Id);
                    if (officeInfoToRemove != null)
                    {
                        // Remove the person from the list
                        offices.Remove(officeInfoToRemove);
                    }



                }
            }



            return Json(offices);

        }

        //get all the offices availables
        [HttpGet("/ListAllOffices")]
        public async Task<IActionResult> ListAllOffices()
        {

            // use the DB contet to query for all Address entities and transform them into
            // OfficeInf bjects:
            List<OfficeInfo> offices = await _mediLinkContext.OfficeAddresses
                    .Include(t => t.OfficeType)
                    .OrderByDescending(t => t.StreetAddress)
                    .Select(t => new OfficeInfo()
                    {
                        fullAddress = t.StreetAddress + " " + t.City + " " + t.PostalCode,
                        OfficeName = t.OfficeName,
                        OfficeTypeName = t.OfficeType.OfficeTypeName,
                        Id = t.Id

                    })
                    .ToListAsync();


            return Json(offices);

        }

        //get all the offices availables
        [HttpGet("/ListAllOfficesTypes")]
        public async Task<IActionResult> ListAllOfficesTypes()
        {

            // use the DB contet to query for all Address entities and transform them into
            // OfficeInf bjects:
            List<OfficeType> officesTypes = await _mediLinkContext.OfficeTypes.ToListAsync();

            return Json(officesTypes);

        }

        [HttpPost]
        public async Task<IActionResult> SavePractitionerAddress(string listOffices)
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

            //create PractitionerOfficeAddress instance
            List<PractitionerOfficeAddress> practitionerOfficeAddress = new List<PractitionerOfficeAddress>();

            //verify if the user add offices address to the new practitioner
            if (!string.IsNullOrEmpty(listOffices))
            {
                //convert the string to array to iterate over each office id added
                string[] offices = listOffices.Split(",");

                //iterate over each office id added and save the address
                foreach (string office in offices)
                {
                    int idOffice = Convert.ToInt32(office);

                    practitionerOfficeAddress.Add(new PractitionerOfficeAddress { PractitionerId = practitioner.Id, OfficeAddressesId = idOffice });


                }

                _mediLinkContext.PractitionerAddresses.AddRange(practitionerOfficeAddress);

                await _mediLinkContext.SaveChangesAsync();

                if (offices.Length > 0 && offices.Length < 2)
                {
                    TempData["mensajeResultSaveAddress"] = "One address has been added successfully";
                }
                else
                {
                    TempData["mensajeResultSaveAddress"] = $"{offices.Length} addresses has been added successfully";
                }






            }



            return RedirectToAction("PractitionerHomePage");
        }

        
        [HttpGet("/ViewNewPatientRequests/{status}")]
        public async Task<ActionResult> ViewNewPatientRequests(string status)
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string userName = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                userName = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
            }

            Practitioner oPractitioner = await _mediLinkContext.Practitioners
            .Include(pt => pt.PractitionerType)
            .Where(pa => pa.Email == userName)
            .FirstAsync();
           
            //Practitioner oPractitioner = await _userService.GetPractitionerByEmail(userName);

            List<PatientNewRequest> ListPatientReq = new List<PatientNewRequest>();

            // Find the record(s) you want to delete using a where statement
            List<NewPatientRequest> ListNewPatientRequest = await _mediLinkContext.NewPatientRequests
            .Where(pa => pa.PractitionerId == oPractitioner.Id && pa.status == status)
            .Include(pat => pat.Patient)
            .ThenInclude(oa => oa.PatientDetails)
            .ToListAsync();

            foreach (var item in ListNewPatientRequest)
            {
                PatientNewRequest oPatientNewRequest = new PatientNewRequest();

                PatientAddress oPatientAddress = await _mediLinkContext.PatientAddress
               .Where(pa => pa.Id == Convert.ToInt32(item.Patient.PatientDetails.PatientAddressesId))
               .FirstAsync();

                OfficeAddress oOfficeAddress = await _mediLinkContext.OfficeAddresses
               .Where(pa => pa.Id == Convert.ToInt32(item.officePractitionerId))
               .FirstAsync();

                //get the patient birthday
                DateTime patientDoB = (DateTime)item.Patient.PatientDetails.DoB;

                // Get the current date
                DateTime currentDate = DateTime.Today;

                // Calculate the age
                int patientAge = currentDate.Year - patientDoB.Year;

                // Check if the birthday has occurred this year
                if (currentDate.Month < patientDoB.Month || (currentDate.Month == patientDoB.Month && currentDate.Day < patientDoB.Day))
                {
                    patientAge--;
                }

                oPatientNewRequest.Id = item.Patient.Id;
                oPatientNewRequest.fullname = item.Patient.PatientDetails.FirstName + " " + item.Patient.PatientDetails.LastName;
                oPatientNewRequest.fullAddress = oPatientAddress.StreetAddress + " " + oPatientAddress.City + " " + oPatientAddress.Province;
                oPatientNewRequest.officeName = oOfficeAddress.OfficeName;
                oPatientNewRequest.dateRequest = item.DateRequest.ToShortDateString();
                oPatientNewRequest.dateApproved = item.DateApproved.ToShortDateString();
                oPatientNewRequest.age = patientAge;
                oPatientNewRequest.gender = item.Patient.PatientDetails.gender;
                oPatientNewRequest.officeId = oOfficeAddress.Id;
                oPatientNewRequest.practictionerId = item.PractitionerId;
                oPatientNewRequest.practictionerFullname = item.Practitioner.FirstName + " " + item.Practitioner.LastName;
                oPatientNewRequest.practictionerType = item.Practitioner.PractitionerType.Name;
                oPatientNewRequest.officeAddress = oOfficeAddress.StreetAddress + " " + oOfficeAddress.City + " " + oOfficeAddress.Province;

                ListPatientReq.Add(oPatientNewRequest);


            }



            return Json(ListPatientReq);



        }

        [HttpGet("/ViewPatientRequests/{status}")]
        public async Task<ActionResult> ViewPatientRequests(string status)
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string userName = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                userName = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
            }

            Patient oPatient = await _userService.GetUserByEmail(userName);

            List<PatientNewRequest> ListPatientReq = new List<PatientNewRequest>();

            // Find the record(s) you want to delete using a where statement
            List<NewPatientRequest> ListNewPatientRequest = await _mediLinkContext.NewPatientRequests
            .Where(pa => pa.PatientId == oPatient.Id && pa.status == status)
            .Include(pat => pat.Patient)
            .ThenInclude(oa => oa.PatientDetails)
            .ToListAsync();

            foreach (var item in ListNewPatientRequest)
            {
                PatientNewRequest oPatientNewRequest = new PatientNewRequest();

                PatientAddress oPatientAddress = await _mediLinkContext.PatientAddress
               .Where(pa => pa.Id == Convert.ToInt32(item.Patient.PatientDetails.PatientAddressesId))
               .FirstAsync();

                OfficeAddress oOfficeAddress = await _mediLinkContext.OfficeAddresses
               .Where(pa => pa.Id == Convert.ToInt32(item.officePractitionerId))
               .FirstAsync();

                Practitioner oPractitioner = await _mediLinkContext.Practitioners
               .Include(pt => pt.PractitionerType)
               .Where(pa => pa.Id == Convert.ToInt32(item.PractitionerId))
               .FirstAsync();

                //get the patient birthday
                DateTime patientDoB = (DateTime)item.Patient.PatientDetails.DoB;

                // Get the current date
                DateTime currentDate = DateTime.Today;

                // Calculate the age
                int patientAge = currentDate.Year - patientDoB.Year;

                // Check if the birthday has occurred this year
                if (currentDate.Month < patientDoB.Month || (currentDate.Month == patientDoB.Month && currentDate.Day < patientDoB.Day))
                {
                    patientAge--;
                }

                oPatientNewRequest.Id = item.Patient.Id;
                oPatientNewRequest.fullname = item.Patient.PatientDetails.FirstName + " " + item.Patient.PatientDetails.LastName;
                oPatientNewRequest.fullAddress = oPatientAddress.StreetAddress + " " + oPatientAddress.City + " " + oPatientAddress.Province;
                oPatientNewRequest.officeName = oOfficeAddress.OfficeName;
                oPatientNewRequest.dateRequest = item.DateRequest.ToShortDateString();
                oPatientNewRequest.dateApproved = item.DateApproved.ToShortDateString();
                oPatientNewRequest.age = patientAge;
                oPatientNewRequest.gender = item.Patient.PatientDetails.gender;
                oPatientNewRequest.officeId = oOfficeAddress.Id;
                oPatientNewRequest.practictionerId = item.PractitionerId;
                oPatientNewRequest.practictionerFullname = oPractitioner.FirstName + " " + oPractitioner.LastName;
                oPatientNewRequest.practictionerType = oPractitioner.PractitionerType.Name;
                oPatientNewRequest.officeAddress = oOfficeAddress.StreetAddress + " " + oOfficeAddress.City + " " + oOfficeAddress.Province;
                oPatientNewRequest.practEmail = oPractitioner.Email;

                ListPatientReq.Add(oPatientNewRequest);


            }



            return Json(ListPatientReq);



        }

        [HttpPost]
        public async Task<ActionResult> SaveRequestPatient()
        {

            Practitioner oPractitioner = null;

            string patientUpdate = Request.Form["status-pat-req"];
            string statusPatient = "";


            Console.WriteLine(patientUpdate);

            if(!string.IsNullOrEmpty(patientUpdate)) {

                string[] newPatientReq = patientUpdate.Split(',');



                foreach (var item in newPatientReq)
                {
                    string[] currentPatient = item.Split("-");
                    int patientID = Convert.ToInt32(((string)currentPatient[0]));
                    statusPatient = currentPatient[1].Trim();
                    int practOfficeID = Convert.ToInt32(((string)currentPatient[2]));

                    //set a variable to store the practictioner ID the come from the post request
                    int practID;

                    if(currentPatient.Length > 3)
                    {
                        practID = Convert.ToInt32(((string)currentPatient[3]));

                       oPractitioner = await _mediLinkContext.Practitioners
                      .Where(pa => pa.Id == practID)
                      .FirstAsync();

                    }
                    else
                    {
                        ClaimsPrincipal claimuser = HttpContext.User;
                        string userName = "";

                        if (claimuser.Identity.IsAuthenticated)
                        {
                            userName = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name)
                                .Select(c => c.Value).SingleOrDefault();
                        }

                        oPractitioner = await _userService.GetPractitionerByEmail(userName);

                        practID = oPractitioner.Id;
                    }

                    Patient oPatient = await _mediLinkContext.Patients
                  .Where(pa => pa.Id == patientID)
                  .Include(pa => pa.PatientDetails)
                  .FirstAsync();
                   

                    // Query the new patient request record by its ID
                    NewPatientRequest oNewPatientRequest = await _mediLinkContext.NewPatientRequests.Where(pa => pa.PractitionerId == practID && pa.PatientId == patientID && pa.officePractitionerId == practOfficeID).FirstAsync();

                    if (oNewPatientRequest != null)
                    {
                        oNewPatientRequest.status = statusPatient;

                        // Map the path relative to the content root
                        string path = "";


                        if (statusPatient == "approve")
                        {
                            oNewPatientRequest.DateApproved = DateTime.Now;

                            oPractitioner.lastPatientAcceptedDate = DateTime.Now;

                            // update the record
                            _mediLinkContext.Practitioners.Update(oPractitioner);
                            _mediLinkContext.SaveChanges();

                            // Map the path relative to the content root
                            path = Path.Combine(_webHostEnvironment.ContentRootPath, "Templates", "PatientRequestNotification.html");


                            
                        }


                        if (statusPatient == "waitlist")
                        {
                           
                            // Map the path relative to the content root
                            path = Path.Combine(_webHostEnvironment.ContentRootPath, "Templates", "PatientRequestNotificationWaitList.html");


                        }

                        if (statusPatient == "deny")
                        {

                            // Map the path relative to the content root
                            path = Path.Combine(_webHostEnvironment.ContentRootPath, "Templates", "PatientRequestNotificationDenied.html");


                        }

                        if (statusPatient == "removed")
                        {

                            // Map the path relative to the content root
                            path = Path.Combine(_webHostEnvironment.ContentRootPath, "Templates", "PatientRequestNotificationRemoved.html");


                        }

                       
                        if (statusPatient == "canceled")
                        {

                            // Map the path relative to the content root
                            path = Path.Combine(_webHostEnvironment.ContentRootPath, "Templates", "PatientCancelRequest.html");


                        }

                        //prepare to send email
                        string content = System.IO.File.ReadAllText(path);

                        string fullnamePract = oPractitioner.FirstName + " " + oPractitioner.LastName;

                        string fullnamePatient = oPatient.PatientDetails.FirstName + " " + oPatient.PatientDetails.LastName;

                        string htmlBody = string.Format(content, fullnamePatient, fullnamePract);

                        Email emailDTO = new Email()
                        {
                            recipient = oPractitioner.Email,
                            subject = "Notification New Patient Request MediLink",
                            body = htmlBody
                        };

                        bool sent = EmailService.SendEmail(emailDTO);




                        // update the record
                        _mediLinkContext.NewPatientRequests.Update(oNewPatientRequest);
                        _mediLinkContext.SaveChanges();
                        
                        TempData["mensajeResultSaveAddress"] = "The New Patient Request has been updated successfully";

                    }
                    else
                    {

                        TempData["mensajeResultSaveAddress"] = "Error.. The New Patient Request has not been updated successfully";
                    }

                   
                }

            }

            if (statusPatient == "canceled")
            {
                TempData["mensajeResultSaveAddress"] = "The Request in the waitlist has been canceled successfully";

                return RedirectToAction("PatientHomePage", "Patient");


            }
            else
            {
                return RedirectToAction("PractitionerHomePage");
            }
            


        }

        [HttpPost]
        public ActionResult AddNewOffice()
        {
           
            // Access form fields using FormCollection
            string officeName = Request.Form["officename"];
            int typeOffice = Convert.ToInt32(Request.Form["officeType"]);
            string street = Request.Form["street"];
            string city = Request.Form["city"];
            string province = Request.Form["province"];
            string postalCode = Request.Form["postalcode"];
            string zone = Request.Form["zone"];
            string country = Request.Form["country"];

            OfficeAddress address = new OfficeAddress();

            address.OfficeName = officeName;
            address.OfficeTypeId = typeOffice;
            address.StreetAddress = street;
            address.City = city;
            address.Province = province;
            address.country = country;
            address.PostalCode = postalCode;
            address.zone = zone;

            _mediLinkContext.OfficeAddresses.Add(address);
            _mediLinkContext.SaveChanges();

            TempData["mensajeResultAddAddress"] = "The office " + officeName + " has beed added successfully";

            return RedirectToAction("PractitionerHomePage");



        }


        private MediLinkDbContext _mediLinkContext;
        private IUserService _userService;
        private readonly IWebHostEnvironment _webHostEnvironment;
    }
}
