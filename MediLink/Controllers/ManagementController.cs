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
            if (string.IsNullOrEmpty(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ViewData["mensajeResultLoginPatient"] = "Please insert a valid email and password";
                return View();
            }

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
                errorMessage.Add($"User already exists");

                ViewBag.MyErrorList = errorMessage;
                ViewData["MessageRegister"] = errorMessage;
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
            string mensajeResultLog = TempData["mensajeResultRestart"] as string;
            ViewData["mensajeResultRestartPatient"] = mensajeResultLog;

            ViewBag.token = token;


            return View();


        }

        [HttpPost]
        public async Task<IActionResult> RestartPasswordFormPatient(PatientNewRequest oPatientNew)
        {

            if (oPatientNew.Password != oPatientNew.ConfirmPassword)
            {
                TempData["mensajeResultRestart"] = "Password does not match";

                return RedirectToAction("RestartPasswordFormPatient", new { token = oPatientNew.token });

            }

            if (oPatientNew.Password == null || oPatientNew.ConfirmPassword == null)
            {
                TempData["mensajeResultRestart"] = "Password not valid";

                return RedirectToAction("RestartPasswordFormPatient", new { token = oPatientNew.token });

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
            if (string.IsNullOrEmpty(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ViewData["mensajeResultLoginPractitioner"] = "Please insert a valid email and password";
                return View();
            }

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

            TempData["practictionerUser"] = practFound.Email;

            return RedirectToAction("PractitionerHomePage", "Practitioner");
        }

        public async Task<IActionResult> RegisterPractitioner()
        {
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
                        OfficeName = t.OfficeName,
                        OfficeTypeName = t.OfficeType.OfficeTypeName,
                        Id = t.Id

                    })
                    .ToListAsync();

            // use the DB contet to query for all languages entities and transform them into
            // Language objects:
            List<Languages> languages = await _mediLinkContext.Languages.ToListAsync();


            PractitionerNewRequest oPractitioner = new PractitionerNewRequest();

            oPractitioner.practitionerTypes = practitionerTypes;
            oPractitioner.officeInfo = offices;
            oPractitioner.languagesInfo = languages;

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
                        OfficeName = t.OfficeName,
                        OfficeTypeName = t.OfficeType.OfficeTypeName,
                        Id = t.Id

                    })
                    .ToListAsync();

            // use the DB contet to query for all languages entities and transform them into
            // Language objects:
            List<Languages> languages = await _mediLinkContext.Languages.ToListAsync();

            //set the officess to practitioner instance
            oPractict.officeInfo = offices;
            oPractict.languagesInfo = languages;

            //assign list of practitioner types
            oPractict.practitionerTypes = practitionerTypes;


            List<string> errorMessage = new List<string>();

            if (practFound != null)
            {
               errorMessage.Add("User Already Exist");

                ViewBag.MyErrorList = errorMessage;

                ViewData["MessageRegisterPract"] = errorMessage;

                return View(oPractict);
                
            }


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
            if (oPractict.PractitionerTypesId == null || oPractict.PractitionerTypesId == 0)
            {
                errorMessage.Add("-Must select a practitioner type ");


            }

            if (string.IsNullOrEmpty(oPractict.listOffices))
            {
                errorMessage.Add("-Please Select Office Address");
            }

            if (string.IsNullOrEmpty(oPractict.listLanguages))
            {
                errorMessage.Add("-Please Select Spoken Languages");
            }


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
                oNewPract.languagesInfo = languages;


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
            string mensajeResultLog = TempData["mensajeResultRestartPract"] as string;
            ViewData["mensajeResultRestartPract"] = mensajeResultLog;

            ViewBag.token = token;


            return View();


        }

        [HttpPost]
        public async Task<IActionResult> RestartPasswordFormPractitioner(PractitionerNewRequest oPractitionerNew)
        {

            if (oPractitionerNew.Password != oPractitionerNew.ConfirmPassword)
            {
                TempData["mensajeResultRestartPract"] = "Password does not match";

                return RedirectToAction("RestartPasswordFormPractitioner", new { token = oPractitionerNew.token });

            }

            if (oPractitionerNew.Password == null || oPractitionerNew.ConfirmPassword == null)
            {
                TempData["mensajeResultRestartPract"] = "Password not valid";

                return RedirectToAction("RestartPasswordFormPractitioner", new { token = oPractitionerNew.token });

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

        // ------------------------- Walk-in Clinic --------------------------------------------------


        public ActionResult LoginWalkInClinic()
        {
            string mensajeResultLog = TempData["mensajeResultLoginClinic"] as string;

            ViewData["mensajeResultLoginClinic"] = mensajeResultLog;

            return View();

        }

        [HttpPost]
        public async Task<IActionResult> LoginWalkInClinic(string Email, string Password)
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ViewData["mensajeResultLoginClinic"] = "Please insert a valid email and password";
                return View();
            }

            WalkInClinic clientFound = await _userService.GetWalkInClinic(Email, Utilities.EncryptPassword(Password));
                       
            if (clientFound == null)
            {
                ViewData["mensajeResultLoginClinic"] = "User Not Found";
                return View();
            }
          

            if (!clientFound.IsValidated)
            {
                ViewData["mensajeResultLoginClinic"] = "Your email has not been confirmed yet. An email has been sent to your account";
                return View();
            }

            if (clientFound.passwordReset)
            {
                ViewData["mensajeResultLoginClinic"] = "A password reset has been requested for your account and an email has been sent to your account. Please go to the email and reset your password";
                return View();
            }


            List<Claim> claims = new List<Claim>() {
                new Claim(ClaimTypes.Name, clientFound.Email)
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

            return RedirectToAction("WalkinClinicHomePage", "WalkinClinic");
        }

        public async Task<IActionResult> RegisterWalkInClinic()
        {
            WalkClinicInfo oWalkClinicInfo = new WalkClinicInfo();

            // use the DB contet to query for all languages entities and transform them into
            // Language objects:
            List<Languages> languages = await _mediLinkContext.Languages.ToListAsync();


            oWalkClinicInfo.languagesInfo = languages;

            return View(oWalkClinicInfo);
        }

       
        [HttpPost]
        public async Task<IActionResult> RegisterWalkInClinic(WalkClinicInfo oWalkClinicInfo)
        {
            //verify if the Walk-in Clinic already exist 
            WalkInClinic clinicFound = await _userService.GetWalkInClinicByEmail(oWalkClinicInfo.Email);

            // use the DB contet to query for all languages entities and transform them into
            // Language objects:
            List<Languages> languages = await _mediLinkContext.Languages.ToListAsync();

            List<string> errorMessage = new List<string>();
            //string[] errorMessage;

            if (clinicFound != null)
            {
                errorMessage.Add("-Walk-in Clinic already exists");

                // Pass the error list to the view using ViewBag
                ViewBag.MyErrorList = errorMessage;
                ViewData["MessageRegister"] = errorMessage;
                
                return View(oWalkClinicInfo);
            }


            // Regular expression for phone validation
            string phonePattern = @"^\d{3}-\d{3}-\d{4}$";

            // Creating Regex object
            Regex regex = new Regex(phonePattern);

            if (string.IsNullOrEmpty(oWalkClinicInfo.OfficeName))
            {
                errorMessage.Add("-Please enter office name");
            }


            if (string.IsNullOrEmpty(oWalkClinicInfo.Email) || string.IsNullOrWhiteSpace(oWalkClinicInfo.Email))
            {
                errorMessage.Add("-Please enter email");
            }



            if (oWalkClinicInfo.Password != oWalkClinicInfo.ConfirmPassword)
            {
                errorMessage.Add("-Password does not match");

            }

            if (string.IsNullOrEmpty(oWalkClinicInfo.Password) && string.IsNullOrEmpty(oWalkClinicInfo.ConfirmPassword))
            {
                errorMessage.Add("-Please enter a password");

            }

            if (string.IsNullOrEmpty(oWalkClinicInfo.PhoneNumber) || string.IsNullOrWhiteSpace(oWalkClinicInfo.PhoneNumber))
            {
                errorMessage.Add("-Please enter Phone Number");
            }
            else
            {
                // Matching the number against the regex pattern
                if (!regex.IsMatch(oWalkClinicInfo.PhoneNumber))
                {
                    errorMessage.Add("-Phone number not valid, please use the following input format: 222-222-2222");


                }
            }




            // valid if selected a address
            if (string.IsNullOrEmpty(oWalkClinicInfo.StreetAddress))
            {
                errorMessage.Add("-Please enter street address ");

            }

            // valid if selected a city
            if (string.IsNullOrEmpty(oWalkClinicInfo.City))
            {
                errorMessage.Add("-Please enter city ");

            }

            // valid if selected a city
            if (oWalkClinicInfo.Province == null)
            {
                errorMessage.Add("-Please select a province");

            }

            // valid if selected a ;ostcode
            if (string.IsNullOrEmpty(oWalkClinicInfo.PostalCode))
            {
                errorMessage.Add("-Please enter postal code");

            }

            if (string.IsNullOrEmpty(oWalkClinicInfo.listLanguages))
            {
                errorMessage.Add("-Please Select Spoken Languages");
            }



            // valid if there are errors
            if (errorMessage.Count == 0)
            {
                oWalkClinicInfo.Password = Utilities.EncryptPassword(oWalkClinicInfo.Password);

                oWalkClinicInfo.token = Utilities.GenerateToken();

                WalkClinicInfo walkinCreated = await _userService.SaveWalkInClinic(oWalkClinicInfo);

                if (walkinCreated.Id > 0)
                {
                    // Map the path relative to the content root
                    string path = Path.Combine(_webHostEnvironment.ContentRootPath, "Templates", "ConfirmEmail.html");

                    string content = System.IO.File.ReadAllText(path);
                    string url = string.Format("{0}://{1}{2}", HttpContext.Request.Scheme, Request.Headers["host"], "/Management/ConfirmRegisterWalkinClinic?token=" + oWalkClinicInfo.token);

                    string htmlBody = string.Format(content, oWalkClinicInfo.OfficeName, url);

                    Email emailDTO = new Email()
                    {
                        recipient = oWalkClinicInfo.Email,
                        subject = "Confirmation Email MediLink",
                        body = htmlBody
                    };

                    bool sent = EmailService.SendEmail(emailDTO);
                    errorMessage.Add($"Your account has been created. We have sent a message to the email {oWalkClinicInfo.Email} to confirm your email address");


                }
                else
                {
                    errorMessage.Add("Can not create user");

                }

                ViewBag.MyErrorList = errorMessage;
                ViewData["MessageRegister"] = errorMessage;


                WalkClinicInfo oWalkClinicInfo2 = new WalkClinicInfo();
                oWalkClinicInfo2.OfficeName = "";
                oWalkClinicInfo2.Email = "";
                oWalkClinicInfo2.Password = "";
                oWalkClinicInfo2.ConfirmPassword = "";
                oWalkClinicInfo2.languagesInfo = languages;

                return View(oWalkClinicInfo2);

            }
            else
            {
                // Pass the error list to the view using ViewBag
                ViewBag.MyErrorList = errorMessage;
                ViewData["MessageRegister"] = errorMessage;
                oWalkClinicInfo.languagesInfo = languages;
                return View(oWalkClinicInfo);
            }









        }

        //confirm tha the email was sent to activate the Walkin Clinic account
        public async Task<IActionResult> ConfirmRegisterWalkinClinic(string token)
        {

            WalkInClinic oWalkInClinic = await _userService.ConfirmRegisterWalkinClinic(token);

            if (oWalkInClinic != null)
            {
                ViewBag.Response = oWalkInClinic.IsValidated;
            }
            else
            {
                ViewBag.Response = false;
            }


            return View();

        }


        public async Task<IActionResult> RestartPasswordWalkinClinic(string email)
        {
            WalkInClinic oWalkInClinic = await _userService.ResetPasswordWalkinClinic(email);

            if (oWalkInClinic != null)
            {

                string path = Path.Combine(_webHostEnvironment.ContentRootPath, "Templates", "RestorePassword.html");
                string content = System.IO.File.ReadAllText(path);
                string url = string.Format("{0}://{1}{2}", HttpContext.Request.Scheme, Request.Headers["host"], "/Management/RestartPasswordFormWalkinClinic?token=" + oWalkInClinic.token);

                string htmlBody = string.Format(content, oWalkInClinic.Email, url);

                Email emailDTO = new Email()
                {
                    recipient = oWalkInClinic.Email,
                    subject = "Reset Password MediLink",
                    body = htmlBody
                };

                bool emailSent = Services.EmailService.SendEmail(emailDTO);

                if (emailSent)
                {
                    TempData["mensajeResultLoginClinic"] = "Your password has been reset and a password reset request has been sent to your account. Please complete the request to access your account again";
                }



            }
            else
            {
                TempData["mensajeResultLoginClinic"] = "User not found";


            }

            return RedirectToAction("LoginWalkInClinic", "Management");



        }

        public ActionResult RestartPasswordFormWalkinClinic(string token)
        {
            string mensajeResultLog = TempData["mensajeResultLoginClinic"] as string;
            ViewData["mensajeResultLoginCli"] = mensajeResultLog;

            ViewBag.token = token;


            return View();


        }

        [HttpPost]
        public async Task<IActionResult> RestartPasswordFormWalkinClinic(WalkClinicInfo oWalkClinicInfo)
        {

            if (oWalkClinicInfo.Password != oWalkClinicInfo.ConfirmPassword)
            {
                TempData["mensajeResultLoginClinic"] = "Password does not match";

                return RedirectToAction("RestartPasswordFormWalkinClinic", new { token = oWalkClinicInfo.token });

            }

            if (oWalkClinicInfo.Password == null || oWalkClinicInfo.ConfirmPassword == null)
            {
                TempData["mensajeResultLoginClinic"] = "Password not valid";

                return RedirectToAction("RestartPasswordFormWalkinClinic", new { token = oWalkClinicInfo.token });

            }

            oWalkClinicInfo.Password = Utilities.EncryptPassword(oWalkClinicInfo.Password);

            WalkInClinic oWalkInClinic = await _userService.UpdatePasswordWalkinClinic(oWalkClinicInfo);

            if (oWalkInClinic != null)
            {
                TempData["mensajeResultLoginClinic"] = "Password has been updated";
            }
            else
            {
                TempData["mensajeResultLoginClinic"] = "User not found";
            }

            return RedirectToAction("LoginWalkInClinic", "Management");


        }







        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
    }
}
