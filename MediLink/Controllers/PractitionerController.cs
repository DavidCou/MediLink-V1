﻿using MediLink.Entities;
using MediLink.Models;
using MediLink.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace MediLink.Controllers
{
    public class PractitionerController : Controller
    {
        public PractitionerController(MediLinkDbContext mediLinkContext, IUserService userService)
        {
            _mediLinkContext = mediLinkContext;
            _userService = userService;
        }

        public async Task<IActionResult> PractitionerHomePage()
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
                CurrentSpokenLanguages = practitionerSpokenLanguages,
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

            // commented out until the practitionertype issue can be solved
            //if (practitionerUpdateViewModel.CurrentPractitionerTypeId == null || practitionerUpdateViewModel.CurrentPractitionerTypeId == 0)
            //{
            //    errorMessage.Add("You must select a practitioner type");
            //}

            if (practitionerUpdateViewModel.Languages == null)
            {
                errorMessage.Add("You must select at least one spoken laguage");
            }
            Practitioner practitioner = await _userService.GetPractitionerByEmail(userName);

            List<PractitionerSpokenLanguages> spokenLanguages = await _mediLinkContext.PractitionerSpokenLanguages
                    .Where(psl => psl.PractitionerId == practitioner.Id).ToListAsync();

            if (errorMessage.Count == 0)
            {
                //This may have the same issue as patients update details but IDK, was not able to test - DC 
                if (practitionerUpdateViewModel.Languages != null)
                {
                    
                    foreach (var language in spokenLanguages)
                    {
                        _mediLinkContext.PractitionerSpokenLanguages.Remove(language);
                        await _mediLinkContext.SaveChangesAsync();
                    }

                    foreach (var language in practitionerUpdateViewModel.Languages)
                    {
                        var currentSpokenlanguage = new PractitionerSpokenLanguages { LanguageId = language.Id, PractitionerId = practitioner.Id };
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
                //practitioner.PractitionerTypeId = practitionerUpdateViewModel.CurrentPractitionerTypeId; // The id is being set to 0 for some reason, probably not being picked up 
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

                practitionerUpdateViewModel.Languages = languages;
                practitionerUpdateViewModel.CurrentSpokenLanguages = spokenLanguages;
                practitionerUpdateViewModel.PractitionerTypes = practitionerTypes;
                practitionerUpdateViewModel.CurrentPractitionerTypeId = practitionerType.Id;
                

                ViewBag.MyErrorList = errorMessage;
                ViewData["UpdateErrorMessage"] = errorMessage;
                return View(practitionerUpdateViewModel);
            }
        }

        private MediLinkDbContext _mediLinkContext;
        private IUserService _userService;
    }
}
