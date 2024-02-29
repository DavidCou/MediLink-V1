
﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MediLink.Entities;
using MediLink.Models;
using MediLink.Services.Contract;
using NuGet.Common;

namespace MediLink.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly MediLinkDbContext _dbContext;
        public UserService(MediLinkDbContext dbContext)
        {
            _dbContext = dbContext;
        }     

        public async Task<Patient> GetUser(string email, string password)
        {
            Patient userFound = await _dbContext.Patients.Where(p => p.Email == email && p.Password == password)
                 .FirstOrDefaultAsync();

            return userFound;
        }


        public async Task<Patient> GetUserByEmail(string email)
        {
            Patient userFound = await _dbContext.Patients.Where(p => p.Email == email)
                 .FirstOrDefaultAsync();

            return userFound;
        }
        

         public async Task<Patient> ConfirmRegisterPatient(string token)
        {
            Patient userFound = await _dbContext.Patients.Where(p => p.token== token)
                 .FirstOrDefaultAsync();

            if (userFound != null)
            {
                // Update the value
                userFound.IsEmailConfirmed = true;

                // Save the changes back to the database
                _dbContext.SaveChanges();

                return userFound;
            }
            else
            {
                return userFound;
            }
        }

        public async Task<Patient> ResetPasswordPatient(string email)
        {
            Patient userFound = await _dbContext.Patients.Where(p => p.Email == email)
                 .FirstOrDefaultAsync();

            if (userFound != null)
            {
                // Update the value
                userFound.passwordReset = true;

                // Save the changes back to the database
                _dbContext.SaveChanges();

                return userFound;
            }
            else
            {
                return userFound;
            }
        }


        public async Task<Patient> UpdatePasswordPatient(PatientNewRequest oPatient)
        {
            Patient userFound = await _dbContext.Patients.Where(p => p.token == oPatient.token)
                 .FirstOrDefaultAsync();

            if (userFound != null)
            {
                // Update the value
                userFound.Password = oPatient.Password;
                userFound.passwordReset = false;

                // Save the changes back to the database
                _dbContext.SaveChanges();

                return userFound;
            }
            else
            {
                return userFound;
            }
        }
        


        public async Task<PatientNewRequest> SavePatient(PatientNewRequest oPatientNew)
        {
            //create patients address
            PatientAddress patientAddress = new PatientAddress();

            patientAddress.City = oPatientNew.City;
            patientAddress.StreetAddress = oPatientNew.StreetAddress;
            patientAddress.PostalCode = oPatientNew.PostalCode;
            patientAddress.Province = oPatientNew.Province;
            patientAddress.country = oPatientNew.country;
            _dbContext.PatientAddress.Add(patientAddress);

            await _dbContext.SaveChangesAsync();

            //create patient details
            PatientDetail patientDetail = new PatientDetail();
            patientDetail.FirstName = oPatientNew.FirstName;
            patientDetail.LastName = oPatientNew.LastName;
            patientDetail.gender = oPatientNew.gender;
            patientDetail.PhoneNumber = oPatientNew.PhoneNumber;
            patientDetail.PatientAddressesId = patientAddress.Id;
            _dbContext.PatientDetails.Add(patientDetail);

            await _dbContext.SaveChangesAsync();

            //create patient
            Patient patient = new Patient();
            patient.PatientDetailsId = patientDetail.Id;
            patient.Email = oPatientNew.Email;
            patient.Password = oPatientNew.Password;
            patient.token = oPatientNew.token;
            _dbContext.Patients.Add(patient);


            await _dbContext.SaveChangesAsync();

            oPatientNew.Id = patient.Id;

            return oPatientNew;
        }


        // ------------------------ Practitioners -----------------------------------------

        public async Task<Practitioner> GetPractitioner(string email, string password)
        {
            Practitioner userFound = await _dbContext.Practitioners.Where(p => p.Email == email && p.Password == password)
                 .FirstOrDefaultAsync();

            return userFound;
        }

        public async Task<Practitioner> GetPractitionerByEmail(string email)
        {
            Practitioner userFound = await _dbContext.Practitioners.Where(p => p.Email == email)
                 .FirstOrDefaultAsync();

            return userFound;
        }

        public async Task<Practitioner> ConfirmRegisterPractitioner(string token)
        {
            Practitioner userFound = await _dbContext.Practitioners.Where(p => p.token == token)
                 .FirstOrDefaultAsync();

            if (userFound != null)
            {
                // Update the value
                userFound.IsValidated = true;

                // Save the changes back to the database
                _dbContext.SaveChanges();

                return userFound;
            }
            else
            {
                return userFound;
            }
        }

        public async Task<Practitioner> ResetPasswordPractitioner(string email)
        {
            Practitioner userFound = await _dbContext.Practitioners.Where(p => p.Email == email)
                 .FirstOrDefaultAsync();

            if (userFound != null)
            {
                // Update the value
                userFound.passwordReset = true;

                // Save the changes back to the database
                _dbContext.SaveChanges();

                return userFound;
            }
            else
            {
                return userFound;
            }
        }

        public async Task<Practitioner> UpdatePasswordPractitioner(PractitionerNewRequest oDoctor)
        {
            Practitioner userFound = await _dbContext.Practitioners.Where(p => p.token == oDoctor.token)
                 .FirstOrDefaultAsync();

            if (userFound != null)
            {
                // Update the value
                userFound.Password = oDoctor.Password;
                userFound.passwordReset = false;

                // Save the changes back to the database
                _dbContext.SaveChanges();

                return userFound;
            }
            else
            {
                return userFound;
            }
        }

        public async Task<PractitionerNewRequest> SavePractitioner(PractitionerNewRequest oDoctor)
        {
            //create patient instance
            Practitioner practitioner = new Practitioner();

            //create PractitionerOfficeAddress instance
            List<PractitionerOfficeAddress> practitionerOfficeAddress = new List<PractitionerOfficeAddress>();

            //assing the value to the new instance created
            practitioner.Email = oDoctor.Email;
            practitioner.Password = oDoctor.Password;
            practitioner.FirstName = oDoctor.FirstName;
            practitioner.LastName = oDoctor.LastName;
            practitioner.gender = oDoctor.gender;
            practitioner.PhoneNumber = oDoctor.PhoneNumber;
            practitioner.token = oDoctor.token;
            practitioner.PractitionerTypeId = oDoctor.PractitionerTypesId;
            _dbContext.Practitioners.Add(practitioner);


            await _dbContext.SaveChangesAsync();

            //get the last practitioner id created 
            oDoctor.Id = practitioner.Id;

            //verify if the user add offices address to the new practitioner
            if (oDoctor.listOffices != null)
            {
                //convert the string to array to iterate over each office id added
                string[] offices = oDoctor.listOffices.Split(",");

                //iterate over each office id added and save the address
                foreach (string office in offices)
                {
                    int idOffice = Convert.ToInt32(office);

                    practitionerOfficeAddress.Add(new PractitionerOfficeAddress { PractitionerId = oDoctor.Id, OfficeAddressesId = idOffice });


                }

                _dbContext.PractitionerAddresses.AddRange(practitionerOfficeAddress);

                await _dbContext.SaveChangesAsync();


            }

            return oDoctor;
        }


    }
}
