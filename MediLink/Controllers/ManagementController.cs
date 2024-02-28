using MediLink.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Web;

using MediLink.Services;
using MediLink.Resources;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using MediLink.Services.Contract;
using MediLink.Models;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using NuGet.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace MediLink.Controllers
{
    public class ManagementController : Controller
    {
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private MediLinkDbContext _mediLinkContext;

        public ManagementController(IUserService userService, IWebHostEnvironment webHostEnvironment, MediLinkDbContext mediLinkDbContext)
        {
            _userService = userService;
            _webHostEnvironment = webHostEnvironment;
            _mediLinkContext = mediLinkDbContext;

        }


        public ActionResult LoginPatient()
        {
            string mensajeResultLog = TempData["mensajeResultLogin"] as string;

            ViewData["mensajeResultLoginPatient"] = mensajeResultLog;

            return View();

        }

        [HttpPost]
        public async Task<IActionResult> LoginPatient(string Email, string Password)
        {

            Patient patientFound = await _userService.GetUser(Email, Utilities.EncryptPassword(Password));

            if (patientFound == null)
            {
                ViewData["mensajeResultLoginPatient"] = "User Not Found";
                return View();
            }


            if (!patientFound.IsEmailConfirmed)
            {
                ViewData["mensajeResultLoginPatient"] = "Your email has not been confirmed yet. An email has been sent to your account";
                return View();
            }

            if (patientFound.passwordReset)
            {
                ViewData["mensajeResultLoginPatient"] = "A password reset has been requested for your account and an email has been sent to your account. Please go to the email and reset your password";
                return View();
            }


            List<Claim> claims = new List<Claim>() {
                new Claim(ClaimTypes.Name, patientFound.Email)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
                );

            return RedirectToAction("PatientHomePage", "Patient");
        }

        public ActionResult RegisterPatient()
        {
            PatientNewRequest oPatient = new PatientNewRequest();
            return View(oPatient);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterPatient(PatientNewRequest oPatientNew)
        {
            //verify if the patient already exist 
            Patient patientFound = await _userService.GetUserByEmail(oPatientNew.Email);

            List<string> errorMessage = new List<string>();
            //string[] errorMessage;

            if (patientFound != null)
            {
                ViewData["MessageRegister"] = "User already exists";
                return View();
            }


            // Regular expression for phone validation
            string phonePattern = @"^\d{3}-\d{3}-\d{4}$";

            // Creating Regex object
            Regex regex = new Regex(phonePattern);

            DateTime currentDate = DateTime.Now;
            DateTime inputDate = new DateTime(oPatientNew.DoB.Year, oPatientNew.DoB.Month, oPatientNew.DoB.Day);

            // Calculate the current date minus 2 days
            DateTime currentDateMinusTwoDays = currentDate.AddDays(-2);

            if (string.IsNullOrEmpty(oPatientNew.FirstName) || string.IsNullOrWhiteSpace(oPatientNew.FirstName))
            {
                errorMessage.Add("-Please enter your first name");
            }

            if (string.IsNullOrEmpty(oPatientNew.LastName) || string.IsNullOrWhiteSpace(oPatientNew.LastName))
            {
                errorMessage.Add("-Please enter your last name");
            }

            if (string.IsNullOrEmpty(oPatientNew.Email) || string.IsNullOrWhiteSpace(oPatientNew.Email))
            {
                errorMessage.Add("-Please enter you email");
            }

                     

            if (oPatientNew.Password != oPatientNew.ConfirmPassword)
            {
                errorMessage.Add("-Password does not match");
                
            }

            if (string.IsNullOrEmpty(oPatientNew.Password) && string.IsNullOrEmpty(oPatientNew.ConfirmPassword))
            {
                errorMessage.Add("-Please enter a password");
                
            }

            //verify if the user has selected personal infomation tag
            if (!string.IsNullOrEmpty(oPatientNew.PhoneNumber) || !string.IsNullOrEmpty(oPatientNew.gender) || !oPatientNew.DoB.ToString().Contains("0001-01-01"))
            {
                // Matching the number against the regex pattern
                if (!regex.IsMatch(oPatientNew.PhoneNumber))
                {
                    errorMessage.Add("-Phone number not valid, please use the following input format: 222-222-2222");
                  

                }

                // valid if selected a gender
                if (oPatientNew.gender == null)
                {
                    errorMessage.Add("-Must select a gender ");
                   

                }

                if (oPatientNew.DoB.ToString().Contains("0001-01-01"))
                {
                    errorMessage.Add("-Must select a date of birth");                   

                }
                else
                {
                    if (inputDate >= currentDateMinusTwoDays)
                    {
                        errorMessage.Add("-Birthday is invalid, your birthday must be at least two days in the past");
                       

                    }
                }

            }

            //verify if the user has selected address infomation tag
            if (!string.IsNullOrEmpty(oPatientNew.StreetAddress) || !string.IsNullOrEmpty(oPatientNew.City) || !string.IsNullOrEmpty(oPatientNew.PostalCode))
            {
                // valid if selected a address
                if (string.IsNullOrEmpty(oPatientNew.StreetAddress))
                {
                    errorMessage.Add("-Please enter your street address ");                    

                }

                // valid if selected a city
                if (string.IsNullOrEmpty(oPatientNew.City))
                {
                    errorMessage.Add("-Please enter your city ");
                   
                }

                // valid if selected a city
                if (oPatientNew.Province == null)
                {
                    errorMessage.Add("-Please select a province");

                }

                // valid if selected a ;ostcode
                if (string.IsNullOrEmpty(oPatientNew.PostalCode))
                {
                    errorMessage.Add("-Please enter your postal code");

                }

            }



            // valid if there are errors
            if (errorMessage.Count == 0)
            {
                oPatientNew.Password = Utilities.EncryptPassword(oPatientNew.Password);

                oPatientNew.token = Utilities.GenerateToken();

                PatientNewRequest userCreated = await _userService.SavePatient(oPatientNew);

                if (userCreated.Id > 0)
                {
                    // Map the path relative to the content root
                    string path = Path.Combine(_webHostEnvironment.ContentRootPath, "Templates", "ConfirmEmail.html");

                    string content = System.IO.File.ReadAllText(path);
                    string url = string.Format("{0}://{1}{2}", HttpContext.Request.Scheme, Request.Headers["host"], "/Management/ConfirmRegisterPatient?token=" + oPatientNew.token);

                    string htmlBody = string.Format(content, oPatientNew.LastName, url);

                    Email emailDTO = new Email()
                    {
                        recipient = oPatientNew.Email,
                        subject = "Confirmation Email MediLink",
                        body = htmlBody
                    };

                    bool sent = EmailService.SendEmail(emailDTO);
                    errorMessage.Add($"Your account has been created. We have sent a message to the email {oPatientNew.Email} to confirm your email address");
                   

                }
                else
                {
                    errorMessage.Add("Can not create user");
                   
                }

                ViewBag.MyErrorList = errorMessage;
                ViewData["MessageRegister"] = errorMessage;


                PatientNewRequest oPatient = new PatientNewRequest();
                oPatient.FirstName = "";
                oPatient.LastName = "";
                oPatient.Password = "";
                oPatient.ConfirmPassword = "";
                return View(oPatient);

            }
            else
            {
                // Pass the error list to the view using ViewBag
                ViewBag.MyErrorList = errorMessage;
                ViewData["MessageRegister"] = errorMessage;
                return View(oPatientNew);
            }






            


        }

        //confirm tha the email was sent to activate the account
        public async Task<IActionResult> ConfirmRegisterPatient(string token)
        {

            Patient oPatient = await _userService.ConfirmRegisterPatient(token);

            if(oPatient != null) 
            {
                ViewBag.Response = oPatient.IsEmailConfirmed;
            }
            else
            {
                ViewBag.Response = false;
            }
           

            return View();

        }

        public async Task<IActionResult> RestartPassword(string email)
        {
            Patient oPatient = await _userService.ResetPasswordPatient(email);
           
            if (oPatient != null)
            {
                              
                string path = Path.Combine(_webHostEnvironment.ContentRootPath, "Templates", "RestorePassword.html");
                string content = System.IO.File.ReadAllText(path);
                string url = string.Format("{0}://{1}{2}", HttpContext.Request.Scheme, Request.Headers["host"], "/Management/RestartPasswordFormPatient?token=" + oPatient.token);

                string htmlBody = string.Format(content, oPatient.Email, url);

                Email emailDTO = new Email()
                {
                    recipient = oPatient.Email,
                    subject = "Reset Password MediLink",
                    body = htmlBody
                };

                bool emailSent = Services.EmailService.SendEmail(emailDTO);

                if (emailSent)
                {
                    TempData["mensajeResultLogin"] = "Your password has been reset and a password reset request has been sent to your account. Please complete the request to access your account again";
                }



            }
            else
            {
                TempData["mensajeResultLogin"] = "User not found";


            }

            return RedirectToAction("LoginPatient", "Management");



        }

        public ActionResult RestartPasswordFormPatient(string token)
        {

            ViewBag.token = token;


            return View();


        }

        [HttpPost]
        public async Task<IActionResult> RestartPasswordFormPatient(PatientNewRequest oPatientNew)
        {
                                   
            if (oPatientNew.Password != oPatientNew.ConfirmPassword)
            {
                TempData["mensajeResultLogin"] = "Password does not match";

                return RedirectToAction("LoginPatient", "Management");

            }

            oPatientNew.Password = Utilities.EncryptPassword(oPatientNew.Password);

            Patient oPatient = await _userService.UpdatePasswordPatient(oPatientNew);

            if (oPatient != null)
            {
                TempData["mensajeResultLogin"] = "Password has been updated";
            }
            else
            {
                TempData["mensajeResultLogin"] = "User not found";
            }

            return RedirectToAction("LoginPatient", "Management");


        }


        // ------------------------- Practitioner --------------------------------------------------

        public ActionResult LoginPractitioner()
        {
            string mensajeResultLog = TempData["mensajeResultLoginPract"] as string;

            ViewData["mensajeResultLoginPractitioner"] = mensajeResultLog;

            return View();

        }

        [HttpPost]
        public async Task<IActionResult> LoginPractitioner(string Email, string Password)
        {

            Practitioner practFound = await _userService.GetPractitioner(Email, Utilities.EncryptPassword(Password));

            if (practFound == null)
            {
                ViewData["mensajeResultLoginPractitioner"] = "Practitioner Not Found";
                return View();
            }


            if (!practFound.IsValidated)
            {
                ViewData["mensajeResultLoginPractitioner"] = "Your email has not been confirmed yet. An email has been sent to your account";
                return View();
            }

            if (practFound.passwordReset)
            {
                ViewData["mensajeResultLoginPractitioner"] = "A password reset has been requested for your account and an email has been sent to your account. Please go to the email and reset your password";
                return View();
            }


            List<Claim> claims = new List<Claim>() {
                new Claim(ClaimTypes.Name, practFound.Email)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
                );

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> RegisterPractitioner()
        {
            //get a list all the practitioners tyoes
            List<PractitionerType> practitionerTypes = await _mediLinkContext.PractitionerTypes.ToListAsync();

            PractitionerNewRequest oPractitioner = new PractitionerNewRequest();

            oPractitioner.practitionerTypes = practitionerTypes;

            return View(oPractitioner);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterPractitioner(PractitionerNewRequest oPractict)
        {
            //verify if the patient already exist 
            Practitioner practFound = await _userService.GetPractitionerByEmail(oPractict.Email);

            //get a list all the practitioners tyoes
            List<PractitionerType> practitionerTypes = await _mediLinkContext.PractitionerTypes.ToListAsync();


            // use the DB contet to query for all Address entities and transform them into
            // OfficeInf bjects:
            List<OfficeInfo> offices = await _mediLinkContext.OfficeAddresses
                    .Include(t => t.OfficeType)                   
                    .OrderByDescending(t => t.StreetAddress)
                    .Select(t => new OfficeInfo()
                    {
                        fullAddress = t.StreetAddress + " " + t.City + " " + t.PostalCode,
                        OfficeName = t.OfficeType.OfficeName
                       
                    })
                    .ToListAsync();




            if (practFound != null)
            {
                ViewData["MessageRegisterPract"] = "User Already Exist";
                return View();
            }

            List<string> errorMessage = new List<string>();
                       

            // Regular expression for phone validation
            string phonePattern = @"^\d{3}-\d{3}-\d{4}$";

            // Creating Regex object
            Regex regex = new Regex(phonePattern);

           
            if (string.IsNullOrEmpty(oPractict.FirstName) || string.IsNullOrWhiteSpace(oPractict.FirstName))
            {
                errorMessage.Add("-Please enter your first name");
            }

            if (string.IsNullOrEmpty(oPractict.LastName) || string.IsNullOrWhiteSpace(oPractict.LastName))
            {
                errorMessage.Add("-Please enter your last name");
            }

            if (string.IsNullOrEmpty(oPractict.Email) || string.IsNullOrWhiteSpace(oPractict.Email))
            {
                errorMessage.Add("-Please enter your email");
            }



            if (oPractict.Password != oPractict.ConfirmPassword)
            {
                errorMessage.Add("-Password does not match");

            }

            if (string.IsNullOrEmpty(oPractict.Password) && string.IsNullOrEmpty(oPractict.ConfirmPassword))
            {
                errorMessage.Add("-Please enter a Password ");

            }

            if (string.IsNullOrEmpty(oPractict.PhoneNumber) && string.IsNullOrEmpty(oPractict.PhoneNumber))
            {
                errorMessage.Add("-Please enter your phone number ");

            }
            else
            {
                // Matching the number against the regex pattern
                if (!regex.IsMatch(oPractict.PhoneNumber))
                {
                    errorMessage.Add("-Phone number not valid, please use the following input format: 222-222-2222");


                }
            }

            // valid if selected a gender
            if (oPractict.gender == null)
            {
                errorMessage.Add("-Must select a gender ");


            }

            // valid if selected a type
            if (oPractict.PractitionerTypesId == null)
            {
                errorMessage.Add("-Must select a practitioner type ");


            }

            //assign list of practitioner types
            oPractict.practitionerTypes = practitionerTypes;

            if (errorMessage.Count == 0)
            {
               
                oPractict.Password = Utilities.EncryptPassword(oPractict.Password);

                oPractict.token = Utilities.GenerateToken();

                PractitionerNewRequest userCreated = await _userService.SavePractitioner(oPractict);

                if (userCreated.Id > 0)
                {
                    // Map the path relative to the content root
                    string path = Path.Combine(_webHostEnvironment.ContentRootPath, "Templates", "ConfirmEmail.html");

                    string content = System.IO.File.ReadAllText(path);
                    string url = string.Format("{0}://{1}{2}", HttpContext.Request.Scheme, Request.Headers["host"], "/Management/ConfirmRegisterPractitioner?token=" + oPractict.token);

                    string htmlBody = string.Format(content, oPractict.LastName, url);

                    Email emailDTO = new Email()
                    {
                        recipient = oPractict.Email,
                        subject = "Confirmation Email MediLink",
                        body = htmlBody
                    };

                    bool sent = EmailService.SendEmail(emailDTO);

                    errorMessage.Add($"Your account has been created. We have sent a message to the email {oPractict.Email} to confirm your email address");
                     

                }
                else
                {
                    errorMessage.Add("Can not create user");
          
                }

               
              

                // Pass the error list to the view using ViewBag
                ViewBag.MyErrorList = errorMessage;

                ViewData["MessageRegisterPract"] = errorMessage;

                PractitionerNewRequest oNewPract = new PractitionerNewRequest();
                oNewPract.FirstName = "";
                oNewPract.LastName = "";
                oNewPract.Email = "";
                oNewPract.Password = "";
                oNewPract.ConfirmPassword = "";
                oNewPract.practitionerTypes = practitionerTypes;
                oNewPract.officeInfo = offices;


                return View(oNewPract);

            }
            else
            {
                // Pass the error list to the view using ViewBag
                ViewBag.MyErrorList = errorMessage;
               
                ViewData["MessageRegisterPract"] = errorMessage;

                return View(oPractict);
            }


            



           


        }

        //confirm tha the email was sent to activate the account
        public async Task<IActionResult> ConfirmRegisterPractitioner(string token)
        {

            Practitioner oPractitioner = await _userService.ConfirmRegisterPractitioner(token);

            if (oPractitioner != null)
            {
                ViewBag.Response = oPractitioner.IsValidated;
            }
            else
            {
                ViewBag.Response = false;
            }


            return View();

        }

        public async Task<IActionResult> RestartPasswordPractitioner(string email)
        {
            Practitioner oPractitioner = await _userService.ResetPasswordPractitioner(email);

            if (oPractitioner != null)
            {

                string path = Path.Combine(_webHostEnvironment.ContentRootPath, "Templates", "RestorePassword.html");
                string content = System.IO.File.ReadAllText(path);
                string url = string.Format("{0}://{1}{2}", HttpContext.Request.Scheme, Request.Headers["host"], "/Management/RestartPasswordFormPractitioner?token=" + oPractitioner.token);

                string htmlBody = string.Format(content, oPractitioner.Email, url);

                Email emailDTO = new Email()
                {
                    recipient = oPractitioner.Email,
                    subject = "Reset Password MediLink",
                    body = htmlBody
                };

                bool emailSent = Services.EmailService.SendEmail(emailDTO);

                if (emailSent)
                {
                    TempData["mensajeResultLoginPract"] = "Your password has been reset and a password reset request has been sent to your account. Please complete the request to access your account again";
                }



            }
            else
            {
                TempData["mensajeResultLoginPract"] = "User not found";


            }

            return RedirectToAction("LoginPractitioner", "Management");



        }

        public ActionResult RestartPasswordFormPractitioner(string token)
        {

            ViewBag.token = token;


            return View();


        }

        [HttpPost]
        public async Task<IActionResult> RestartPasswordFormPractitioner(PractitionerNewRequest oPractitionerNew)
        {

            if (oPractitionerNew.Password != oPractitionerNew.ConfirmPassword)
            {
                TempData["mensajeResultLoginPract"] = "Password not match";

                return RedirectToAction("LoginPractitioner", "Management");

            }

            oPractitionerNew.Password = Utilities.EncryptPassword(oPractitionerNew.Password);

            Practitioner oPatient = await _userService.UpdatePasswordPractitioner(oPractitionerNew);

            if (oPatient != null)
            {
                TempData["mensajeResultLoginPract"] = "Password has been updated";
            }
            else
            {
                TempData["mensajeResultLoginPract"] = "User not found";
            }

            return RedirectToAction("LoginPractitioner", "Management");


        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
    }
}
