using System;
using System.Collections.Generic;
using WorldNews.Core.Entities;
using WorldNews.Data.Contracts;
using WorldNews.Logic.Contracts;
using WorldNews.Logic.DTO.Profile;
using WorldNews.Logic.Infrastructure;

namespace WorldNews.Logic.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUnitOfWork unitOfWork;

        public ProfileService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public DataServiceMessage<ProfileBaseDTO> GetAdminProfile(string login)
        {
            List<string> errors = new List<string>();
            bool succeeded = true;
            ProfileBaseDTO data = null;

            try
            {
                ApplicationUser applicationUser = unitOfWork.Profiles.FindByLogin(login);
                if (applicationUser != null)
                {
                    if (unitOfWork.Profiles.IsInRole(applicationUser.Id, Roles.AdminRole))
                    {
                        data = new ProfileBaseDTO
                        {
                            Email = applicationUser.Email,
                            Login = applicationUser.UserName,
                            FirstName = applicationUser.FirstName,
                            LastName = applicationUser.LastName
                        };
                    }
                    else
                    {
                        succeeded = false;
                        errors.Add("Access denied");
                    }
                }
                else
                {
                    succeeded = false;
                    errors.Add("Admin with such login was not found");
                }
            }
            catch (Exception ex)
            {
                ExceptionMessageBuilder.FillErrors(ex, errors);
                succeeded = false;
            }

            return new DataServiceMessage<ProfileBaseDTO>
            {
                Succeeded = succeeded,
                Errors = errors,
                Data = data
            };
        }

        public ServiceMessage UpdateAdminProfile(ProfileBaseDTO profileDTO)
        {
            List<string> errors = new List<string>();
            bool succeeded = Validate(profileDTO, errors);

            if (succeeded)
            {
                try
                {
                    ApplicationUser applicationUser = unitOfWork.Profiles.FindByLogin(profileDTO.Login);
                    if (applicationUser != null)
                    {
                        if (unitOfWork.Profiles.IsInRole(applicationUser.Id, Roles.AdminRole))
                        {
                            applicationUser.Email = profileDTO.Email;
                            applicationUser.FirstName = profileDTO.FirstName;
                            applicationUser.LastName = profileDTO.LastName;

                            unitOfWork.Commit();
                        }
                        else
                        {
                            succeeded = false;
                            errors.Add("Access denied");
                        }
                    }
                    else
                    {
                        succeeded = false;
                        errors.Add("Admin with such login was not found");
                    }
                }
                catch (Exception ex)
                {
                    ExceptionMessageBuilder.FillErrors(ex, errors);
                    succeeded = false;
                }
            }

            return new ServiceMessage
            {
                Succeeded = succeeded,
                Errors = errors
            };
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }

        #region Validation
        private bool Validate(ProfileBaseDTO profileDTO, ICollection<string> errors)
        {
            bool isValid = true;

            if (String.IsNullOrEmpty(profileDTO.Login))
            {
                isValid = false;
                errors.Add("Login cannot be empty");
            }

            if (String.IsNullOrEmpty(profileDTO.Email))
            {
                isValid = false;
                errors.Add("E-Mail cannot be empty");
            }

            if (String.IsNullOrEmpty(profileDTO.FirstName))
            {
                isValid = false;
                errors.Add("First name cannot be empty");
            }

            if (String.IsNullOrEmpty(profileDTO.LastName))
            {
                isValid = false;
                errors.Add("Last name cannot be empty");
            }

            return isValid;
        }
        #endregion
    }
}
