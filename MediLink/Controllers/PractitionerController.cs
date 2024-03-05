﻿using MediLink.Entities;
using MediLink.Models;
using MediLink.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
                .Where(pa => pa.OfficeAddressesId == practitioner.Id)
                .ToListAsync();

            List<OfficeAddress> officeAddresses = new List<OfficeAddress>();

            foreach(var officeAddress in practitionerOfficeAddresses)
            {
                officeAddresses.Add(officeAddress.OfficeAddresses);
            }

            List<PractitionerSpokenLanguages> practitionerSpokenLanguages = await _mediLinkContext.PractitionerSpokenLanguages
                .Where(psl => psl.PractitionerId == practitioner.Id).ToListAsync();

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

        private MediLinkDbContext _mediLinkContext;
        private IUserService _userService;
    }
}