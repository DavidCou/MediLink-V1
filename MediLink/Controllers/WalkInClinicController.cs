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
    public class WalkInClinicController : Controller
    {
        public WalkInClinicController(MediLinkDbContext mediLinkContext, IUserService userService)
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

            WalkInClinic walkInClinic = await _userService.GetWalkInClinicByEmail(userName);

            OfficeAddress officeAddress = await _mediLinkContext.OfficeAddresses
                .Where(of => of.Id == walkInClinic.OfficeAddressId)
                .FirstOrDefaultAsync();

            List<WalkInPractitionerSpokenLanguages> walkInPractitionerSpokenLanguages = await _mediLinkContext.WalkInPractitionerSpokenLanguages
                .Where(wpsl => wpsl.WalkInPractitionerId == walkInClinic.Id)
                .ToListAsync();


            WalkClinicViewModel walkClinicViewModel = new WalkClinicViewModel()
            {
                WalkInClinic = walkInClinic,
                WalkInPractitionerSpokenLanguages = walkInPractitionerSpokenLanguages,
                OfficeAddress = officeAddress
            };

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

            WalkClinicUpdateViewModel walkClinicUpdateViewModel = new WalkClinicUpdateViewModel()
            {
                Languages = languages,
                PhoneNumber = walkInClinic.PhoneNumber,
                ClinicNotes = walkInClinic.ClinicNotes,
                Email = walkInClinic.Email,
                OfficeName = officeAddress.OfficeName,
                City = officeAddress.City,
                Province = officeAddress.Province,
                Country = officeAddress.country,
                PostalCode = officeAddress.PostalCode,
                StreetAddress = officeAddress.StreetAddress,
                UnitNumber = officeAddress.zone
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

            if (errorMessage.Count == 0)
            {
                WalkInClinic walkInClinic = await _userService.GetWalkInClinicByEmail(userName);

                OfficeAddress officeAddress = await _mediLinkContext.OfficeAddresses
                .Where(of => of.Id == walkInClinic.OfficeAddressId)
                .FirstOrDefaultAsync();

                OfficeType officeType = await _mediLinkContext.OfficeTypes
                    .Where(ot => ot.OfficeTypeName == "Walk In Clinic")
                    .FirstOrDefaultAsync();

                if (walkInClinic != null && officeAddress != null)
                {
                   
                    //TODO - Fix patientUpdateViewModel.SpokenLanguageIds it is always null for some reason and causes errors when reloading the page to show error messages
                    if (walkClinicUpdateViewModel.Languages != null)
                    {
                        List<WalkInPractitionerSpokenLanguages>? previousSpokenLanguages = await _mediLinkContext.WalkInPractitionerSpokenLanguages 
                        .Where(wpsl =>  wpsl.WalkInPractitionerId == walkInClinic.Id).ToListAsync();
                        foreach (var language in previousSpokenLanguages)
                        {
                            _mediLinkContext.WalkInPractitionerSpokenLanguages.Remove(language);
                            await _mediLinkContext.SaveChangesAsync();
                        }

                        foreach (var language in walkClinicUpdateViewModel.Languages)
                        {
                            var currentSpokenlanguage = new WalkInPractitionerSpokenLanguages { LanguageId = language.Id, WalkInPractitionerId = walkInClinic.Id };
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
                    officeAddress.zone = walkClinicUpdateViewModel.UnitNumber;
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
                    ViewBag.MyErrorList = errorMessage;
                    ViewData["UpdateErrorMessage"] = errorMessage;
                    return View(walkClinicUpdateViewModel);
                }

            }
            else
            {
                ViewBag.MyErrorList = errorMessage;
                ViewData["UpdateErrorMessage"] = errorMessage;
                return View(walkClinicUpdateViewModel);
            }

            ViewBag.Success = "Your profile has been updated.";
            ViewData["UpdateSuccess"] = "Your profile has been updated.";

            return RedirectToAction("WalkinClinicHomePage");
        }

        private MediLinkDbContext _mediLinkContext;
        private IUserService _userService;
    }
}
